using Escalon;
using Escalon.Nova;
using Escalon.Unity;
using Nova;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

public static class AnimationSystem
{
    public static void AnimatePosition(UIBlock2D entity, bool worldPos, Vector3 start, Vector3 end, float duration,
        float animationSpeed, ref AnimationHandle handle, Easing.Shape shape = Easing.Shape.SINE,
        Easing.Extent extent = Easing.Extent.IN, AnimationChainType chainType = AnimationChainType.Sequential)
    {
        PositionAnimationUIBlock animationUIBlock =
            new PositionAnimationUIBlock(entity, !worldPos, start, end, extent, shape);
        AddAnimation(animationUIBlock, chainType, duration / animationSpeed, ref handle);
    }

    public static void AnimateScale(UIBlock2D entity, Vector3 start, Vector3 end, float duration, float animationSpeed,
        ref AnimationHandle handle, Easing.Shape shape = Easing.Shape.SINE, Easing.Extent extent = Easing.Extent.IN,
        AnimationChainType chainType = AnimationChainType.Sequential)
    {
        ScaleAnimationUIBlock animationUIBlock = new ScaleAnimationUIBlock(entity, start, end, extent, shape);
        AddAnimation(animationUIBlock, chainType, duration / animationSpeed, ref handle);
    }

    public static void AnimateRotation(UIBlock2D entity, bool worldPos, Vector3 start, Vector3 end, float duration,
        float animationSpeed, ref AnimationHandle handle, Easing.Shape shape = Easing.Shape.SINE,
        Easing.Extent extent = Easing.Extent.IN, AnimationChainType chainType = AnimationChainType.Sequential)
    {
        RotationAnimationUIBlock animationUIBlock =
            new RotationAnimationUIBlock(entity, !worldPos, start, end, extent, shape);
        AddAnimation(animationUIBlock, chainType, duration / animationSpeed, ref handle);
    }

    public static void AnimateColour(UIBlock target, Color start, Color end, float duration, float animationSpeed,
        ref AnimationHandle handle, Easing.Shape shape = Easing.Shape.SINE, Easing.Extent extent = Easing.Extent.IN,
        AnimationChainType chainType = AnimationChainType.Sequential)
    {
        ColourAnimationUIBlock animationUIBlock =
            new ColourAnimationUIBlock(target, start.ToVector4(), end.ToVector4(), extent, shape);
        AddAnimation(animationUIBlock, chainType, duration / animationSpeed, ref handle);
    }


    public static void AnimatePosition(GameObject entity, bool worldPos, UnityEngine.Vector3 start,
        UnityEngine.Vector3 end, float duration, float animationSpeed, ref AnimationHandle handle,
        Easing.Shape shape = Easing.Shape.SINE, Easing.Extent extent = Easing.Extent.IN,
        AnimationChainType chainType = AnimationChainType.Sequential)
    {
        PositionAnimationGameObject animationUIBlock =
            new PositionAnimationGameObject(entity, worldPos, start.ToNumeric(), end.ToNumeric(), extent, shape);
        AddAnimation(animationUIBlock, chainType, duration / animationSpeed, ref handle);
    }

    public static void AnimateScale(GameObject entity, UnityEngine.Vector3 start, UnityEngine.Vector3 end,
        float duration, float animationSpeed, ref AnimationHandle handle, Easing.Shape shape = Easing.Shape.SINE,
        Easing.Extent extent = Easing.Extent.IN, AnimationChainType chainType = AnimationChainType.Sequential)
    {
        ScaleAnimationGameObject animationUIBlock =
            new ScaleAnimationGameObject(entity, start.ToNumeric(), end.ToNumeric(), extent, shape);
        AddAnimation(animationUIBlock, chainType, duration / animationSpeed, ref handle);
    }

    public static void AnimateRotation(GameObject entity, bool worldPos, UnityEngine.Vector3 start,
        UnityEngine.Vector3 end, float duration, float animationSpeed, ref AnimationHandle handle,
        Easing.Shape shape = Easing.Shape.SINE, Easing.Extent extent = Easing.Extent.IN,
        AnimationChainType chainType = AnimationChainType.Sequential)
    {
        RotationAnimationGameObject animationUIBlock =
            new RotationAnimationGameObject(entity, !worldPos, start.ToNumeric(), end.ToNumeric(), extent, shape);
        AddAnimation(animationUIBlock, chainType, duration / animationSpeed, ref handle);
    }

    public static void AddAnimation(UIBlock2D target, AnimationConfiguration animationConfig, bool reverse, ref AnimationHandle handle, float animationSpeed = 1)
    {
        Vector3 start = reverse ? animationConfig.End.ToNumeric() : animationConfig.Start.ToNumeric();
        Vector3 end = reverse ? animationConfig.Start.ToNumeric() : animationConfig.End.ToNumeric();
        Vector4 startColour = reverse ? animationConfig.EndColour.ToVector4() : animationConfig.StartColour.ToVector4();
        Vector4 endColour = reverse ? animationConfig.StartColour.ToVector4() : animationConfig.EndColour.ToVector4();

        switch (animationConfig.TargetType)
        {
            case AnimationEaseType.None:
                DelayAnimation animationDelay = new DelayAnimation();
                AddAnimation(animationDelay, animationConfig.ChainType, animationConfig.Duration / animationSpeed, ref handle);
                break;
            case AnimationEaseType.Position:
                PositionAnimationUIBlock animationPosition = new PositionAnimationUIBlock(target, animationConfig.Local, start, end, animationConfig.Extent, animationConfig.Shape);
                AddAnimation(animationPosition, animationConfig.ChainType, animationConfig.Duration / animationSpeed, ref handle);
                break;
            case AnimationEaseType.Scale:
                ScaleAnimationUIBlock animationScale = new ScaleAnimationUIBlock(target, start, end, animationConfig.Extent, animationConfig.Shape);
                AddAnimation(animationScale, animationConfig.ChainType, animationConfig.Duration / animationSpeed, ref handle);
                break;
            case AnimationEaseType.Rotation:
                RotationAnimationUIBlock animationRotation = new RotationAnimationUIBlock(target, animationConfig.Local, start, end, animationConfig.Extent, animationConfig.Shape);
                AddAnimation(animationRotation, animationConfig.ChainType, animationConfig.Duration / animationSpeed, ref handle);
                break;
            case AnimationEaseType.Colour:
                ColourAnimationUIBlock animationColour = new ColourAnimationUIBlock(target, startColour, endColour, animationConfig.Extent, animationConfig.Shape);
                AddAnimation(animationColour, animationConfig.ChainType, animationConfig.Duration, ref handle);
                break;
        }
    }

    public static void AddAnimation(GameObject target, AnimationConfiguration animationConfig, bool reverse, ref AnimationHandle handle, float animationSpeed = 1)
    {
        Vector3 start = reverse ? animationConfig.End.ToNumeric() : animationConfig.Start.ToNumeric();
        Vector3 end = reverse ? animationConfig.Start.ToNumeric() : animationConfig.End.ToNumeric();
        Vector4 startColour = reverse ? animationConfig.EndColour.ToVector4() : animationConfig.StartColour.ToVector4();
        Vector4 endColour = reverse ? animationConfig.StartColour.ToVector4() : animationConfig.EndColour.ToVector4();

        switch (animationConfig.TargetType)
        {
            case AnimationEaseType.None:
                DelayAnimation animationDelay = new DelayAnimation();
                AddAnimation(animationDelay, animationConfig.ChainType, animationConfig.Duration / animationSpeed, ref handle);
                break;
            case AnimationEaseType.Position:
                PositionAnimationGameObject animationPosition = new PositionAnimationGameObject(target, animationConfig.Local, start, end, animationConfig.Extent, animationConfig.Shape);
                AddAnimation(animationPosition, animationConfig.ChainType, animationConfig.Duration / animationSpeed, ref handle);
                break;
            case AnimationEaseType.Scale:
                ScaleAnimationGameObject animationScale = new ScaleAnimationGameObject(target, start, end, animationConfig.Extent, animationConfig.Shape);
                AddAnimation(animationScale, animationConfig.ChainType, animationConfig.Duration / animationSpeed, ref handle);
                break;
            case AnimationEaseType.Rotation:
                RotationAnimationGameObject animationRotation = new RotationAnimationGameObject(target, animationConfig.Local, start, end, animationConfig.Extent, animationConfig.Shape);
                AddAnimation(animationRotation, animationConfig.ChainType, animationConfig.Duration / animationSpeed, ref handle);
                break;
            case AnimationEaseType.Colour:
                ColourAnimationGameObject animationColour = new ColourAnimationGameObject(target, startColour, endColour, animationConfig.Extent, animationConfig.Shape);
                AddAnimation(animationColour, animationConfig.ChainType, animationConfig.Duration / animationSpeed, ref handle);
                break;
        }
    }
    
    public static void AddAnimation<T>(T animation, AnimationChainType chainType, float duration, ref AnimationHandle handle) where T : struct, IAnimation
    {
        if (handle == default)
        {
            handle = animation.Run(duration);
        }
        else
        {
            switch (chainType)
            {
                case AnimationChainType.Sequential:
                    handle = handle.Chain(animation, duration);
                    break;
                case AnimationChainType.Simultaneous :
                    handle = handle.Include(animation, duration);
                    break;
            }
        }
    }
}
