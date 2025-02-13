using Data.Enums;
using System.Diagnostics;

namespace Business.Services;

public static class CalculateTotalProjectPriceService
{
    public static decimal CalculatePrice(DateTime startDate, DateTime endDate, Units unit, decimal pricePerUnit)
    {
        int days = CalculatedTotalDays(startDate, endDate);
        Debug.WriteLine($"{days} {unit} {pricePerUnit}");
        if (unit == Units.Day) return pricePerUnit * days;
        if (unit == Units.Hour) return (days * 8) * pricePerUnit;

        return 0;
    }
    private static int CalculatedTotalDays(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate) return 0;

        int totalDays = 0;

        for(DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                continue;
            }
            totalDays++;
        }
        return totalDays;
    }
}
