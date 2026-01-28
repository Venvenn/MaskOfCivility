using System;

[Serializable]
public struct TimeConfig : IData
{
    public float SecondsPerDay;
    public SerializableDateTime StartDate;
}
