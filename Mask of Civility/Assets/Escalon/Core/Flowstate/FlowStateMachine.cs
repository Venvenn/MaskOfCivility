using System.Collections.Generic;

namespace Escalon
{
    /// <summary>
    /// State machine that controls the flow between states as well as what stage a state is in 
    /// </summary>
    public class FlowStateMachine
    {
        private enum CollectionAction
        {
            Add,
            Remove
        }

        /// <summary>
        /// This machines position in the overall FSM hierarchy
        /// </summary>
        public readonly TreeNode<FlowStateMachine> StateTreeNode;

        /// <summary>
        /// This machines position in the overall FSM hierarchy
        /// </summary>
        public readonly FlowState OwningState;
        
        /// <summary>
        /// Aspect container used to easily access systems from states.
        /// </summary>
        public readonly Container Container;
        
        /// <summary>
        /// queue used to execute actions in order, so that state actions are accurately performed in the order they are intended to be
        /// </summary>
        private readonly Queue<(CollectionAction action, FlowState state)> _stateActionQueue = new Queue<(CollectionAction, FlowState)>();

        /// <summary>
        /// A queue of notifications to send, used to be abel to send notifications in sync with state stack changes
        /// </summary>
        private readonly Queue<(object sender, string message, object args)> _notificationQueue = new Queue<(object sender, string message, object args)>();
        
        /// <summary>
        /// The state stack is used to store all the states owned by this fsm, the top one being the currently active state
        /// </summary>
        private readonly Stack<FlowState> _stateStack = new Stack<FlowState>();
        
        private ViewManager _viewManager => Container.GetAspect<ViewManager>();
        
        /// <summary>
        /// The number of states currently on the state stack
        /// </summary>
        public int StateCount => _stateStack.Count;

        public bool Empty => StateCount == 0 && _stateActionQueue.Count == 0 && _notificationQueue.Count == 0;
        
        /// <summary>
        /// The number of states currently on the state stack
        /// </summary>
        public FlowState ActiveState => _stateStack.Count > 0 ? _stateStack.Peek() : null;
        
        public FlowStateMachine(FlowState owningState = null, Container container = null)
        {
            StateTreeNode = new TreeNode<FlowStateMachine>(this);
            Container = container ?? new Container();
            OwningState = owningState;
            if (owningState != null)
            {
                StateTreeNode.AddParent(owningState.FlowStateMachine.StateTreeNode);
            }
        }

        /// <summary>
        /// UpdateProcessor the state on the top of the state stack
        /// </summary>
        public void Update()
        {
            UpdateActiveState();
        }

        /// <summary>
        /// UpdateProcessor the state on the top of the state stack
        /// </summary>
        private void UpdateActiveState()
        {
            if (_stateStack.Count > 0)
            {
                FlowState flowState = _stateStack.Peek();
                switch (flowState.Stage)
                {
                    case FlowState.StateStage.Presenting:
                    {
                        bool presented = true;
                        if (_viewManager != null)
                        {
                            presented = !_viewManager.IsViewInitialising(flowState.StateId) && !_viewManager.IsViewTransitioning(flowState.StateId);
                        }   
                        
                        FlowState.TransitionState transitionStatePlatform = flowState.UpdateInitialisePlatform();
                        FlowState.TransitionState transitionState = flowState.UpdateInitialise();
                        if (presented && transitionStatePlatform == FlowState.TransitionState.Completed &&
                            transitionState == FlowState.TransitionState.Completed)
                        {
                            flowState.OnFinishInitialisePlatform();
                            flowState.OnFinishInitialise();
                            ActivateState(flowState);
                        }
                        break;
                    }
                    case FlowState.StateStage.Active:
                    {
                        flowState.ActiveUpdatePlatform();
                        flowState.ActiveUpdate();
                        _viewManager?.UpdateView(flowState.StateId);
                        UpdateStateStack();
                        break;
                    }
                    case FlowState.StateStage.Dismissing:
                    {
                        bool dismissed = !_viewManager.IsViewTransitioning(flowState.StateId);   
                        
                        FlowState.TransitionState transitionStatePlatform = flowState.UpdateDismissPlatform();
                        FlowState.TransitionState transitionState = flowState.UpdateDismiss();
                        if (dismissed && transitionStatePlatform == FlowState.TransitionState.Completed && transitionState == FlowState.TransitionState.Completed)
                        {
                            flowState.OnFinishDismissPlatform();
                            flowState.OnFinishDismiss();
                            DeactivateState(flowState);
                            flowState.RemoveReceiver();
                            
                            _viewManager?.DisposeView(flowState.StateId);
                            
                            PopState();
                        }
                        break;
                    }
                }
            }
            else
            {
                UpdateStateStack();
            }
        }

        /// <summary>
        /// Performs a fixed update for the top state
        /// </summary>
        public void FixedUpdate()
        {
            if (_stateStack.Count > 0 && _stateStack.Peek().Stage == FlowState.StateStage.Active)
            {
                _stateStack.Peek().ActiveFixedUpdate();
            }
        }

        /// <summary>
        /// Queues a notification to be run when the state action queue is empty
        /// </summary>
        public void QueueNotification(string message, object args = null)
        {
            QueueNotification(this, message, args);
        }
        
        /// <summary>
        /// Queues a notification to be run when the state action queue is empty
        /// </summary>
        public void QueueNotification(object sender, string message, object args = null)
        {
            _notificationQueue.Enqueue((sender, message, args));
        }
        
        /// <summary>
        /// pops the state on top of the state stack 
        /// </summary>
        public void Pop()
        {
            _stateActionQueue.Enqueue((CollectionAction.Remove , null));
        }

        /// <summary>
        /// pushes a new state onto the top of the stack 
        /// </summary>
        public void Push(FlowState flowState)
        {
            _stateActionQueue.Enqueue((CollectionAction.Add , flowState));
        }

        /// <summary>
        /// returns the currently active flow state
        /// </summary>
        public FlowState GetTopState()
        {
            if (_stateStack.Count > 0)
            {
                return _stateStack.Peek();
            }

            return null;
        }

        /// <summary>
        /// performs stack actions if there are any to perform
        /// </summary>
        protected void UpdateStateStack()
        {
            if (_stateActionQueue.Count > 0)
            {
                var actionItem = _stateActionQueue.Dequeue();
                switch (actionItem.action)
                {
                    case CollectionAction.Remove:
                        if (_stateStack.Count > 0)
                        {
                            FlowState state = _stateStack.Peek();
                            state.OnStartDismiss();
                            state.Stage = FlowState.StateStage.Dismissing;
                            _viewManager?.DismissView(state.StateId);
                        }
                        break;
                    case CollectionAction.Add:
                    {
                        PushState(actionItem.state); 
                        break;
                    }
                }
            }
            else
            {
                UpdateNotificationQueue();
            }
        }
        
        protected void UpdateNotificationQueue()
        {
            while (_notificationQueue.Count > 0)
            {
                (object sender, string message, object args) = _notificationQueue.Dequeue();
                sender.PostNotification(message, args);
            }
        }
        
        /// <summary>
        /// pushes a new state onto the top of the stack 
        /// </summary>
        protected void PushState(FlowState flowState)
        {
            if (_stateStack.Count > 0)
            {
                DeactivateState(_stateStack.Peek());
            }

            _stateStack.Push(flowState);

            flowState.FlowStateMachine = this;
            flowState.Stage = FlowState.StateStage.Presenting;
            flowState.OnStartInitialise();
            
            _viewManager?.PresentView(flowState);
        }

        /// <summary>
        /// pops the state on top of the state stack 
        /// </summary>
        protected void PopState()
        {
            if (_stateStack.Count > 0)
            {
                _stateStack.Pop();

                if (_stateStack.Count > 0)
                {
                    ActivateState(_stateStack.Peek());
                }
            }
        }

        private void ActivateState(FlowState flowState)
        {
            flowState.OnActive();
            flowState.Stage = FlowState.StateStage.Active;
            _viewManager?.ActivateView(flowState.StateId);
        }
        
        private void DeactivateState(FlowState flowState)
        {
            flowState.Stage = FlowState.StateStage.Inactive;
            flowState.OnInactive();
            _viewManager?.DeactivateView(flowState.StateId);
        }

        /// <summary>
        /// Pops all of the states belonging to this state machine
        /// </summary>
        public void PopAllStates()
        {
            for (int i = 0; i < _stateStack.Count; i++)
            {
                Pop();
            }
        }

        /// <summary>
        /// Updates any IUpdateable Aspects, should only be called once per container
        /// </summary>
        public void UpdateAspects()
        {
            Container.Update();
        }

        public void GetAllActiveChildStates(ref List<FlowState> activeStates)
        {
            if (ActiveState != null)
            {
                IEnumerable<TreeNode<FlowStateMachine>> childNodes = StateTreeNode.Children;
                activeStates.Add(ActiveState);
           
                foreach (var node in childNodes)
                {
                    FlowState activeState = node.Value.ActiveState;
                    if (activeState != null && (activeState.FlowStateMachine.OwningState == null || activeState.FlowStateMachine.OwningState == ActiveState))
                    {
                        activeState.FlowStateMachine.GetAllActiveChildStates(ref activeStates);
                    }
                }
            }
        }
    }
}