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

using ABMod.Common;
﻿using ABMod.Tiles.GreenMushroomBiome;
using ABMod.Tiles.GreenMushroomBiome.Trees;

namespace ABMod.Generation
{
	public class GreenMushroomGen : ModSystem
	{
		//Tiles
		public static ushort mushroomsoil = (ushort)ModContent.TileType<MushroomSoil>(),
		mushroompasture = (ushort)ModContent.TileType<MushroomPasture>();
		
		//Generation values
		public static int PlaceBiomeX = 0;
		public static int PlaceBiomeY = 0;
		public static int BiomeRadiusX = 0;
		public static int BiomeRadiusY = 0;
		
		private void GreenBiomeGen(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Planting contermeasures against the rot";
			Flags.GreenCavesPositions.Clear();

			//Randomized Y position
			int randomY = (WorldGen.genRand.Next(200, 225) / 100);

			//Biome X and Y position
			PlaceBiomeX = (Main.maxTilesX / 2);
			PlaceBiomeY = (int)Main.maxTilesY / randomY;

			Point originCenter = new((int)PlaceBiomeX, (int)PlaceBiomeY);
			
			//Radius
			BiomeRadiusX = Main.maxTilesX >= 8400 ? 220 : Main.maxTilesX >= 6400 ? 180 : 140;
			BiomeRadiusX = BiomeRadiusX * (WorldGen.genRand.Next(100, 150) / 100);
			BiomeRadiusY = (int)(BiomeRadiusX * 0.75f);
			
			//Shape stuff
			ShapeData externalLayer = new ShapeData();
			ShapeData internalLayer = new ShapeData();
			
			//Shape making
			GenAction blotches = new Modifiers.Blotches(2, 0.4);
			GenAction skipTiles = new Modifiers.SkipTiles(new ushort[] { TileID.Containers });
			
			WorldUtils.Gen(originCenter, new Shapes.Circle(BiomeRadiusX, BiomeRadiusY), Actions.Chain(new GenAction[]
			{
				blotches.Output(externalLayer),
				skipTiles.Output(externalLayer),
			}));
			WorldUtils.Gen(originCenter, new Shapes.Circle(BiomeRadiusX - 10, BiomeRadiusY - 10), Actions.Chain(new GenAction[]
			{
				blotches.Output(internalLayer),
			}));
			
			//Remove non-solids
			WorldUtils.Gen(originCenter, new ModShapes.All(externalLayer), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsNotSolid(),
				new Actions.Clear(),
			}));
			
			//Place them
			WorldUtils.Gen(originCenter, new ModShapes.All(internalLayer), Actions.Chain(new GenAction[]
			{
				new Actions.SetTile(mushroomsoil, true, true),
			}));
			
			WorldUtils.Gen(originCenter, new ModShapes.All(externalLayer), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsSolid(),
				new Actions.SetTile(mushroomsoil, true, true),
			}));
			
			//Empty spaces
			GenerateHoles((int)PlaceBiomeX, (int)PlaceBiomeY, BiomeRadiusX, BiomeRadiusY, blotches);
			
			//Grass
			GrowGrass(originCenter, externalLayer);

			//Trees
			MoreMushrooms(PlaceBiomeX, BiomeRadiusX, PlaceBiomeY, BiomeRadiusY);
		}
		
		//Random caves
		private void GenerateHoles(int PlaceBiomeX, int PlaceBiomeY, int BiomeRadiusX, int BiomeRadiusY, GenAction blotches)
		{
			//Calculate a starting point in the X and Y axis
			int BeginningX = PlaceBiomeX - BiomeRadiusX * 4;
			int EndingX = PlaceBiomeX + BiomeRadiusX * 4;
			
			int BeginningY = PlaceBiomeY - BiomeRadiusY * 4;
			int EndingY = PlaceBiomeY + BiomeRadiusY * 4;
			
			for(int j = BeginningY; j < EndingY; j += 10)
			{
				for(int i = BeginningX; i < EndingX; i++)
				{
					//These are important
					float CavesDistance = Main.maxTilesX >= 8400 ? 45 : Main.maxTilesX >= 6400 ? 35 : 25;
					int distance = Main.maxTilesX >= 8400 ? 40 : Main.maxTilesX >= 6400 ? 30 : 20;
					
					Tile tile = Framing.GetTileSafely(i, j);
					
					//If the tile isn't Mushroom Soil, skip the rest
					if(tile.TileType != mushroomsoil)
					{
						continue;
					}
					
					//If there isn't enough Mushroom Soil in the area, skip the rest
					bool DontPlace = false;
					for(int x = i - distance; x <= i + distance; x++)
					{
						if(Framing.GetTileSafely(x, j).TileType != mushroomsoil)
						{
							DontPlace = true;
							break;
						}
					}
					
					for(int y = j - distance; y <= j + distance; y++)
					{
						if(Framing.GetTileSafely(i, y).TileType != mushroomsoil)
						{
							DontPlace = true;
							break;
						}
					}
					
					if(DontPlace)
					{
						continue;
					}
					
					//Check if the caves are too close to each other. If it's too close, skip the rest
					Vector2 PositionToCheck = new Vector2(i, j);
					bool tooClose = false;
					
					foreach(var ExistingPosition in Flags.GreenCavesPositions)
					{
						if(Vector2.DistanceSquared(PositionToCheck, ExistingPosition) < CavesDistance * CavesDistance)
						{
							tooClose = true;
							break;
						}
					}
					
					if(tooClose)
					{
						continue;
					}
					
					//Generate the hole with a 50% chance
					if(WorldGen.genRand.NextBool())
					{
						//OffSet of the hole
						int OffSetY = WorldGen.genRand.Next(-15, 16);
						
						//Coordinates where the hole will generate
						Point newCenter = new(i, j + OffSetY);
						
						//Size of the hole
						int worldSizeExtra = Main.maxTilesX >= 8400 ? 15 : Main.maxTilesX >= 6400 ? 7 : 0;
						
						Vector2 Origin = new Vector2(PlaceBiomeX, PlaceBiomeY);
						float VeryClose = 50;
						float AlmostCenter = 25;
						
						int SizeX = 0;
						int SizeY = 0;
						
						if(Vector2.DistanceSquared(PositionToCheck, Origin) < AlmostCenter * AlmostCenter)
						{
							SizeX = WorldGen.genRand.Next(32, 38) + worldSizeExtra;
							SizeY = (int)(SizeX * 0.55f);
						}
						else
						{
							if(Vector2.DistanceSquared(PositionToCheck, Origin) < VeryClose * VeryClose)
							{
								SizeX = WorldGen.genRand.Next(22, 30) + worldSizeExtra;
								SizeY = (int)(SizeX * 0.55f);
							}
							else
							{
								SizeX = WorldGen.genRand.Next(12, 20) + worldSizeExtra;
								SizeY = (int)(SizeX * 0.55f);
							}
						}
						
						//Shape making
						ShapeData hole = new ShapeData();
						
						WorldUtils.Gen(newCenter, new Shapes.Circle(SizeX, SizeY), Actions.Chain(new GenAction[]
						{
							blotches.Output(hole),
						}));
						
						WorldUtils.Gen(newCenter, new ModShapes.All(hole), Actions.Chain(new GenAction[]
						{
							new Actions.Clear(),
						}));
						
						//Save the position of the hole
						Flags.GreenCavesPositions.Add(new Vector2(i, j + OffSetY));
					}
				}
			}
		}
		
		private void GrowGrass(Point originCenter, ShapeData externalLayer)
		{
			//Grass
			WorldUtils.Gen(originCenter, new ModShapes.All(externalLayer), Actions.Chain(new GenAction[]
			{
				new Modifiers.OnlyTiles(mushroomsoil),
				new Modifiers.IsTouchingAir(true),
				new Actions.SetTile(mushroompasture, false, true),
			}));

			//Grass patch
			WorldUtils.Gen(originCenter, new ModShapes.All(externalLayer), Actions.Chain(new GenAction[]
			{
				new Modifiers.OnlyTiles(mushroomsoil),
				new Modifiers.Dither(.99),
				new Actions.SetTile(mushroompasture, false, true),
			}));
		}
		
		//Random Tree generation
		private void MoreMushrooms(float PlaceBiomeX, int BiomeRadiusX, float PlaceBiomeY, int BiomeRadiusY)
		{
			for(int X = (int)PlaceBiomeX - (int)BiomeRadiusX; X <= (int)PlaceBiomeX + (int)BiomeRadiusX; X++)
			{
				for(int Y = (int)PlaceBiomeY - (int)BiomeRadiusY; Y <= (int)PlaceBiomeY + (int)BiomeRadiusY; Y++)
				{
					if(Main.tile[X, Y].TileType == (ushort)ModContent.TileType<MushroomPasture>())
                    {
                        if (WorldGen.genRand.NextBool(6))
						{
							if(WorldgenTools.GrowTreeCheck(X, Y, 4, 18, ModContent.TileType<GreenFungusTree>()))
							{
								GreenFungusTree.Grow(X, Y - 1, 8, 15, false);
							}
						}
                    }
				}
			}
		}
		
		/*
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			//Add the biome in the worldgen task
			int BiomesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
			if (BiomesIndex != -1)
			{
				tasks.Insert(BiomesIndex + 1, new PassLegacy("Green Mushroom Biome", GreenBiomeGen));
			}
		}
		*/
	}
}