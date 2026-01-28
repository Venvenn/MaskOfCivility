using System.Threading.Tasks;
using Escalon;
using UnityEngine;

public class GameView : IView
{
    private GameScreenUI _gameUI;
    private CoreManagers _coreManagers;
    
    public Task Init(FlowState flowState, CoreManagers coreManagers)
    {
        _coreManagers = coreManagers;
        return Task.CompletedTask;
    }

    public void Present()
    {
        _gameUI = Object.Instantiate(Resources.Load<GameScreenUI>("Prefabs/UI/Screens/GameScreenUI"));
        _gameUI.Init(_coreManagers);
    }

    public void UpdateView()
    {
      
    }

    public void OnActive()
    {

    }

    public void OnInactive()
    {
  
    }

    public void Dismiss()
    {
    
    }

    public bool IsTransitioning()
    {
        return false;
    }


    public void DisposeView()
    {
        _gameUI.Dispose();
        Object.Destroy(_gameUI.gameObject);
    }
}
