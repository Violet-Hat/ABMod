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

            int startX = PlaceMushX - BiomeWidth;
            int endX = PlaceMushX + BiomeWidth;

            int startY = PlaceMushY - BiomeHeight;
            int endY = PlaceMushY + BiomeHeight;

            for (int x = startX; x <= endX; x++) //Loop to go through every x coordinate in the area
            {
                for (int y = startY; y <= endY; y++) //Loop to go through every y coordinate in the area
                {
                    bool isInsideOval = WorldGenTools.IsInEllipse(PlaceMushX, PlaceMushY, BiomeWidth + 1, BiomeHeight + 1, x, y);

                    //Get the noise value
                    float noiseVal = SimplexNoise.FractalNoise2(seed, x / scaleX, y / scaleY);

                    //If the value is smaller than the threshold, kill the tile
                    if (noiseVal < threshold && isInsideOval)
                        WorldGen.KillTile(x, y, noItem: true);
                }
            }

            //Smoothing
            startX = PlaceMushX - BiomeWidth - 5;
            endX = PlaceMushX + BiomeWidth + 5;

            startY = PlaceMushY - BiomeHeight - 5;
            endY = PlaceMushY + BiomeHeight + 5;

            for (int l = 0; l < 5; l++) //Smoothing loop
            {
                for (int x = startX; x <= endX; x++) //Loop to go through every x coordinate in the area
                {
                    for (int y = startY; y <= endY; y++) //Loop to go through every y coordinate in the area
                    {
                        bool isInsideOval = WorldGenTools.IsInEllipse(PlaceMushX, PlaceMushY, BiomeWidth + 1, BiomeHeight + 1, x, y);

                        //Get the number of neighbors using Moore Neighborhood
                        int tileCount = WorldGenTools.MooreTiles(x, y);

                        if (tileCount > 4 && isInsideOval) //If there's more than 4 neighbors, place a cell
                        {
                            WorldGen.PlaceTile(x, y, TileID.Mudstone, true);
                        }
                        else if (tileCount < 4 && isInsideOval) //If there's less than 4 neighbors, kill the cell
                        {
                            WorldGen.KillTile(x, y, noItem: true);
                        }
                    }
                }
            }
        }
    }
}