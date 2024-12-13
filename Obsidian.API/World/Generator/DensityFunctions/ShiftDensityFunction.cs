namespace Obsidian.API.World.Generator.DensityFunctions;

[DensityFunction("minecraft:shift")]
public sealed class ShiftDensityFunction : IDensityFunction
{
    public string Type => "minecraft:shift";

    public required INoise Argument { get; init; }

    public double GetValue(double x, double y, double z) => Argument.GetValue(x / 4.0, y / 4.0, z / 4.0) * 4.0;
}
