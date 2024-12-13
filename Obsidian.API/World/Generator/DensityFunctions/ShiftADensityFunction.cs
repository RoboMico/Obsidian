namespace Obsidian.API.World.Generator.DensityFunctions;

[DensityFunction("minecraft:shift_a")]
public sealed class ShiftADensityFunction : IDensityFunction
{
    public string Type => "minecraft:shift_a";

    public required INoise Argument { get; init; }

    public double GetValue(double x, double y, double z) => Argument.GetValue(x / 4.0, 0, z / 4.0) * 4.0;
}
