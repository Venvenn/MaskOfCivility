using Escalon;
using Nova;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public struct RotationAnimationUIBlock : IAnimation
{
    private UIBlock2D _entity;
    private Vector3 _start;
    private Vector3 _end;
    private Easing.Extent _extent;
    private Easing.Shape _shape;
    private bool _local;

    public RotationAnimationUIBlock(UIBlock2D entity, bool local, Vector3 start, Vector3 end, Easing.Extent extent, Easing.Shape shape)
    {
        _entity = entity;
        _start = start;
        _end = end;
        _extent = extent;
        _shape = shape;
        _local = local;
    }

    public void Update(float percentDone)
    {
        Vector3 rotation = Easing.Ease(_shape, _extent, _start, _end, percentDone);
        if (_entity != null)
        {
            if (_local)
            {
                _entity.transform.localRotation = Quaternion.Euler(new UnityEngine.Vector3(rotation.X, rotation.Y, rotation.Z));

            }
            else
            {
                _entity.transform.rotation = Quaternion.Euler(new UnityEngine.Vector3(rotation.X, rotation.Y, rotation.Z));
            }
        }
    }
}

public struct RotationAnimationGameObject : IAnimation
{
    private GameObject _entity;
    private Vector3 _start;
    private Vector3 _end;
    private Easing.Extent _extent;
    private Easing.Shape _shape;
    private bool _local;
    
    public RotationAnimationGameObject(GameObject entity, bool local, Vector3 start, Vector3 end, Easing.Extent extent, Easing.Shape shape)
    {
        _entity = entity;
        _start = start;
        _end = end;
        _extent = extent;
        _shape = shape;
        _local = local;
    }
    public void Update(float percentDone)
    {
        Vector3 rotation = Easing.Ease(_shape, _extent, _start, _end, percentDone);
        if (_entity != null)
        {
            if (_local)
            {
                _entity.transform.localRotation = Quaternion.Euler(new UnityEngine.Vector3(rotation.X, rotation.Y, rotation.Z));
            }
            else
            {
                _entity.transform.rotation = Quaternion.Euler(new UnityEngine.Vector3(rotation.X, rotation.Y, rotation.Z));
            }
        }
    }
}
