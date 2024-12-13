using Obsidian.API.World.Generator.DensityFunctions;

namespace Obsidian.API.World;
public readonly struct Spline : ISpline
{
    public required IDensityFunction Coordinate { get; init; }

    public required SplinePoint[] Points { get; init; }
}
