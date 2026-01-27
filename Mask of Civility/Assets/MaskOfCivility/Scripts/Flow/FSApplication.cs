using Escalon;

public class FSApplication : FlowState
{
    private FlowStateMachine _applicationStateMachine;
    private EntityManager _entityManager;
    private DataManager _dataManager;
    private CoreManagers _coreManagers;
    
    public override void OnStartInitialise()
    {
        _applicationStateMachine = new FlowStateMachine(this, Container);
        _entityManager = Container.AddAspect<EntityManager>();
        _dataManager = Container.AddAspect<DataManager>();
        _coreManagers = new CoreManagers(_entityManager, _dataManager);
        Container.AddAspect(_coreManagers);
    }
    
    public override TransitionState UpdateInitialise()
    {
        return TransitionState.Completed;
    }
    
    public override void OnFinishInitialise()
    {
        _applicationStateMachine.Push(new FSGame());
    }

    public override void ActiveUpdate()
    {
        _applicationStateMachine.Update();
    }
}
