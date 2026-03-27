using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using ABMod.Tiles.AsteroidBiome.Moss;

namespace ABMod.Generation.Asteroids.Small
{
    public class Star : AsteroidBase
    {
        public override bool Place(Point origin, bool style)
        {
            int size;

            if (!style)
            {
                size = WorldGen.genRand.Next(5, 9);
            }
            else
            {
                size = WorldGen.genRand.Next(4, 8);
            }

            return false;
        }

        public override void GenerateAsteroid(Point origin, int size)
        {
            //Empty
        }
    }
}