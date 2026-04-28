using System.Threading.Tasks;
using Arch.Core;
using Escalon;
using UnityEngine;

public class FSGeneration : FlowState
{
    public const string k_beginGame = "FSGeneration.BeginGame";
    public const string k_reveal = "FSGeneration.Reveal";
    
    private CoreManagers _coreManagers;
    private Entity _map = Entity.Null;

    public override void OnStartInitialise()
    {
        _coreManagers = Container.GetAspect<CoreManagers>();

        GameObject cameraTarget = GameObject.Find("CameraTarget");
        cameraTarget.transform.position = Vector3.up;
        this.AddObserver(PlayGame, k_beginGame);
    }

    public override TransitionState UpdateInitialise()
    {
        if (_coreManagers.Container.GetAspect<ViewManager>().IsViewInitialising(StateId))
        {
            return TransitionState.InProgress;
        }
        return TransitionState.Completed;
    }

    public override async void OnFinishInitialise()
    {
        MapGeneratorDynamicData generatorData = new MapGeneratorDynamicData()
        {
            Prefab = Resources.Load<TileView>("Prefabs/MapTile"),
        };
        
        SelectionData selectionData = new SelectionData()
        {
            HoveredTile = Entity.Null,
            SelectedTile = Entity.Null
        };

        _coreManagers.DataManager.Write(generatorData);
        _coreManagers.DataManager.Write(selectionData);

        Task<Entity> generation = MapGeneratorSystem.Generate(_coreManagers);
        while (!generation.IsCompleted)
        {
            await Task.Yield();
        }
        _map = generation.Result;
        
        this.PostNotification(k_reveal);
    }

    private async void PlayGame(object sender, object args)
    {
        while (_map == Entity.Null)
        {
            await Task.Yield();
        }
        
        FlowStateMachine.Pop();
        FlowStateMachine.Push(new FSGame(_map));
    }
}
