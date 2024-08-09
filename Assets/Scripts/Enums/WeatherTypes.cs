using System.Collections.Generic;

public enum WeatherTypes
{
    DEFAULT,
    HOT,
    COLD
}

public struct WeatherTypeInfo
{
    public float HeatFactor { get; }
    public float FuelFactor { get; }

    public WeatherTypeInfo(float heatFactor, float fuelFactor)
    {
        HeatFactor = heatFactor;
        FuelFactor = fuelFactor;
    }
}

public static class WeatherTypeExtensions
{
    private static readonly Dictionary<WeatherTypes, WeatherTypeInfo> WeatherTypeInfos = new Dictionary<WeatherTypes, WeatherTypeInfo>
    {
        { WeatherTypes.DEFAULT, new WeatherTypeInfo(1.0f, 1.0f) },
        { WeatherTypes.HOT, new WeatherTypeInfo(1.5f, 0.9f) },
        { WeatherTypes.COLD, new WeatherTypeInfo(0.5f, 1.5f) }
    };

    public static WeatherTypeInfo GetWeatherTypeInfo(this WeatherTypes weatherType)
    {
        return WeatherTypeInfos[weatherType];
    }
}
