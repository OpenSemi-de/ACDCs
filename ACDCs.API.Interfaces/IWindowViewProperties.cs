﻿namespace ACDCs.API.Interfaces;

using Sharp.UI;

[BindableProperties]
public interface IWindowViewProperties
{
    public View WindowContent { get; set; }
}
