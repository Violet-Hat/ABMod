using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.IO;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.Localization;

using ABMod.Content.Generation.Helpers;

namespace ABMod.Content.Generation
{
    public class GreenMushroomGen
    {
        //Generation values
        static readonly int PlaceMushX = Main.maxTilesX / 2;
		static readonly int PlaceMushY = (int)(Main.maxTilesY * 0.6f);

		static readonly int BiomeWidth = Main.maxTilesX >= 8400 ? 280 : (Main.maxTilesX >= 6400 ? 200 : 140);
        static readonly int BiomeHeight = (int)(BiomeWidth * 0.6f);

        public static void GreenMushGen(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = Language.GetOrRegister("Mods.ABMod.WorldgenTasks.GreenMush").Value;

            Point origin = new(PlaceMushX, PlaceMushY);
            ShapeHelper.PlaceOval(origin, TileID.Mudstone, WallID.MudstoneBrick, BiomeWidth, BiomeHeight);

            //Cave creation
			int seed = WorldGen.genRand.Next();
            float scaleX = 75;
            float scaleY = 25;
            float threshold = 0.05f;

            for (int x = PlaceMushX - BiomeWidth; x <= PlaceMushX + BiomeWidth; x++)
            {
                for (int y = PlaceMushY - BiomeHeight; y <= PlaceMushY + BiomeHeight; y++)
                {
                    bool isMud = Framing.GetTileSafely(x, y).TileType == TileID.Mudstone;
                    bool isInOval = WorldGenTools.IsInEllipse(PlaceMushX, PlaceMushY, BiomeWidth + 1, BiomeHeight + 1, x, y);

                    if (isMud && isInOval)
                    {
                        float noiseVal = SimplexNoise.FractalNoise2(seed, x / scaleX, y / scaleY);

                        if (noiseVal < threshold)
                            WorldGen.KillTile(x, y, noItem: true);
                    }
                }
            }

            for (int l = 0; l < 5; l++)
            {
                for (int x = PlaceMushX - BiomeWidth - 5; x <= PlaceMushX + BiomeWidth + 5; x++)
                {
                    for (int y = PlaceMushY - BiomeHeight - 5; y <= PlaceMushY + BiomeHeight + 5; y++)
                    {
                        bool isInOval = WorldGenTools.IsInEllipse(PlaceMushX, PlaceMushY, BiomeWidth + 1, BiomeHeight + 1, x, y);
                        int tileCount = WorldGenTools.MooreTiles(x, y);

                        if (tileCount > 4 && isInOval)
                        {
                            WorldGen.PlaceTile(x, y, TileID.Mudstone, true);
                        }
                        else if (tileCount < 4 && isInOval)
                        {
                            WorldGen.KillTile(x, y, noItem: true);
                        }
                    }
                }
            }
        }
    }
}