using Escalon;
using Nova;
using UnityEngine;
using Vector4 = System.Numerics.Vector4;

public struct ColourAnimationUIBlock  : IAnimation
{
    private UIBlock _target;
    private Vector4 _start;
    private Vector4 _end;
    private Easing.Extent _extent;
    private Easing.Shape _shape;

    public ColourAnimationUIBlock(UIBlock target, Vector4 start, Vector4 end, Easing.Extent extent, Easing.Shape shape)
    {
        _target = target;
        _start = start;
        _end = end;
        _extent = extent;
        _shape = shape;
    }

    public void Update(float percentDone)
    {
        Vector4 colour = Easing.Ease(_shape, _extent, _start, _end, percentDone);
        if (_target != null)
        {
            _target.Color = new Color(colour.X, colour.Y, colour.Z,colour.W);
        }
    }
}

public struct ColourAnimationGameObject  : IAnimation
{
    private Renderer _target;
    private Vector4 _start;
    private Vector4 _end;
    private Easing.Extent _extent;
    private Easing.Shape _shape;

    public ColourAnimationGameObject(GameObject target, Vector4 start, Vector4 end, Easing.Extent extent, Easing.Shape shape)
    {
        _target = target.GetComponent<Renderer>();
        _start = start;
        _end = end;
        _extent = extent;
        _shape = shape;
    }

    public void Update(float percentDone)
    {
        Vector4 colour = Easing.Ease(_shape, _extent, _start, _end, percentDone);
        if (_target != null)
        {
            if (Application.isPlaying)
            {
                _target.material.color = new Color(colour.X, colour.Y, colour.Z,colour.W);

            }
            else
            {
                _target.sharedMaterial.color = new Color(colour.X, colour.Y, colour.Z,colour.W);
            }
        }
    }
}
