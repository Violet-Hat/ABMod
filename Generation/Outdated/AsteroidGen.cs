using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria;
using Terraria.IO;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using static Terraria.WorldGen;
using static tModPorter.ProgressUpdate;

using ABMod.Generation.Asteroids;

namespace ABMod.Generation
{
	public class AsteroidGen : ModSystem
	{
		//Reminder!
		//Rock is a small asteroid
		//Donut, Ore are regular asteroids
		//Center, Ice, Geode are big asteroids
		//Cave, Ice are giant asteroids
		
		private void HorizonGen(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Turning the skies red";

			//Biome X and Y origin
			float PlaceAsteroidsX = (Main.maxTilesX / 2);
			float PlaceAsteroidsY = (Main.maxTilesY / 14.5f);

			//Radius of the biome
			int biomeX = (int)(Main.maxTilesX / 5.5f);
			int biomeY = (int)(PlaceAsteroidsY * 0.55f);

			//Place the asteroids
			PlaceAsteroids((int)PlaceAsteroidsX, (int)PlaceAsteroidsY, biomeX, biomeY);
		}
		
		private void PlaceAsteroids(int PlaceAsteroidsX, int PlaceAsteroidsY, int biomeX, int biomeY)
		{
			//Config 2 time
			var config2 = WorldGenConfiguration.FromEmbeddedPath("Terraria.GameContent.WorldBuilding.Configuration.json");
			
			//Asteroid amounts
			int giantAsteroid = Main.maxTilesX >= 8400 ? 3 : Main.maxTilesX >= 6400 ? 2 : 1;
			int bigAsteroid = Main.maxTilesX >= 8400 ? 10 : Main.maxTilesX >= 6400 ? 7 : 4;
			int mediumAsteroid = Main.maxTilesX >= 8400 ? 26 : Main.maxTilesX >= 6400 ? 21 : 16;
			int smallAsteroid = Main.maxTilesX >= 8400 ? 30 : Main.maxTilesX >= 6400 ? 25 : 20;
			//int middleAsteroid = 1;
			
			//Central asteroid
			config2.CreateBiome<CentralAsteroid>().Place(new Point(PlaceAsteroidsX, PlaceAsteroidsY), GenVars.structures);
			
			//Giant Asteroids
			const int giantAttempts = 10000;
			int i = 0;
			while (giantAsteroid > 0 && i < giantAttempts)
			{
				//Generate random locations
				int x = WorldGen.genRand.Next(PlaceAsteroidsX - biomeX, PlaceAsteroidsX + biomeX);
				int y = WorldGen.genRand.Next(41, PlaceAsteroidsY + biomeY - 45);
				
				if(config2.CreateBiome<CaveAsteroid>().Place(new Point(x, y), GenVars.structures))
				{
					giantAsteroid--;
				}
				
				i++;
			}
			
			//Big Asteroids
			const int bigAttempts = 10000;
			i = 0;
			while (bigAsteroid > 0 && i < bigAttempts)
			{
				//Generate random locations
				int x = WorldGen.genRand.Next(PlaceAsteroidsX - biomeX, PlaceAsteroidsX + biomeX);
				int y = WorldGen.genRand.Next(41, PlaceAsteroidsY + biomeY - 35);
				
				if(WorldGen.genRand.NextBool(2))
				{
					if(config2.CreateBiome<GeodeAsteroid>().Place(new Point(x, y), GenVars.structures))
					{
						bigAsteroid--;
					}
				}
				else
				{
					if(config2.CreateBiome<IceAsteroid>().Place(new Point(x, y), GenVars.structures))
					{
						bigAsteroid--;
					}
				}
				
				i++;
			}
			
			//Medium Asteroids
			const int mediumAttempts = 10000;
			i = 0;
			while (mediumAsteroid > 0 && i < mediumAttempts)
			{
				//Generate random locations
				int x = WorldGen.genRand.Next(PlaceAsteroidsX - biomeX, PlaceAsteroidsX + biomeX);
				int y = WorldGen.genRand.Next(41, PlaceAsteroidsY + biomeY - 15);
				
				if(WorldGen.genRand.NextBool(2))
				{
					if(config2.CreateBiome<DonutAsteroid>().Place(new Point(x, y), GenVars.structures))
					{
						mediumAsteroid--;
					}
				}
				else
				{
					if(config2.CreateBiome<OreAsteroid>().Place(new Point(x, y), GenVars.structures))
					{
						mediumAsteroid--;
					}
				}
				
				i++;
			}
			
			//Small Asteroids
			const int smallAttempts = 10000;
			i = 0;
			while (smallAsteroid > 0 && i < smallAttempts)
			{
				//Generate random locations
				int x = WorldGen.genRand.Next(PlaceAsteroidsX - biomeX, PlaceAsteroidsX + biomeX);
				int y = WorldGen.genRand.Next(41, PlaceAsteroidsY + biomeY - 5);
				
				if(config2.CreateBiome<RockAsteroid>().Place(new Point(x, y), GenVars.structures))
				{
					smallAsteroid--;
				}
				
				i++;
			}
		}
		
		/*
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			//Add the biome in the worldgen task
			int BiomesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
			if (BiomesIndex != -1)
			{
				tasks.Insert(BiomesIndex + 1, new PassLegacy("Horizon's Edge", HorizonGen));
			}
		}
		*/
	}
}