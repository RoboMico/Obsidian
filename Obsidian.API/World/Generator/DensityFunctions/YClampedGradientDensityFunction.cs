namespace Obsidian.API.World.Generator.DensityFunctions;

[DensityFunction("minecraft:y_clamped_gradient")]
public sealed class YClampedGradientDensityFunction : IDensityFunction
{
    public required double FromValue { get; init; }
    public required double FromY { get; init; }
    public required double ToValue { get; init; }
    public required double ToY { get; init; }

    public string Type => "minecraft:y_clamped_gradient";

    public double MinValue => Math.Min(FromValue, ToValue);

    public double MaxValue => Math.Max(FromValue, ToValue);

    public double GetValue(double x, double y, double z) => ClampedMap(y, FromY, ToY, FromValue, ToValue);


    private static double ClampedMap(double blockY, double fromY, double toY, double fromValue, double toValue) => ClampedLerp(fromValue, toValue, InverseLerp(blockY, fromY, toY));

    private static double InverseLerp(double x, double y, double z) => (x - y) / (z - y);

    private static double ClampedLerp(double from, double to, double val) => val < 0.0 ? from : val > 1.0 ? to : Lerp(val, from, to);

    private static double Lerp(double from, double to, double val) => to + from * (val - to);
}
