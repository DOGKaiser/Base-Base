using System.Collections.Generic;
using UnityEngine;
using System;

public class SeasonalTimeManager : MonoBehaviour {

    public static SeasonalTimeManager Instance;

    private Dictionary<kSeasonTimerType, Action>seasonalTimers = new Dictionary<kSeasonTimerType, Action>();

    private DateTime previousTime = DateTime.UtcNow;

    // Start is called before the first frame update
    void Start() {
        Instance = this;
        previousTime = DateTime.UtcNow;
    }

    // Update is called once per frame
    void Update() {
        UpdateSeasonalTimers();
        previousTime = DateTime.UtcNow;
    }

    public void SubscribeToSeasonalTimer(kSeasonTimerType timerType, Action action) {
        if (seasonalTimers.ContainsKey(timerType)) {
            seasonalTimers[timerType] += action;
        } else {
            seasonalTimers.Add(timerType, action);
        }
    }

    public void UnsubscribeFromSeasonalTimer(kSeasonTimerType timerType, Action action) {
        if (seasonalTimers.ContainsKey(timerType)) {
            seasonalTimers[timerType] -= action;
        }
    }

    private void UpdateSeasonalTimers() {
        foreach (var timer in seasonalTimers) {
            if (DidSeasonResetFromLastDateTime(previousTime, timer.Key)) {
                timer.Value?.Invoke();
            }
        }
    }

    public bool DidSeasonResetFromLastDateTime(DateTime dateTime, kSeasonTimerType seasonTimerType) {
        DateTime curTime = DateTime.UtcNow;

        switch (seasonTimerType) {
            case kSeasonTimerType.Yearly:
                return curTime.Year != dateTime.Year;
            case kSeasonTimerType.Monthly:
                return curTime.Month != dateTime.Month || curTime.Year != dateTime.Year;
            case kSeasonTimerType.Weekly:
                return curTime.DayOfWeek < dateTime.DayOfWeek || curTime.Day - dateTime.Day >= 7;
            case kSeasonTimerType.Daily:
                return curTime.Day != dateTime.Day;
            case kSeasonTimerType.Hourly:
                return curTime.Hour != dateTime.Hour;
            case kSeasonTimerType.Minutely:
                return curTime.Minute != dateTime.Minute;
            default:
                return false;
        }
    }
}