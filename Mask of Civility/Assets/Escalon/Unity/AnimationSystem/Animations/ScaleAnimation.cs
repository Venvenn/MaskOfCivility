using Escalon;
using Nova;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public struct ScaleAnimationUIBlock : IAnimation
{
    private UIBlock2D _entity;
    private Vector3 _start;
    private Vector3 _end;
    private Easing.Extent _extent;
    private Easing.Shape _shape;
    
    public ScaleAnimationUIBlock(UIBlock2D entity, Vector3 start, Vector3 end, Easing.Extent extent, Easing.Shape shape)
    {
        _entity = entity;
        _start = start;
        _end = end;
        _extent = extent;
        _shape = shape;
    }

    public void Update(float percentDone)
    {
        Vector3 scale = Easing.Ease(_shape, _extent, _start, _end, percentDone);
        if (_entity != null)
        {
            _entity.transform.localScale = new UnityEngine.Vector3(scale.X, scale.Y, scale.Z);
        }
    }
}

public struct ScaleAnimationGameObject : IAnimation
{
    private GameObject _entity;
    private Vector3 _start;
    private Vector3 _end;
    private Easing.Extent _extent;
    private Easing.Shape _shape;

    public ScaleAnimationGameObject(GameObject entity, Vector3 start, Vector3 end, Easing.Extent extent, Easing.Shape shape)
    {
        _entity = entity;
        _start = start;
        _end = end;
        _extent = extent;
        _shape = shape;
    }

    public void Update(float percentDone)
    {
        Vector3 scale = Easing.Ease(_shape, _extent, _start, _end, percentDone);
        if (_entity != null)
        {
            _entity.transform.localScale = new UnityEngine.Vector3(scale.X, scale.Y, scale.Z);
        }
    }
}
