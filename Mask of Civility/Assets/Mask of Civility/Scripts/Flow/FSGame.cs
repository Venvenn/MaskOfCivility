using System;
using Arch.Core;
using Arch.System;
using Escalon;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using FlowState = Escalon.FlowState;

public class FSGame : FlowState
{
    private CoreManagers _coreManagers;
    private Entity _map;
    private Group<float> _systems;

    public FSGame(Entity map)
    {
        _map = map;
    }
    
    public override void OnStartInitialise()
    {
        _coreManagers = Container.GetAspect<CoreManagers>();
        _systems = new Group<float>("GamePlay", 
            new TileVisualsSystem(_coreManagers.EntityManager.World, _coreManagers), 
            new ResourceSystem(_coreManagers.EntityManager.World, _coreManagers));
        _systems.Initialize();
        
        Container.AddAspect(new WorldTimeManager(_coreManagers.DataManager.Read<TimeConfig>(), _coreManagers));
    }

    public override void OnFinishInitialise()
    {
    }

    private void TileSelection()
    {
        SelectionData selectionData = _coreManagers.DataManager.Read<SelectionData>();
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.value), out hitInfo);
        if (hit) 
        {
            if (hitInfo.transform.TryGetComponent<TileView>(out var tileView))
            {
                selectionData.HoveredTile = tileView.Entity;
            }
            else
            {
                selectionData.HoveredTile = Entity.Null;
            }
            if(Mouse.current.leftButton.isPressed)
            {
                this.PostNotification(Highlighter.k_openHighlighter, tileView.Entity);
                selectionData.SelectedTile = tileView.Entity;
                // AttackAction attackAction = new AttackAction(_coreManagers.DataManager.Read<TileActionsData>().Actions[0], tileView.Entity,_coreManagers.DataManager.Read<PlayerData>().Country, _coreManagers);
                // attackAction.Run();
            } 
        }
        else
        {
            if(Mouse.current.leftButton.isPressed)
            {
                selectionData.SelectedTile = Entity.Null;
                this.PostNotification(Highlighter.k_closeHighlighter);
            } 
        }

        _coreManagers.DataManager.Write(selectionData);
    }
    
    public override void ActiveUpdate()
    {
        _systems.Update(Time.deltaTime);
        TileSelection();
    }

    public override void OnFinishDismiss()
    {
        _systems.Dispose();
    }
}