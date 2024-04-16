﻿using System.ComponentModel.DataAnnotations;

namespace Red.OpenWeather;

[Flags]
public enum Excludes
{
    [Display(Name = "current")]
    Current,
    [Display(Name = "minutely")]
    Minutely,
    [Display(Name = "hourly")]
    Hourly,
    [Display(Name = "daily")]
    Daily,
    [Display(Name = "alerts")]
    Alerts
}
