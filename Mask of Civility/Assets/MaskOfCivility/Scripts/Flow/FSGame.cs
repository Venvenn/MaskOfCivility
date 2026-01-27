using System;
using Arch.Core;
using Escalon;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FSGame : FlowState
{
    private CoreManagers _coreManagers;
    private Entity _map;

    public override void OnStartInitialise()
    {
        _coreManagers = Container.GetAspect<CoreManagers>();
    }
    
    public override void OnFinishInitialise()
    {
        MapGeneratorData generatorData = new MapGeneratorData()
        {
            Size = new int2(256, 128),
            SeaLevel = 0.5f,
            MapTile = Resources.Load<GameObject>("Prefabs/MapTile")
        };
        NoiseSettings noiseSettings = new NoiseSettings(HeightMapGenerator.NormalizeMode.Local, Vector2.zero, Random.Range(0, int.MaxValue));

        _coreManagers.DataManager.Write(generatorData);
        _coreManagers.DataManager.Write(noiseSettings);
        
        _map = MapGeneratorSystem.Generate(_coreManagers);
    }
}
