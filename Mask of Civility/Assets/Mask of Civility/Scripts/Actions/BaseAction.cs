
using System;
using Arch.Core;
using Arch.Core.Extensions;
using Escalon;

public abstract class BaseAction
{
    protected TileActionData _data;
    protected CoreManagers _coreManagers;
    protected WorldTimeManager _timeManager;
    protected Entity _target;
    protected Entity _sourceCountry;
    
    public BaseAction(TileActionData data, Entity target, Entity country, CoreManagers coreManagers)
    {
        _data = data;
        _target = target;
        _sourceCountry = country;
        _coreManagers = coreManagers;
        _timeManager = coreManagers.Container.GetAspect<WorldTimeManager>();
    }

    public void Run()
    {
        if (CheckConditions())
        {
            Execute();
            this.PostNotification(WorldTimeManager.k_addTimedCallback, new TimedCallback
            {
                Time = _data.GetDestinationDate(_timeManager.CurrentTime),
                Action = Award
            });
        }
    }
    
    public virtual bool CheckConditions()
    {
        var resourceAmounts = _sourceCountry.Get<CountryData>().ResourceAmounts;
        foreach (var costs in _data.Cost)
        {
            if (resourceAmounts[costs.Key] < costs.Value)
            {
                return false;
            }
        }

        return true;
    }

    public virtual void Execute()
    {
        CountryData countryData = _sourceCountry.Get<CountryData>();
        foreach (var costs in _data.Cost)
        {
            countryData.ResourceAmounts[costs.Key] -= costs.Value;
        }
        _coreManagers.EntityManager.SetComponent(_sourceCountry, countryData);
    }
    
    public virtual void Award()
    {
        CountryTileData countryTileData = _target.Get<CountryTileData>();
        countryTileData.SoftHolder = _target;
        _coreManagers.EntityManager.SetComponent(_target, countryTileData);
        
        CountryData countryData = _sourceCountry.Get<CountryData>();
        countryData.Mask += _data.MaskEffect;
        _coreManagers.EntityManager.SetComponent(_sourceCountry, countryData);
    }
}

public class AttackAction : BaseAction
{
    public AttackAction(TileActionData data, Entity target, Entity country, CoreManagers coreManagers) : base(data, target, country, coreManagers)
    {
    }
    
    // public override bool CheckConditions()
    // {
    //     
    // }

    public override void Award()
    {
        CountryTileData countryTileData = _target.Get<CountryTileData>();
        countryTileData.SoftHolder = _target;
        countryTileData.HardHolder = _target;
        _coreManagers.EntityManager.SetComponent(_target, countryTileData);
        
        CountryData countryData = _sourceCountry.Get<CountryData>();
        countryData.Mask += _data.MaskEffect;
        _coreManagers.EntityManager.SetComponent(_sourceCountry, countryData);
    }
}

public class OilPipeline : BaseAction
{
    public OilPipeline(TileActionData data, Entity target, Entity country, CoreManagers coreManagers) : base(data, target, country, coreManagers)
    {
    }
    
    // public override bool CheckConditions()
    // {
    //     
    // }
}
