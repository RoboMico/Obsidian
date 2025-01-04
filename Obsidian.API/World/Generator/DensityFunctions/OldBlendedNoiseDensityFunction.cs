using Obsidian.API.Noise;

namespace Obsidian.API.World.Generator.DensityFunctions;

[DensityFunction("minecraft:old_blended_noise")]
public class OldBlendedNoiseDensityFunction : IDensityFunction
{
    public virtual required double SmearScaleMultiplier { get; init; }

    public virtual required double XzFactor { get; init; }

    public virtual required double XzScale { get; init; }

    public virtual required double YFactor { get; init; }

    public virtual required double YScale { get; init; }

    public string Type => "minecraft:old_blended_noise";

    public double MinValue => -MaxValue;

    public double MaxValue
    {
        get
        {
            if (!_initialized)
            {
                Create();
            }
            return field;
        }
        private set;
    }

    private bool _initialized = false;
    private PerlinNoise _minLimitNoise;
    private PerlinNoise _maxLimitNoise;
    private PerlinNoise _mainNoise;


    public void Create()
    {
        MaxValue = 3.0D;
    }
    public virtual double GetValue(double x, double y, double z) => 1.0;

}
