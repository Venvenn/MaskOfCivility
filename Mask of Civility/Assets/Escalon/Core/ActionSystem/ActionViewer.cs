using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Arch.Core;

namespace Escalon.ActionSystem
{
    /// <summary>
    /// Used to Visualise the actions performed by the Action System
    /// </summary>
    public class ActionViewer
    {
        public const string k_eventViewed = "ActionViewer.EventViewed";
        public const string k_abilityActionViewed = "ActionViewer.AbilityActionViewed";
        public const string k_abilityActionFinished = "ActionViewer.AbilityActionFinished";
        
        private readonly Dictionary<Type, List<IActionVisualBehaviour>> _actionVisuals = new Dictionary<Type, List<IActionVisualBehaviour>>();
        private readonly Dictionary<Entity, List<IActionVisualBehaviour>> _eventVisuals = new Dictionary<Entity, List<IActionVisualBehaviour>>();
        private readonly Queue<PastEvent> _viewQueue = new Queue<PastEvent>();
        private readonly DataManager _dataManager;
        private bool _visualising;
        
        public bool Running => _viewQueue.Count > 0 || _visualising;

        public ActionViewer(DataManager dataManager, FlowState viewerScope)
        {
            _dataManager = dataManager;
            viewerScope.AddObserver(OnActionPerformed, ActionManager.k_eventExecuted);
        }
        
        public async void PlayNextAction()
        {
            if (_viewQueue.Count > 0 && !_visualising)
            {
                _visualising = true;
                PastEvent pastEvent = _viewQueue.Dequeue();
                Stopwatch timer = new Stopwatch();
                timer.Start();
                await pastEvent.EventSource.PostAwaitableNotification(k_eventViewed, pastEvent);
                await Visualise(pastEvent);
                await pastEvent.EventSource.PostAwaitableNotification(Action.NotificationView(pastEvent.EventTrigger.GetType()), pastEvent.EventTrigger);
                timer.Stop();
                int milliseconds = (int)timer.Elapsed.TotalMilliseconds;
                 if (_dataManager.Read<EventViewTimeData>().EventViewDelays.TryGetValue( pastEvent.EventTrigger.GetType().Name, out var delayTime))
                 {
                     int remainingTime = (int)(delayTime * 1000) - milliseconds;
                     if (remainingTime > 0)
                     {
                         await Task.Delay(remainingTime);
                     }
                 }
                _visualising = false;
            }
        }

        public void AddActionVisual<T>(IActionVisualBehaviour actionVisualBehaviour) where T : IEventTrigger
        {
            Type type = typeof(T);
            _actionVisuals.TryAdd(type, new List<IActionVisualBehaviour>());
            _actionVisuals[type].Add(actionVisualBehaviour);
        }
        
        public void RemoveAction<T>() where T : IActionEffect
        {
            Type type = typeof(T);
            _actionVisuals.Remove(type);
        }

        private void OnActionPerformed(object sender, object args)
        {
            ActionHistoryData historyData = _dataManager.Read<ActionHistoryData>();
            List<PastEvent> pastEvents = (List<PastEvent>)args;
            foreach (var pastEvent in pastEvents)
            {
                _viewQueue.Enqueue(pastEvent);
                historyData.History.AddRange(pastEvent.GetAllActions(true));
            }
        }

        private async Task Visualise(PastEvent pastEvent)
        {
            if (_actionVisuals.TryGetValue(pastEvent.EventTrigger.GetType(), out var eventVisuals))
            {
                foreach (IActionVisualBehaviour actionVisual in eventVisuals)
                {
                    await actionVisual.Execute(pastEvent.EventSource);
                }
            }
            
            foreach (PastSequence sequence in pastEvent.Sequences)
            {
                foreach (PastAction action in sequence.Actions)
                {
                    if(action.IsAbility)
                    {
                        await sequence.SequenceSource.PostAwaitableNotification(k_abilityActionViewed, action);
                    }
                    
                    if (_actionVisuals.TryGetValue(action.Effect.GetType(), out var visuals))
                    {
                        foreach (IActionVisualBehaviour actionVisual in visuals)
                        {
                            await actionVisual.Execute(sequence.SequenceSource);
                        }
                    }

                    foreach (Entity target in action.TargetedEntities)
                    {
                        await target.PostAwaitableNotification(Action.NotificationView(action.Effect.GetType()));
                    }
                    
                    if(action.IsAbility)
                    {
                        await sequence.SequenceSource.PostAwaitableNotification(k_abilityActionFinished, action);
                    }
                }
            }
        }

        public void Clear()
        {
            _actionVisuals.Clear();
            _eventVisuals.Clear();
            _viewQueue.Clear();
            _visualising = false;
        }
    }
}