using System;
using Songhay.Abstractions;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="ITemporal"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ITemporalExtensions
{
    /// <summary>
    /// Sets conventional default values
    /// for <see cref="ITemporal"/> data.
    /// </summary>
    /// <param name="data">the <see cref="ITemporal"/> data</param>
    public static void SetDefaults(this ITemporal data)
    {
        data.SetDefaults(endDate: null);
    }

    /// <summary>
    /// Sets conventional default values
    /// for <see cref="ITemporal"/> data.
    /// </summary>
    /// <param name="data">the <see cref="ITemporal"/> data</param>
    /// <param name="endDate">sets <see cref="ITemporal.EndDate"/></param>
    public static void SetDefaults(this ITemporal? data, DateTime? endDate)
    {
        if (data == null) return;

        data.InceptDate = DateTime.Now;
        data.ModificationDate = data.InceptDate;
        data.EndDate = endDate;
    }
}
