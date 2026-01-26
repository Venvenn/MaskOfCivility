using Escalon;
using Nova;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public struct PositionAnimationUIBlock : IAnimation
{
    private UIBlock2D _entity;
    private Vector3 _start;
    private Vector3 _end;
    private Easing.Extent _extent;
    private Easing.Shape _shape;
    private bool _local;

    
    public PositionAnimationUIBlock(UIBlock2D entity, bool local, Vector3 start, Vector3 end, Easing.Extent extent, Easing.Shape shape)
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
        Vector3 position = Easing.Ease(_shape, _extent, _start, _end, percentDone);
        
        if (_local)
        {
            _entity.TrySetLocalPosition(new UnityEngine.Vector3(position.X, position.Y, position.Z)); 
        }
        else
        {
            _entity.TrySetWorldPosition(new UnityEngine.Vector3(position.X, position.Y, position.Z)); 
        }
        _entity.CalculateLayout();
    }
}

public struct PositionAnimationGameObject : IAnimation
{
    private GameObject _gameObject;
    private Vector3 _start;
    private Vector3 _end;
    private Easing.Extent _extent;
    private Easing.Shape _shape;
    private bool _worldPos;
    
    public PositionAnimationGameObject(GameObject gameObject, bool worldPos, Vector3 start, Vector3 end, Easing.Extent extent, Easing.Shape shape)
    {
        _gameObject = gameObject;
        _start = start;
        _end = end;
        _extent = extent;
        _shape = shape;
        _worldPos = worldPos;
    }

    public void Update(float percentDone)
    {
        Vector3 position = Easing.Ease(_shape, _extent, _start, _end, percentDone);
        if (_gameObject != null)
        {
            if (_worldPos)
            {
                _gameObject.transform.position = new UnityEngine.Vector3(position.X, position.Y, position.Z);
            }
            else
            {
                _gameObject.transform.localPosition = new UnityEngine.Vector3(position.X, position.Y, position.Z);
            }
        }
    }
}


