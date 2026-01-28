using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Escalon;
using Nova;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public const string k_openHighlighter = "Highlighter.OpenHighlighter";
    public const string k_closeHighlighter = "Highlighter.CloseHighlighter";
    
    [SerializeField] private ListView _listView;
    [SerializeField] private SerializableDictionary<ResourceType, Sprite> _icons;
    
    private Entity _tileEntity = Entity.Null;
    private CoreManagers _coreManagers;
    
    public void Init(CoreManagers coreManagers)
    {
        _coreManagers = coreManagers;
        _listView.AddDataBinder<TileActionData, ActionItemView>(BindContact);
        Notification.AddObserver<FSGame>(Open, k_openHighlighter);
        Notification.AddObserver<FSGame>(Close, k_closeHighlighter);
    }

    public void Open(object sender, object args)
    {
        gameObject.SetActive(true);
        _tileEntity = (Entity) args;
        TileActionsData tileActionsData = _coreManagers.DataManager.Read<TileActionsData>();
        CountryTileData tileChangeData = _tileEntity.Get<CountryTileData>();
        ResourceData resourceData = _tileEntity.Get<ResourceData>();
        bool playerOwn = _coreManagers.DataManager.Read<PlayerData>().Country == tileChangeData.HardHolder;
        bool playerControl = _coreManagers.DataManager.Read<PlayerData>().Country == tileChangeData.HardHolder;
        
        List<TileActionData> actions = new List<TileActionData>();
        foreach (var action in tileActionsData.Actions)
        {
            bool targetType = false;
            if (playerOwn)
            {
                if ((action.TargetType & TargetType.YourTile) != 0)
                {
                    targetType = true;
                }
            }
            else
            {
                if (!playerControl && (action.TargetType & TargetType.EnemyTile) != 0)
                {
                    targetType = true;
                }
            }
            
            bool resourceType = (action.ResourceTargetTypes & (ResourceTargetTypes)resourceData.ResourceType) != 0;

            if (targetType && resourceType)
            {
                actions.Add(action);
            }
        }

        _listView.SetDataSource(actions);
        _listView.Refresh();
    }

    public void Close(object sender, object args)
    {
        gameObject.SetActive(false);
    }

    private void BindContact(Data.OnBind<TileActionData> evt, ActionItemView visuals, int index)
    {
        TileActionData contact = evt.UserData;
        visuals.Init(contact, _icons);
    }
}
