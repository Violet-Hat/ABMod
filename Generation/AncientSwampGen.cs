using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.IO;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;

using ABMod.Common;
using ABMod.Tiles.AncientSwampBiome;

namespace ABMod.Generation
{
	public class AncientSwampGen : ModSystem
	{
		//Generation values
		public static int PlaceSwampX = 0;
		public static int PlaceSwampY = 0;
		public static int BiomeWidth = 0;
        public static int BiomeHeightLimit = 0;
		public static List <int> structuresPosX = new List<int>();

		private void SwampGen(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Creating a time anomaly";

			//Clear tiles
			Flags.SoftDirtPoints.Clear();

			//X & Y value of the swamp origin, make sure X is on the jungle side
			if (GenVars.dungeonSide == -1)
			{
				PlaceSwampX = Main.maxTilesX - (Main.maxTilesX / 8);
			}
			else
			{
				PlaceSwampX = (int)(Main.maxTilesX / 8f);
			}

			PlaceSwampY = (int)Main.worldSurface - (Main.maxTilesY / 8);

			//Width & height of the swamp
			BiomeWidth = Main.maxTilesX >= 8400 ? 355 : Main.maxTilesX >= 6400 ? 250 : 175;
			BiomeHeightLimit = (int)(Main.maxTilesY / 1.75f);

			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth;

			//Place the biome base
			int progressEigth = (int)(BiomeWidth * 2 / 8f);

			int tileType;
			int wallType;

			int limit = (int)Main.worldSurface + 30;
			int deepLimit = (int)Main.worldSurface + 40;

			for (int X = StartX; X <= EndX; X++)
			{
				//Progress setters
				progress.Set((float)(X - StartX) / (EndX - StartX));

				//Tile replacement loop
				for (int Y = PlaceSwampY; Y < BiomeHeightLimit; Y++)
				{
					//Tile type check
					tileType = TileTypeCheck(Y, limit, deepLimit);
					wallType = WallTypeCheck(Y, limit, deepLimit);

					if (WorldgenTools.NoFloatingIslands(X, Y, 45))
					{
						Tile tile = Main.tile[X, Y];

						//Replace tiles
						if (tile.HasTile && !BiomeTile.IsFloatingIslandTile(X, Y))
						{
							//Clear if not solid
							if (WorldGen.SolidTile(X, Y))
							{
								tile.TileType = (ushort)tileType;
							}
							else
							{
								Main.tile[X, Y].ClearTile();
							}
						}

						//Place tiles on ebonstone and crimstone walls because THEY SUCK
						if (!tile.HasTile && (tile.WallType == WallID.EbonstoneUnsafe || tile.WallType == WallID.CrimstoneUnsafe))
						{
							tile.TileType = (ushort)tileType;
						}

						//Replace walls
						if (tile.WallType > WallID.None)
						{
							tile.WallType = (ushort)wallType;
						}

						//Place walls
						if(tile.WallType == WallID.None && Y > (int)Main.worldSurface)
                        {
                            WorldGen.PlaceWall(X, Y, wallType, true);
                        }
					}
				}
			}

			//Fixed base and hole fill
			for (int X = StartX; X <= EndX; X++)
			{
				for (int Y = (int)Main.worldSurface - 70; Y < BiomeHeightLimit; Y++)
				{
					Tile tile = Main.tile[X, Y];

					if (!tile.HasTile)
					{
						tileType = TileTypeCheck(Y, limit, deepLimit);
						wallType = WallTypeCheck(Y, limit, deepLimit);

						WorldGen.PlaceTile(X, Y,tileType, true);
						WorldGen.PlaceWall(X, Y, wallType, true);
					}
				}
			}

			//Dithering
			for (int X = StartX - 10; X < EndX + 10; X++)
			{
				if(X < StartX || X > EndX)
				{
					for (int Y = PlaceSwampY; Y < BiomeHeightLimit; Y++)
					{
						//Tile type check
						tileType = TileTypeCheck(Y, limit, deepLimit);
						wallType = WallTypeCheck(Y, limit, deepLimit);

						if (WorldgenTools.NoFloatingIslands(X, Y, 45))
						{
							Tile tile = Main.tile[X, Y];

							//Replace tiles
							if (tile.HasTile && tile.TileType != TileID.Cloud && tile.TileType != TileID.RainCloud && !BiomeTile.IsSwampTile(X, Y))
							{
								if (WorldGen.SolidTile(X, Y) && WorldGen.genRand.NextBool())
								{
									tile.TileType = (ushort)tileType;
								}
							}

							//Replace walls
							if (tile.WallType > WallID.None && WorldGen.genRand.NextBool())
							{
								tile.WallType = (ushort)wallType;
							}

							//Place walls
							if(tile.WallType == WallID.None && Y > (int)Main.worldSurface && WorldGen.genRand.NextBool())
							{
								WorldGen.PlaceWall(X, Y, wallType, true);
							}
						}
					}
				}
			}

			tileType = ModContent.TileType<AncientStone>();
			wallType = ModContent.WallType<AncientStoneWall>();

			for (int X = StartX - 10; X <= EndX + 10; X++)
			{
				for (int Y = BiomeHeightLimit; Y <= BiomeHeightLimit + 10; Y++)
				{
					Tile tile = Main.tile[X, Y];

					//Replace tiles
					if (tile.HasTile && tile.TileType != TileID.Cloud && tile.TileType != TileID.RainCloud && !BiomeTile.IsSwampTile(X, Y))
					{
						if (WorldGen.SolidTile(X, Y) && WorldGen.genRand.NextBool())
						{
							tile.TileType = (ushort)tileType;
						}
					}

					//Replace walls
					if (tile.WallType > WallID.None && WorldGen.genRand.NextBool())
					{
						tile.WallType = (ushort)wallType;
					}

					//Place walls
					if(tile.WallType == WallID.None && Y > (int)Main.worldSurface && WorldGen.genRand.NextBool())
					{
						WorldGen.PlaceWall(X, Y, wallType, true);
					}
				}
			}
		}

		private void SwampFlattening(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Flattening the anomaly";

			//Beginning and end of the biome
			int StartX = PlaceSwampX - BiomeWidth;
			int MiddleX = PlaceSwampX;
			int EndX = PlaceSwampX + BiomeWidth;

			//Ground points
			int LeftY = FindGround(StartX);
			int RightY = FindGround(EndX);

			int MiddleY = LeftY > RightY ? LeftY : RightY;

			if (Math.Abs(LeftY - RightY) < 25)
			{
				MiddleY += 25 - Math.Abs(LeftY - RightY);
			}
			
			progress.Set(0.33);

			//Connect points and flatten the terrain
			ConnectPoints(new Vector2(StartX, LeftY), new Vector2(MiddleX, MiddleY), new Vector2(EndX, RightY));
			progress.Set(0.65);

			//Patches of stone and dirt
			int heightLimit = LeftY < RightY ? LeftY : RightY;
			heightLimit -= 5;

			int i = 0;
			int stoneCount = Main.maxTilesX >= 8400 ? 25 : Main.maxTilesX >= 6400 ? 19 : 7;
			stoneCount += WorldGen.genRand.Next(15, 25);

			while (stoneCount > 0 && i++ < 10000)
			{
				int X = WorldGen.genRand.Next(StartX, EndX + 1);
				int Y = WorldGen.genRand.Next(heightLimit, (int)Main.worldSurface + 25);

				Tile tile = Main.tile[X, Y];

				if (tile.HasTile && tile.TileType == ModContent.TileType<AncientDirt>())
				{
					WorldGen.TileRunner(X, Y, WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(20, 30), ModContent.TileType<AncientStone>(), false, 0f, 0f, false, true);
					stoneCount--;
				}
			}

			progress.Set(0.75);
			
			int j = 0;
			int dirtCount = Main.maxTilesX >= 8400 ? 28 : Main.maxTilesX >= 6400 ? 21 : 9;
			dirtCount += WorldGen.genRand.Next(25, 45);

			while (dirtCount > 0 && j++ < 10000)
            {
				int X = WorldGen.genRand.Next(StartX, EndX + 1);
				int Y = WorldGen.genRand.Next((int)Main.worldSurface + 50, BiomeHeightLimit - 50);

				Tile tile = Main.tile[X, Y];

				if(tile.HasTile && tile.TileType == ModContent.TileType<AncientStone>())
                {
					WorldGen.TileRunner(X, Y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(30, 50), ModContent.TileType<AncientDirt>(), false, 0f, 0f, false, true);
					dirtCount--;
                }
            }
		}

		private void SwampCaves(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Digging caves in the anomaly";

			int seed = WorldGen.genRand.Next();
			int octaves = 5;
			float clearChance = 0.6f;

			//Upper caves
			int CaveStart = (int)(Main.worldSurface - 5);
			int CaveEnd = (int)(Main.worldSurface + 45);

			for (int X = PlaceSwampX - BiomeWidth; X <= PlaceSwampX + BiomeWidth; X++)
			{
				for (int Y = CaveStart; Y < CaveEnd; Y++)
				{
					//Perlin noise values
					//These are for transition purposes
					float horizontalOffsetNoise = WorldgenTools.PerlinNoise2D(X / 225f, Y / 225f, octaves, unchecked(seed + 1)) * 0.01f;
					float cavePerlinValue = WorldgenTools.PerlinNoise2D(X / 550f, Y / 300f, octaves, seed) + 0.5f + horizontalOffsetNoise;
					float cavePerlinValue2 = WorldgenTools.PerlinNoise2D(X / 550f, Y / 300f, octaves, unchecked(seed - 1)) + 0.5f;
					float caveNoiseMap = (cavePerlinValue + cavePerlinValue2) * 0.5f;
					float caveCreationThreshold = horizontalOffsetNoise * 3.5f + 0.235f;

					//Remove tiles based on the noise
					if ((caveNoiseMap * caveNoiseMap > caveCreationThreshold) && (WorldGen.genRand.NextFloat() < clearChance))
					{
						WorldGen.KillTile(X, Y);
					}
				}
			}

			CaveStart = (int)(Main.worldSurface + 30);
			CaveEnd = (int)(Main.rockLayer + 15);

			for (int X = PlaceSwampX - BiomeWidth; X <= PlaceSwampX + BiomeWidth; X++)
			{
				for (int Y = CaveStart; Y < CaveEnd; Y++)
				{
					//Perlin noise values
					//Higher Y values for more vertical caves
					float horizontalOffsetNoise = WorldgenTools.PerlinNoise2D(X / 225f, Y / 225f, octaves, unchecked(seed + 1)) * 0.01f;
					float cavePerlinValue = WorldgenTools.PerlinNoise2D(X / 350f, Y / 575f, octaves, seed) + 0.5f + horizontalOffsetNoise;
					float cavePerlinValue2 = WorldgenTools.PerlinNoise2D(X / 350f, Y / 575f, octaves, unchecked(seed - 1)) + 0.5f;
					float caveNoiseMap = (cavePerlinValue + cavePerlinValue2) * 0.5f;
					float caveCreationThreshold = horizontalOffsetNoise * 3.5f + 0.20f;

					//Remove tiles based on the noise
					if ((caveNoiseMap * caveNoiseMap > caveCreationThreshold) && (WorldGen.genRand.NextFloat() < clearChance))
					{
						WorldGen.KillTile(X, Y);
					}
				}
			}

			progress.Set(0.5);

			//Lower caves
			CaveStart = (int)Main.rockLayer;
			CaveEnd = BiomeHeightLimit;

			for (int X = PlaceSwampX - BiomeWidth; X <= PlaceSwampX + BiomeWidth; X++)
			{
				for (int Y = CaveStart; Y < CaveEnd; Y++)
				{
					//Perlin noise values
					//Higher X values for more horizontal caves
					float horizontalOffsetNoise = WorldgenTools.PerlinNoise2D(X / 225f, Y / 225f, octaves, unchecked(seed + 1)) * 0.01f;
					float cavePerlinValue = WorldgenTools.PerlinNoise2D(X / 575f, Y / 350f, octaves, seed) + 0.5f + horizontalOffsetNoise;
					float cavePerlinValue2 = WorldgenTools.PerlinNoise2D(X / 575f, Y / 350f, octaves, unchecked(seed - 1)) + 0.5f;
					float caveNoiseMap = (cavePerlinValue + cavePerlinValue2) * 0.5f;
					float caveCreationThreshold = horizontalOffsetNoise * 3.5f + 0.20f;

					//Remove tiles based on the noise
					if ((caveNoiseMap * caveNoiseMap > caveCreationThreshold) && (WorldGen.genRand.NextFloat() < clearChance))
					{
						WorldGen.KillTile(X, Y);
					}
				}
			}

			progress.Set(0.85);

			//Smooth the noise
			for (int l = 0; l < 10; l++)
            {
                for (int x = PlaceSwampX - BiomeWidth; x <= PlaceSwampX + BiomeWidth; x++)
                {
                    for(int y = (int)Main.worldSurface - 10; y <= BiomeHeightLimit; y++)
                    {
                        int tileCount = WorldgenTools.CheckTiles(x, y);

                        if (tileCount > 4)
                        {
							int tileType = TilesAround(x, y);

                            WorldGen.PlaceTile(x, y, tileType, true);
                        }
                        else if (tileCount < 4)
                        {
                            WorldGen.KillTile(x, y);
                        }
                    }
                }
            }
		}
		
		private void SwampAmbience(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Adding a nice ambience";

			//List hell
			List <int> values = new List<int>();
			List <String> structuresNames = new List<String>()
			{
				"SwampStruct1",
				"SwampStruct2",
				"SwampStruct3",
				"SwampStruct4",
				"SwampStruct5",
				"SwampStruct6"
			};

			//Start and end
			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth;

			//Structure values
			int numSurfaceStructures = Main.maxTilesX >= 8400 ? 6 : Main.maxTilesX >= 6400 ? 5 : 4;
			int structurePartition = BiomeWidth * 2 / (numSurfaceStructures + 1);
			int structureMaxRange = Main.maxTilesX >= 8400 ? 9 : Main.maxTilesX >= 6400 ? 7 : 5;

			for (int i = 0; i < numSurfaceStructures; i++) values.Add(i + 1);
			
            //Tunnel
			int rando;

            if (GenVars.dungeonSide == -1)
            {
                rando = -structurePartition / 3;
				values.Remove(1);
				GenerateTunnel(StartX + structurePartition + rando);
            }
            else
            {
                rando = structurePartition / 3;
				values.Remove(numSurfaceStructures);
				GenerateTunnel(StartX + (structurePartition * numSurfaceStructures) + rando);
            }

			progress.Set(0.1);

			//Lake for F I S H I NG
			int index = WorldGen.genRand.Next(values.Count);
			int placeLake = values[index];
			int lakeWidth = Main.maxTilesX >= 8400 ? 40 : Main.maxTilesX >= 6400 ? 30 : 20;

			values.Remove(placeLake);
			GenerateLake(StartX + (structurePartition * placeLake), lakeWidth, structureMaxRange);

			progress.Set(0.2);

			//Last structures
			foreach (int placement in values)
			{
				index = WorldGen.genRand.Next(structuresNames.Count);
                string structureFile = structuresNames[index];
				
				structuresNames.Remove(structureFile);
				
				int structOffsetY = GetStructureOffset(structureFile);
				int structWidth = GetStructureWidth(structureFile);

				GenerateStructure(StartX + (structurePartition * placement), structureFile, structureMaxRange, structWidth, structOffsetY);
			}

			progress.Set(0.4);

			//GRASS
			for(int x = StartX; x <= EndX; x++)
			{
				for(int y = PlaceSwampY; y <= Main.worldSurface; y++)
                {
                    WorldGen.SpreadGrass(x, y, ModContent.TileType<PreservedDirt>(), ModContent.TileType<PrehistoricMoss>());
                }
			}

			//Bush walls for pretty
			int bushAmount = Main.maxTilesX >= 8400 ? 10 : Main.maxTilesX >= 6400 ? 8 : 6;
			int halfBushAmount = bushAmount / 2;
			int tries = 0;

			while (bushAmount > 0 && tries++ < 10000)
			{
				bool tooClose = false;
				int x;

				//So it isn't too busy in one half hopefully
				if (bushAmount > halfBushAmount)
				{
					x = WorldGen.genRand.Next(StartX, PlaceSwampX);
				}
				else
				{
					x = WorldGen.genRand.Next(PlaceSwampX, EndX + 1);
				}

				foreach(int posX in structuresPosX)
				{
					if (Math.Abs(x - posX) < 20)
					{
						tooClose = true;
					}
				}

				//Place the bush if it's not too close to artifical structures
				if (!tooClose)
				{
					int y = FindGround(x) + 1;
					int bushWidth = WorldGen.genRand.Next(8, 13);

					switch (WorldGen.genRand.Next(3))
					{
						default:
							WorldUtils.Gen(new Point(x, y), new Shapes.Circle(bushWidth, bushWidth), Actions.Chain(new GenAction[]
							{
								new Modifiers.Blotches(2, 0.4),
								new Actions.PlaceWall((ushort)ModContent.WallType<PrehistoricMossWall>(), true),
							}));
							break;

						case 1:
							WorldUtils.Gen(new Point(x, y), new Shapes.Circle((int)(bushWidth * 0.8), bushWidth), Actions.Chain(new GenAction[]
							{
								new Modifiers.Blotches(2, 0.4),
								new Actions.PlaceWall((ushort)ModContent.WallType<PrehistoricMossWall>(), true),
							}));
							break;

						case 2:
							WorldUtils.Gen(new Point(x, y), new Shapes.Circle(bushWidth, (int)(bushWidth * 0.8)), Actions.Chain(new GenAction[]
							{
								new Modifiers.Blotches(2, 0.4),
								new Actions.PlaceWall((ushort)ModContent.WallType<PrehistoricMossWall>(), true),
							}));
							break;
					}
					bushAmount--;
				}
			}

			progress.Set(0.7);

			//Vegetation
		}

		//Check the tile type
		public static int TileTypeCheck(int Y, int limit, int deepLimit)
		{
			if (Y >= deepLimit)
			{
				return ModContent.TileType<AncientStone>();
			}
			else if (Y >= limit)
			{
				if (WorldGen.genRand.NextBool())
				{
					return ModContent.TileType<AncientStone>();
				}
				else
				{
					return ModContent.TileType<AncientDirt>();
				}
			}
			else
			{
				return ModContent.TileType<AncientDirt>();
			}
		}

		//Check the wall type
		public static int WallTypeCheck(int Y, int limit, int deepLimit)
		{
			if (Y >= deepLimit)
			{
				return ModContent.WallType<AncientStoneWall>();
			}
			else if (Y >= limit)
			{
				if (WorldGen.genRand.NextBool())
				{
					return ModContent.WallType<AncientStoneWall>();
				}
				else
				{
					return ModContent.WallType<AncientDirtWall>();
				}
			}
			else
			{
				return ModContent.WallType<AncientDirtWall>();
			}
		}
		
		//Connect 2 points
		public static void ConnectPoints(Vector2 p2, Vector2 p1, Vector2 p0)
		{
			int segments = 10000;

			//Place the base curve
			for (int i = 0; i < segments; i++)
			{
				float t = i / (float)segments;
				Vector2 Position = BezierCurve.cuadraticBezier(t, p0, p1, p2);

				//Place tiles
				for (int Y = (int)Position.Y; Y <= Main.worldSurface; Y++)
				{
					Tile tile = Main.tile[(int)Position.X, Y];

					if (!tile.HasTile)
					{
						WorldGen.PlaceTile((int)Position.X, Y, ModContent.TileType<AncientDirt>(), true);
						WorldGen.PlaceWall((int)Position.X, Y, ModContent.WallType<AncientDirtWall>(), true);
					}
				}

				//Clear tiles above the line
				int heightLimit = (int)(Main.worldSurface * 0.35f);

				for (int Y = heightLimit; Y < (int)Position.Y; Y++)
				{
					if (BiomeTile.IsSwampTile((int)Position.X, Y) || Main.tile[(int)Position.X, Y].WallType == ModContent.WallType<AncientDirtWall>())
					{
						Main.tile[(int)Position.X, Y].ClearEverything();
					}
				}

				//Save points
				if (!Flags.SoftDirtPoints.Contains(new Point((int)Position.X, (int)Position.Y)) && !Flags.SoftDirtPoints.Contains(new Point((int)Position.X, (int)Position.Y - 1)))
				{
					Flags.SoftDirtPoints.Add(new Point((int)Position.X, (int)Position.Y));
				}
			}

			//Place soft dirt surface
			foreach (Point point in Flags.SoftDirtPoints)
			{
				for (int j = 0; j < 50; j++)
				{
					Tile tile = Main.tile[point.X, point.Y + j];

					if (j >= 40)
					{
						if (WorldGen.genRand.NextBool())
						{
							tile.TileType = (ushort)ModContent.TileType<PreservedDirt>();
							WorldGen.PlaceTile(point.X, point.Y + j, ModContent.TileType<PreservedDirt>(), true);

							tile.WallType = (ushort)ModContent.WallType<PreservedDirtWall>();
							WorldGen.PlaceWall(point.X, point.Y + j, ModContent.WallType<PreservedDirtWall>(), true);
						}
					}
					else
					{
						tile.TileType = (ushort)ModContent.TileType<PreservedDirt>();
						WorldGen.PlaceTile(point.X, point.Y + j, ModContent.TileType<PreservedDirt>(), true);

						tile.WallType = (ushort)ModContent.WallType<PreservedDirtWall>();
						WorldGen.PlaceWall(point.X, point.Y + j, ModContent.WallType<PreservedDirtWall>(), true);
					}
				}

				if (Main.tile[point.X, point.Y].WallType > WallID.None)
				{
					WorldGen.KillWall(point.X, point.Y);
				}

				if(WorldGen.genRand.NextBool() && Main.tile[point.X + 1, point.Y].HasTile)
                {
					WorldGen.PlaceTile(point.X, point.Y - 1, ModContent.TileType<PreservedDirt>(), true);
					WorldGen.PlaceTile(point.X + 1, point.Y - 1, ModContent.TileType<PreservedDirt>(), true);
                }
			}
		}

		//Check tiles around and return which one is more present
		public static int TilesAround(int x, int y)
		{
			int softCount = 0;
			int dirtCount = 0;
			int stoneCount = 0;

            for (int nebX = x - 1; nebX <= x + 1; nebX++)
            {
                for (int nebY = y - 1; nebY <= y + 1; nebY++)
                {
                    if (nebX != x || nebY != y)
                    {
                        if (Main.tile[nebX, nebY].TileType == ModContent.TileType<AncientStone>())
                            stoneCount++;

						if (Main.tile[nebX, nebY].TileType == ModContent.TileType<AncientDirt>())
							dirtCount++;

						if (Main.tile[nebX, nebY].TileType == ModContent.TileType<PreservedDirt>())
							softCount++;
                    }
                }
            }

			if (softCount > dirtCount)
			{
				return ModContent.TileType<PreservedDirt>();
			}
			else if (dirtCount > softCount || dirtCount > stoneCount)
			{
				return ModContent.TileType<AncientDirt>();
			}
			else if (stoneCount > dirtCount)
			{
				return ModContent.TileType<AncientStone>();
			}
			else
			{
				if (softCount == dirtCount)
				{
					return WorldGen.genRand.NextBool() ? ModContent.TileType<PreservedDirt>() : ModContent.TileType<AncientDirt>();
				}
				else
				{
					return WorldGen.genRand.NextBool() ? ModContent.TileType<AncientDirt>() : ModContent.TileType<AncientStone>();
				}
			}
		}

		//Generate tunnel
		public static void GenerateTunnel(int x)
		{
			//Find ground
			int y = FindGround(x);
			y -= 5;

			//Tunnel time
			int area = Main.maxTilesX >= 8400 ? 55 : Main.maxTilesX >= 6400 ? 45 : 35;
			int limit = Math.Abs((int)(Main.worldSurface + 45) - y);
			int currentX = 0;

			float roughness = 0.5f;
			float curvyness = 0.5f;

			int tunnelWidth = 3;

			for (int j = 0; j <= limit; j++)
			{
				//Can be wider?
				if (WorldGen.genRand.NextFloat() < roughness)
				{
					tunnelWidth += WorldGen.genRand.Next(-9, 10);

					//If it's too small, make it the minimum
					if (tunnelWidth < 3)
					{
						tunnelWidth = 3;
					}

					//If it's too large, make it the maximum
					if (tunnelWidth > 9)
					{
						tunnelWidth = 9;
					}
				}

				//Can it change position?
				if (WorldGen.genRand.NextFloat() < curvyness)
				{
					currentX += WorldGen.genRand.Next(-3, 4);

					//Check that isn't too close to the limit left
					if(currentX - tunnelWidth < -area)
					{
						currentX = -area + tunnelWidth + 1;
					}

					//Check that isn't too close to the limit right
					if(currentX + tunnelWidth > area)
					{
						currentX = area - tunnelWidth - 1;
					}
				}

				//Excavate
				for (int i = -tunnelWidth; i <= tunnelWidth; i++)
				{
					WorldGen.KillTile(x + currentX + i, y + j);
				}
			}

			//Clean up
			for (int l = 0; l < 10; l++)
            {
                for (int i = x - area; i <= x + area; i++)
                {
                    for(int j = y - 5; j <= (int)(Main.worldSurface + 45); j++)
                    {
                        int tileCount = WorldgenTools.CheckTiles(i, j);

                        if (tileCount > 4)
                        {
							int tileType = TilesAround(i, j);

                            WorldGen.PlaceTile(i, j, tileType, true);
                        }
                        else if (tileCount < 4)
                        {
                            WorldGen.KillTile(i, j);
                        }
                    }
                }
            }
		}

		//Generate lake
		public static void GenerateLake(int x, int width, int range)
		{
			int posX = x + WorldGen.genRand.Next(-range, range + 1);
			int posY = FindGround(posX);
			posY -= (int)(width / 3);

			//Clear tiles
			WorldUtils.Gen(new Point(posX, posY), new Shapes.Circle(width, width), Actions.Chain(new GenAction[]
            {
                new Modifiers.Blotches(2, 0.4),
                new Actions.ClearTile(true),
            }));

			//Place water
			WorldUtils.Gen(new Point(posX, posY), new Shapes.Circle(width + 1, width + 1), Actions.Chain(new GenAction[]
            {
                new Actions.SetLiquid(LiquidID.Water, 100),
            }));

			//Clean up
			for (int l = 0; l < 10; l++)
            {
                for (int i = posX - width - 5; i <= posX + width + 5; i++)
                {
                    for(int j = posY; j <= posY + width + 5; j++)
                    {
                        Tile tile = Main.tile[i, j];

						if (!tile.HasTile && WorldGen.SolidTile(i, j - 1))
						{
							WorldGen.PlaceTile(i, j, ModContent.TileType<PreservedDirt>(), true);
						}
                    }
                }
            }
		}

		//Generate structure
		public static void GenerateStructure(int x, string structureFile, int range, int width, int offsetY)
		{
			int posX = x + WorldGen.genRand.Next(-range, range + 1);
			int posY = FindGround(posX);
			posY--;

			Vector2 origin = new Vector2(posX - (width / 2), posY - offsetY);
			StructureHelper.API.Generator.GenerateStructure("Generation/Structures/AncientSwamps/" + structureFile + ".shstruct", origin.ToPoint16(), ABMod.Instance);

			structuresPosX.Add(posX);
		}

		//Get the structure offset
		public static int GetStructureOffset(string structureFile)
		{
			if (structureFile == "SwampStruct1" || structureFile == "SwampStruct6")
			{
				return 5;
			}
			else if (structureFile == "SwampStruct2")
			{
				return 15;
			}
			else if (structureFile == "SwampStruct3" || structureFile == "SwampStruct5")
			{
				return 3;
			}
			else
			{
				return 10;
			}
		}

		//Get the structure width
		public static int GetStructureWidth(string structureFile)
		{
			if (structureFile == "SwampStruct1" || structureFile == "SwampStruct5")
			{
				return 26;
			}
			else if (structureFile == "SwampStruct2")
			{
				return 23;
			}
			else if (structureFile == "SwampStruct3")
			{
				return 35;
			}
			else if (structureFile == "SwampStruct4")
			{
				return 34;
			}
			else
			{
				return 29;
			}
		}

		//Find ground
		public static int FindGround(int x)
		{
			int y = 0;

			//Search values
			bool foundGround = false;
			int attemptsLeft = 0;

			//Find values
			while (!foundGround && attemptsLeft++ < 100000)
			{
				if (!BiomeTile.IsSwampTile(x, y) || !WorldgenTools.NoFloatingIslands(x, y, 45) && y < Main.maxTilesY)
				{
					y++;
				}

				if ((WorldGen.SolidTile(x, y) || Main.tile[x, y].WallType > WallID.None) && WorldgenTools.NoFloatingIslands(x, y, 45))
				{
					foundGround = true;
				}
			}

			return y;
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			//Add the biome in the worldgen task
			int BiomesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Dirt Rock Wall Runner"));
			if (BiomesIndex != -1)
			{
				tasks.Insert(BiomesIndex + 1, new PassLegacy("Ancient Swamp", SwampGen));
				tasks.Insert(BiomesIndex + 2, new PassLegacy("Flattening", SwampFlattening));
				tasks.Insert(BiomesIndex + 3, new PassLegacy("Swamp Cave", SwampCaves));
				tasks.Insert(BiomesIndex + 4, new PassLegacy("Swamp Ambience", SwampAmbience));
			}
		}
	}
}