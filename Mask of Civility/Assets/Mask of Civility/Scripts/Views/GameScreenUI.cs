using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Escalon;
using Nova;
using UnityEngine;

public class GameScreenUI : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<ResourceType, TextBlock> _resources;
    [SerializeField] private Highlighter _highlighter;
    
    private CoreManagers _coreManagers;
    private Dictionary<ResourceType, int> _lastAmount = new Dictionary<ResourceType, int>();

    public void Init(CoreManagers coreManagers)
    {
        _coreManagers = coreManagers;
        Notification.AddObserver<FSGame>(OnDayTick, WorldTimeManager.k_dayTick);
        Notification.AddObserver<FSGame>(UpdateResources, ResourceSystem.k_updatePlayerResources);

        foreach (var kvp in _resources)
        {
            _lastAmount.Add(kvp.Key, 0);
        }
        
        UpdateResources(null, null);
        
        _highlighter.Init(_coreManagers);
    }

    public void OnDayTick(object sender, object args)
    {
        
    }
    
    public void UpdateResources(object sender, object args)
    {
        bool showChange = (bool?) args ?? false;
        
        Entity country = _coreManagers.DataManager.Read<PlayerData>().Country;
        CountryData countryData = country.Get<CountryData>();

        foreach (var amount in countryData.ResourceAmounts)
        {
            int change = amount.Value - _lastAmount[amount.Key];


            string changeText = showChange ? change > 0 ? $"(+{change})" : $"({change})" : string.Empty;

            _resources[amount.Key].Text = $"{amount.Value}{changeText}";
            _lastAmount[amount.Key] = amount.Value;
        }
    }

    public void Dispose()
    {
        Notification.RemoveObserver<FSGame>(OnDayTick, WorldTimeManager.k_dayTick);
    }
}
