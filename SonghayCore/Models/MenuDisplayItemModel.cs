﻿namespace Songhay.Models;

/// <summary>
/// Defines a colorable, selectable menu item
/// </summary>
public class MenuDisplayItemModel : ColorDisplayItemModel, IGroupable, ISelectable
{
    /// <summary>
    /// Gets or sets the child items.
    /// </summary>
    public MenuDisplayItemModel[] ChildItems { get; set; } = Enumerable.Empty<MenuDisplayItemModel>().ToArray();

    #region IGroupable members:

    /// <summary>
    /// Display text of the Group.
    /// </summary>
    public string? GroupDisplayText { get; set; }

    /// <summary>
    /// Identifier of the Group.
    /// </summary>
    public string? GroupId { get; set; }

    /// <summary>
    /// Returns `true` when group is visually collapsed.
    /// </summary>
    public bool IsCollapsed { get; set; }

    #endregion

    #region ISelectable members:

    /// <summary>
    /// Gets or sets whether this is default selection.
    /// </summary>
    public bool? IsDefaultSelection { get; set; }

    /// <summary>
    /// Gets or sets whether this is enabled.
    /// </summary>
    public bool? IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether this is selected.
    /// </summary>
    public bool? IsSelected { get; set; }

    #endregion
}
