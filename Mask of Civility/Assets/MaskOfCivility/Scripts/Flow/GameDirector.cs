using Escalon;
using Escalon.Unity;

public class GameDirector : DirectorUnity
{
    private Container _container = new Container();
    
    public override void OnStart()
    {
        ApplicationManagerUnity applicationManager = _container.AddAspect<ApplicationManagerUnity>();
        applicationManager.Init();
        FlowStateMachine = new FlowStateMachine(container: _container);
        FlowStateMachine.Push(new FSApplication());
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
    }
}
