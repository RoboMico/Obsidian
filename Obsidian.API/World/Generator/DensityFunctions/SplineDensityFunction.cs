namespace Obsidian.API.World.Generator.DensityFunctions;
[DensityFunction("minecraft:spline")]
public sealed class SplineDensityFunction : IDensityFunction
{
    public string Type => "minecraft:spline";

    public required Spline Spline { get; init; }

    public double GetValue(double x, double y, double z) => throw new NotImplementedException();
}

public readonly struct Spline : ISpline
{
    public required IDensityFunction Coordinate { get; init; }

    public required SplinePoint[] Points { get; init; }
}

public interface ISpline
{
}

public readonly struct SplineConstant : ISpline
{
    public double Value { get; init; }
}

public readonly struct SplinePoint
{
    public required double Derivative { get; init; }
    public required double Location { get; init; }

    public required ISpline Value { get; init; }
}
