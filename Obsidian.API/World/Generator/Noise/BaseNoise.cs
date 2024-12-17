using SharpNoise;

namespace Obsidian.API.World.Generator.Noise;
public partial class BaseNoise : INoise
{
    public string Type => "minecraft:base_noise";

    public required List<double> Amplitudes { get; init; }

    public required double FirstOctave { get; init; }

    public int Seed { get; init; }

    public double GetValue(double x, double y, double z)
    {
        double result = 0.0;
        int octave = (int)FirstOctave;
        for (int i = 0; i < Amplitudes.Count; i++)
        {
            int s = (Seed + i) & 0x7FFFFFFF;
            double noise1 = NoiseGenerator.GradientCoherentNoise3D(x, y, z, s, NoiseQuality.Standard);
            double noise2 = NoiseGenerator.GradientCoherentNoise3D(x, y, z, s + octave, NoiseQuality.Standard);
            double noise = noise1 + noise2 / 2.0D;
            double persistence = Amplitudes[i] * Math.Pow(2, octave - i - 1) / (Math.Pow(2, octave) - 1);
            result += noise * persistence;
            double lacunarity = Math.Pow(2, octave + i);
            x *= lacunarity;
            y *= lacunarity;
            z *= lacunarity;
            octave = (int)FirstOctave + i;
        }
        double returnVal = 10 * result / (3 * (1 + (1 / (Amplitudes.Count - 2))));

        return returnVal;
    }
}
