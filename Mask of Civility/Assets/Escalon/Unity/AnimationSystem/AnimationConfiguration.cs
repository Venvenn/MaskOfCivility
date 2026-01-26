using System;
using Escalon;
using Escalon.Nova;
using UnityEngine;

[Serializable]
public struct AnimationConfiguration
{
    public AnimationEaseType TargetType;
    public AnimationChainType ChainType;
    
    public Vector3 Start;
    public Vector3 End;
    public Color StartColour;
    public Color EndColour;
    public bool Local;
    
    public float Duration;
    public Easing.Extent Extent;
    public Easing.Shape Shape;
}