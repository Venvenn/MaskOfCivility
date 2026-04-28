
using System;

[Serializable,Flags]
public enum ResourceTypes 
{
    None = 0,
    YourTile = 1 << 0,
    EnemyTile= 1 << 1,
    YourCountry = 1 << 2,
    EnemyCountry = 1 << 3, 
    Unowned = 1 << 4
}
