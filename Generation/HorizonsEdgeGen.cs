using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.IO;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;

namespace ABMod.Generation
{
    public class HorizonsEdgeGen : ModSystem
    {
        //Generation values
        public static int PlaceAsteroidsX;
        public static int PlaceAsteroidsY;

        private void HorizonGen(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Turning the skies red";

			//Biome X and Y origin
            if (GenVars.dungeonSide == -1)
            {
                PlaceAsteroidsX = Main.maxTilesX / 4;
            }
            else
            {
                PlaceAsteroidsX = Main.maxTilesX - (Main.maxTilesX / 4);
            }

			PlaceAsteroidsX = Main.maxTilesX / 4;
			PlaceAsteroidsY = (int)(Main.maxTilesY * 0.11f);

			//Area of the biome
			int biomeAreaX = (int)(Math.Abs(PlaceAsteroidsX - (Main.maxTilesX / 2)) * 0.8f);
			int biomeAreaY = Math.Abs(PlaceAsteroidsY - 40);

            int centerArea = (int)(biomeAreaX * 0.4f);
            int middleArea = (int)(biomeAreaX * 0.75f);
            int edgeArea = biomeAreaX;

            //Minimum asteroid amount
			int sizeThree = Main.maxTilesX >= 8400 ? 5 : Main.maxTilesX >= 6400 ? 4 : 3;
			int sizeTwo = Main.maxTilesX >= 8400 ? 12 : Main.maxTilesX >= 6400 ? 9 : 6;
			int sizeOne = Main.maxTilesX >= 8400 ? 35 : Main.maxTilesX >= 6400 ? 25 : 15;

            //Extra asteroids
            int xSizeThree = WorldGen.genRand.Next((int)(sizeThree * 0.45f));
            int xsizeTwo = WorldGen.genRand.Next((int)(sizeThree * 0.5f));
            int xsizeOne = WorldGen.genRand.Next((int)(sizeThree * 0.35f));

            //Time to place the asteroids
            int attempts = 0;

            while (sizeThree > 0 && attempts++ < 1000)
            {
                //Generate a random location
				int x = WorldGen.genRand.Next(PlaceAsteroidsX - centerArea, PlaceAsteroidsX + centerArea + 1);
				int y = WorldGen.genRand.Next(PlaceAsteroidsY - biomeAreaY, PlaceAsteroidsY + biomeAreaY + 1);

                //The if here
            }

            while (sizeTwo > 0 && attempts++ < 1000)
            {
                //Generate a random location
				int x = WorldGen.genRand.Next(PlaceAsteroidsX - middleArea, PlaceAsteroidsX + middleArea + 1);
				int y = WorldGen.genRand.Next(PlaceAsteroidsY - biomeAreaY, PlaceAsteroidsY + biomeAreaY + 1);

                //The if here
            }

            while (sizeOne > 0 && attempts++ < 1000)
            {
                //Generate a random location
				int x = WorldGen.genRand.Next(PlaceAsteroidsX - edgeArea, PlaceAsteroidsX + edgeArea + 1);
				int y = WorldGen.genRand.Next(PlaceAsteroidsY - biomeAreaY, PlaceAsteroidsY + biomeAreaY + 1);

                //The if here
            }
        }
    }
}