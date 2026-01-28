using System;
using UnityEngine;

[Serializable]
public class SerializableDateTime
{
    //[SerializeField] private int _ticks;
    [SerializeField] private int _year;
    [SerializeField] private int _month;
    [SerializeField] private int _day;
    
    private bool _initialized;
    private DateTime _dateTime;

    public DateTime DateTime
    {
        get
        {
            if (!_initialized)
            {
                _dateTime = new DateTime(_year, _month, _day);
                _initialized = true;
            }

            return _dateTime;
        }
    }

    public SerializableDateTime(DateTime dateTime)
    {
        _dateTime = dateTime;
        _initialized = true;
    }
}

