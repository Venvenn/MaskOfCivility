using System.Threading.Tasks;
using Escalon;
using UnityEngine;

public class GenerationView : IView
{
    private GenerationScreenUI _ui;
    private CoreManagers _coreManagers;
    
    public Task Init(FlowState flowState, CoreManagers coreManagers)
    {
        _coreManagers = coreManagers;
        _ui = Object.Instantiate(Resources.Load<GenerationScreenUI>("Prefabs/UI/Screens/GenerationScreenUI"));
        _ui.Init(_coreManagers);
        return Task.Delay(1000);
    }

    public void Present()
    {

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
        Object.Destroy(_ui.gameObject);
    }
}
