using System;
using System.Collections.Generic;
using Escalon;

public class WorldTimeManager : BaseManager, IUpdateable
{
    public const string k_dayTick = "WorldTimeManager.DayTick";
    public const string k_weekTick = "WorldTimeManager.WeekTick";
    public const string k_monthTick = "WorldTimeManager.MonthTick";
    public const string k_yearTick = "WorldTimeManager.YearTick";
    public const string k_addTimedCallback = "WorldTimeManager.AddTimedCallback";
    
    private CoreManagers _coreManagers;
    private DateTime _startTime;
    private DateTime _currentTime;
    private TimeConfig _timeSettings;
    private float _dayProgress;
    private List<TimedCallback> _timedCallbacks = new List<TimedCallback>();
    
    
    public DateTime CurrentTime => _currentTime;
    
    public WorldTimeManager(TimeConfig config, CoreManagers coreManagers)
    {
        _startTime = config.StartDate.DateTime;
        _currentTime =  config.StartDate.DateTime;
        _timeSettings = config;
        _coreManagers = coreManagers;
        
        Notification.AddObserver<FSApplication>(AddTimedCallBack, k_addTimedCallback);
    }

    public void Update(float dt)
    {
        _dayProgress += dt;
        if (_dayProgress >= _timeSettings.SecondsPerDay)
        {
            _dayProgress = 0;
            var time = _currentTime.AddDays(1);
            this.PostNotification(k_dayTick);

            if (_currentTime.DayOfWeek == DayOfWeek.Sunday)
            {
                this.PostNotification(k_weekTick);
            }
            if (_currentTime.Month != time.Month)
            {
                this.PostNotification(k_monthTick);
            }
            if (_currentTime.Year != time.Year)
            {
                this.PostNotification(k_yearTick);
            }

            foreach (var timedCallback in _timedCallbacks)
            {
                if (time >= timedCallback.Time)
                {
                    timedCallback.Action.Invoke();
                }
            }
            
            _currentTime = time;
        }
    }

    private void AddTimedCallBack(object sender, object args)
    {
        TimedCallback callback = (TimedCallback)args;
        _timedCallbacks.Add(callback);
    }
}
