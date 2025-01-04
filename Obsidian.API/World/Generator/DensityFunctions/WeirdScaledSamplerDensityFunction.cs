namespace Obsidian.API.World.Generator.DensityFunctions;

[DensityFunction("minecraft:weird_scaled_sampler")]
public sealed class WeirdScaledSamplerDensityFunction : IDensityFunction
{
    public string Type => "minecraft:weird_scaled_sampler";

    public required IDensityFunction Input { get; init; }

    public required string RarityValueMapper { get; init; }

    public required INoise Noise { get; init; }

    public double MinValue => throw new NotImplementedException();

    public double MaxValue => throw new NotImplementedException();

    public double GetValue(double x, double y, double z) => 1.0D;
}
