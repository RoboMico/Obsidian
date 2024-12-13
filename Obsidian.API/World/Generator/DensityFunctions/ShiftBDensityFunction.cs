namespace Obsidian.API.World.Generator.DensityFunctions;

[DensityFunction("minecraft:shift_b")]
public sealed class ShiftBDensityFunction : IDensityFunction
{
    public string Type => "minecraft:shift_b";

    public required INoise Argument { get; init; }

    public double GetValue(double x, double y, double z) => Argument.GetValue(z / 4.0, x / 4.0, 0) * 4.0;
}
