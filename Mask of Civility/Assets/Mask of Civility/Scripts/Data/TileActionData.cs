using System;
using Escalon;

[Serializable]
public struct TileActionData
{
    public string Name;
    public string Text;
    public TargetType TargetType;
    public ResourceTargetTypes ResourceTargetTypes;
    public int MaskEffect;
    public SerializableTimeSpan Duration;
    public SerializableDictionary<ResourceType, int> Cost;

    public DateTime GetDestinationDate(DateTime currentTime)
    {
        currentTime = currentTime.AddDays(Duration.Days);
        currentTime = currentTime.AddMonths(Duration.Months);
        currentTime = currentTime.AddYears(Duration.Years);
        return currentTime;
    }
}

[Serializable]
public struct TileActionsData : IData
{
    public TileActionData[] Actions;
}

[Serializable]
public struct SerializableTimeSpan
{
    public int Years;
    public int Months;
    public int Days;
}

[Serializable,Flags]
public enum TargetType
{
    None = 0,
    YourTile = 1 << 0,
    EnemyTile= 1 << 1,
    YourCountry = 1 << 2,
    EnemyCountry = 1 << 3, 
    Unowned = 1 << 4
}

[Serializable, Flags]
public enum ResourceTargetTypes
{
    None = 0,
    Population = 1 << 0,
    Military = 1 << 1,
    Espionage = 1 << 2,
    Industry = 1 << 3,
    Economy = 1 << 4
}

