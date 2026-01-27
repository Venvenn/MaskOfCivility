using Escalon;

public class FSTitleScreen : FlowState
{
    public override void OnStartInitialise()
    {
        base.OnStartInitialise();
    }

    public override TransitionState UpdateInitialise()
    {
        return TransitionState.Completed;
    }
    
    public override void OnFinishInitialise()
    {
    }
    
    public override void OnActive()
    {
    }
    
    public override void OnInactive()
    {
    }
    
    public override void ActiveUpdate()
    {
    }
    
    public override void ActiveFixedUpdate()
    {
    }

    public override void OnStartDismiss()
    {
    }
    
    public override TransitionState UpdateDismiss()
    {
        return TransitionState.Completed;
    }
    
    public override void OnFinishDismiss()
    {
    }
}
