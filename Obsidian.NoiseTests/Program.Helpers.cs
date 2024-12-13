using Obsidian.API.Noise;
using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;
using SharpNoise.Utilities.Imaging;

public partial class Program
{
    private class TestNoiseModule(int sourceModuleCount) : Module(sourceModuleCount)
    {
        public override double GetValue(double x, double y, double z) =>0;
    }

    private static void Test()
    {
        NoiseCube nc = new();
        NoiseMap nm = new();

        LinearNoiseCubeBuilder lncb = new()
        {
            DestNoiseCube = nc,
            SourceModule = new TestNoiseModule(1)
        };
        lncb.SetBounds(0, 1600, -64, 320, 0, 1200);
        lncb.SetDestSize(1600, 384, 1200);
        lncb.Build();

        HeightNoiseMapBuilder dnmb = new()
        {
            DestNoiseMap = nm,
            SourceNoiseCube = nc
        };
        dnmb.SetDestSize(1600, 1200);
        dnmb.Build();

        Image img = new();
        ImageRenderer transitionsRenderer = new()
        {
            SourceNoiseMap = nm,
            DestinationImage = img
        };

        transitionsRenderer.BuildTerrainGradient();
        transitionsRenderer.Render();

        var bmp = transitionsRenderer.DestinationImage.ToGdiBitmap();
        bmp.Save("Continents.bmp");
    }
}
