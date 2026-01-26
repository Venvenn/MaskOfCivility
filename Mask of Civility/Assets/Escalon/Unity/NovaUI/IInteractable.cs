using System;
using Nova;

public interface IInteractable<in T> where T : ItemVisuals
{
    public UIBlock2D UIBlock { get;}
    
    public bool Enabled { get; set;}
    
    public void OnEnable()
    {
        if (!Enabled)
        {
            // Subscribe to gestures
            UIBlock.AddGestureHandler<Gesture.OnPress, T>(OnPressed);
            UIBlock.AddGestureHandler<Gesture.OnDrag, T>(OnDragged);
            UIBlock.AddGestureHandler<Gesture.OnHover, T>(OnHovered);
            UIBlock.AddGestureHandler<Gesture.OnUnhover, T>(OnUnhovered);
            UIBlock.AddGestureHandler<Gesture.OnRelease, T>(OnReleased);
            UIBlock.AddGestureHandler<Gesture.OnCancel, T>(OnCanceled);
            Enabled = true;
        }
    }

    public void OnDisable()
    {
        if (Enabled)
        {
            // Unsubscribe from gestures
            UIBlock.RemoveGestureHandler<Gesture.OnPress, T>(OnPressed);
            UIBlock.RemoveGestureHandler<Gesture.OnDrag, T>(OnDragged);
            UIBlock.RemoveGestureHandler<Gesture.OnHover, T>(OnHovered);
            UIBlock.RemoveGestureHandler<Gesture.OnUnhover, T>(OnUnhovered);
            UIBlock.RemoveGestureHandler<Gesture.OnRelease, T>(OnReleased);
            UIBlock.RemoveGestureHandler<Gesture.OnCancel, T>(OnCanceled);
            Enabled = false; 
        }
    }

    public bool CheckValid(Type type)
    {
        return true;
    }

    public void OnPressed(Gesture.OnPress evt , T visual)
    {
        if (CheckValid(evt.GetType()))
        {
            Pressed(evt , visual);
        }
    }
    
    public void OnDragged(Gesture.OnDrag evt, T visual)
    {
        if (CheckValid(evt.GetType()))
        {
            Dragged(evt, visual);
        }
    }
    void OnHovered(Gesture.OnHover evt, T visual)
    {
        if (CheckValid(evt.GetType()))
        {
            Hovered(evt, visual);
        }
    }
    void OnUnhovered(Gesture.OnUnhover evt, T visual)
    {
        if (CheckValid(evt.GetType()))
        {
            Unhovered(evt, visual);
        } 
    }
    void OnReleased(Gesture.OnRelease evt, T visual)
    {
        if (CheckValid(evt.GetType()))
        {
            Released(evt, visual);
        }
    }
    void OnCanceled(Gesture.OnCancel evt, T visual)
    {
        if (CheckValid(evt.GetType()))
        {
            Canceled(evt, visual);
        }
    }

    void Pressed(Gesture.OnPress evt , T visual);
    void Dragged(Gesture.OnDrag evt, T visual);
    void Hovered(Gesture.OnHover evt, T visual);
    void Unhovered(Gesture.OnUnhover evt, T visual);
    void Released(Gesture.OnRelease evt, T visual);
    void Canceled(Gesture.OnCancel evt, T visual);
}
