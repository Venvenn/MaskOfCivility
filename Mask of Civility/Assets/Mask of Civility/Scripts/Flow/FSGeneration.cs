using System.Collections.Generic;
using Arch.Core;
using Escalon;
using Unity.Mathematics;
using UnityEngine;

public class FSGeneration : FlowState
{
    private CoreManagers _coreManagers;
    private Entity _map;

    public override void OnStartInitialise()
    {
        _coreManagers = Container.GetAspect<CoreManagers>();
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

        _map = await MapGeneratorSystem.Generate(_coreManagers);
        
        FlowStateMachine.Push(new FSGame(_map));
    }
}