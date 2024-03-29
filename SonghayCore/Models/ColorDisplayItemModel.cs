﻿namespace Songhay.Models;

/// <summary>
/// Model for color display
/// </summary>
public class ColorDisplayItemModel : DisplayItemModel, IColorable
{
    #region IColorable members:

    /// <summary>
    /// Gets or sets the background hexadecimal value.
    /// </summary>
    public string? BackgroundHex { get; set; }

    /// <summary>
    /// Gets or sets the foreground hexadecimal value.
    /// </summary>
    public string? ForegroundHex { get; set; }

    #endregion
}
