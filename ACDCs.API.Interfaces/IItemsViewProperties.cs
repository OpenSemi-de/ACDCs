﻿namespace ACDCs.API.Interfaces;

using Sharp.UI;

[BindableProperties]
public interface IItemsViewProperties
{
    double ButtonHeight { get; set; }
    double ButtonWidth { get; set; }
}
