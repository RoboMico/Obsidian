using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;


namespace Obsidian.API.Noise;

public class PerlinNoise
{
    private const int ROUND_OFF = 33554432;
    private readonly ImprovedNoise[] _noiseLevels;
    private readonly int _firstOctave;
    private readonly List<double> _amplitudes;
    private readonly double _lowestFreqValueFactor;
    private readonly double _lowestFreqInputFactor;
    private readonly double _maxValue;

    public static PerlinNoise Create(Random randomSource, int firstOctave, List<double> amplitudes)
    {
        return new PerlinNoise(randomSource, Tuple.Create(firstOctave, amplitudes));
    }

    private static Tuple<int, List<double>> MakeAmplitudes(HashSet<int> octaves)
    {
        if (!octaves.Any())
        {
            throw new ArgumentException("Need some octaves!");
        }

        int minOctave = -octaves.Min();
        int maxOctave = octaves.Max();
        int totalOctaves = minOctave + maxOctave + 1;

        if (totalOctaves < 1)
        {
            throw new ArgumentException("Total number of octaves needs to be >= 1");
        }

        var amplitudes = Enumerable.Repeat(0.0, totalOctaves).ToList();
        foreach (var octave in octaves)
        {
            amplitudes[octave + minOctave] = 1.0;
        }

        return Tuple.Create(-minOctave, amplitudes);
    }

    private PerlinNoise(Random randomSource, Tuple<int, List<double>> configuration)
    {
        _firstOctave = configuration.Item1;
        _amplitudes = configuration.Item2;

        int totalOctaves = _amplitudes.Count;
        int zeroOctaveIndex = -_firstOctave;
        _noiseLevels = new ImprovedNoise[totalOctaves];

        var baseNoise = new ImprovedNoise(randomSource);

        if (zeroOctaveIndex >= 0 && zeroOctaveIndex < totalOctaves)
        {
            double amplitude = _amplitudes[zeroOctaveIndex];
            if (amplitude != 0.0)
            {
                _noiseLevels[zeroOctaveIndex] = baseNoise;
            }
        }

        for (int i = zeroOctaveIndex - 1; i >= 0; i--)
        {
            if (i < totalOctaves)
            {
                double amplitude = _amplitudes[i];
                _noiseLevels[i] = amplitude != 0.0 ? new ImprovedNoise(randomSource) : null;
            }
        }


        _lowestFreqInputFactor = Math.Pow(2.0, -zeroOctaveIndex);
        _lowestFreqValueFactor = Math.Pow(2.0, totalOctaves - 1) / (Math.Pow(2.0, totalOctaves) - 1);
        _maxValue = EdgeValue(2.0);
    }

    public double GetValue(double x, double y, double z)
    {
        return GetValue(x, y, z, 0.0, 0.0, false);
    }

    [Obsolete("Legacy method for backward compatibility.")]
    public double GetValue(double x, double y, double z, double offsetX, double offsetY, bool useOffset)
    {
        double result = 0.0;
        double inputFactor = _lowestFreqInputFactor;
        double valueFactor = _lowestFreqValueFactor;

        foreach (var noise in _noiseLevels)
        {
            if (noise != null)
            {
                double noiseValue = noise.Noise(Wrap(x * inputFactor), useOffset ? -noise.yo : Wrap(y * inputFactor), Wrap(z * inputFactor), offsetX * inputFactor, offsetY * inputFactor);
                result += _amplitudes[_noiseLevels.ToList().IndexOf(noise)] * noiseValue * valueFactor;
            }

            inputFactor *= 2.0;
            valueFactor /= 2.0;
        }

        return result;
    }

    public double MaxBrokenValue(double scaleFactor)
    {
        return EdgeValue(scaleFactor + 2.0);
    }

    private double EdgeValue(double scaleFactor)
    {
        double result = 0.0;
        double valueFactor = _lowestFreqValueFactor;

        foreach (var noise in _noiseLevels)
        {
            if (noise != null)
            {
                result += _amplitudes[_noiseLevels.ToList().IndexOf(noise)] * scaleFactor * valueFactor;
            }
            valueFactor /= 2.0;
        }

        return result;
    }

    public static double Wrap(double value)
    {
        return value - Math.Floor(value / 3.3554432E7 + 0.5) * 3.3554432E7;
    }

    public int FirstOctave => _firstOctave;

    public IReadOnlyList<double> Amplitudes => _amplitudes;

}
