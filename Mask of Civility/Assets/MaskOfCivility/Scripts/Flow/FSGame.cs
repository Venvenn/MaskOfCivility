using System;
using Arch.Core;
using Arch.System;
using Escalon;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FSGame : FlowState
{
    private CoreManagers _coreManagers;
    private Entity _map;
    private Group<float> _systems;

    public override void OnStartInitialise()
    {
        _coreManagers = Container.GetAspect<CoreManagers>();
        _systems = new Group<float>("GamePlay", new TileVisualsSystem(_coreManagers.EntityManager.World));
        _systems.Initialize();
    }

    public override async void OnFinishInitialise()
    {
        MapGeneratorData generatorData = new MapGeneratorData()
        {
            Size = new int2(256, 128),
            SeaLevel = 0.5f,
            MapTile = Resources.Load<GameObject>("Prefabs/MapTile"),
            CountryCount = 30
        };
        NoiseSettings noiseSettings = new NoiseSettings(HeightMapGenerator.NormalizeMode.Local, Vector2.zero,
            Random.Range(0, int.MaxValue));

        _coreManagers.DataManager.Write(generatorData);
        _coreManagers.DataManager.Write(noiseSettings);

        _map = await MapGeneratorSystem.Generate(_coreManagers);
    }

    public override void ActiveUpdate()
    {
        _systems.Update(Time.deltaTime);
    }

    public override void OnFinishDismiss()
    {
        _systems.Dispose();
    }
}