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
using ABMod.Tiles.AncientSwampBiome;
using ABMod.Tiles.AncientSwampBiome.Trees;
using ABMod.Tiles.AncientSwampBiome.Ambient;

namespace ABMod.Generation
{
	public class AncientSwampGen : ModSystem
	{
		//Generation values
		public static int PlaceSwampX = 0;
		public static int PlaceSwampY = 0;
		public static int BiomeWidth = 0;
        public static int BiomeHeightLimit = 0;

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

			for (int X = StartX; X < EndX; X++)
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
						if (tile.HasTile && tile.TileType != TileID.Cloud && tile.TileType != TileID.RainCloud && !IsSwampTile(X, Y))
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
							if (tile.HasTile && tile.TileType != TileID.Cloud && tile.TileType != TileID.RainCloud && !IsSwampTile(X, Y))
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

			for (int X = StartX - 10; X < EndX + 10; X++)
			{
				for (int Y = BiomeHeightLimit; Y <= BiomeHeightLimit + 10; Y++)
				{
					Tile tile = Main.tile[X, Y];

					//Replace tiles
					if (tile.HasTile && tile.TileType != TileID.Cloud && tile.TileType != TileID.RainCloud && !IsSwampTile(X, Y))
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
			int LeftY = 0;
			int RightY = 0;

			//Search values
			bool foundGroundLeft = false;
			int attemptsLeft = 0;

			//Find values
			while (!foundGroundLeft && attemptsLeft++ < 100000)
			{
				if (!IsSwampTile(StartX, LeftY) || !WorldgenTools.NoFloatingIslands(StartX, LeftY, 45) && LeftY < Main.maxTilesY)
				{
					LeftY++;
				}

				if ((WorldGen.SolidTile(StartX, LeftY) || Main.tile[StartX, LeftY].WallType > WallID.None) && WorldgenTools.NoFloatingIslands(StartX, LeftY, 45))
				{
					foundGroundLeft = true;
				}
			}

			bool foundGroundRight = false;
			int attemptsRight = 0;

			while (!foundGroundRight && attemptsRight++ < 100000)
			{
				if (!IsSwampTile(EndX, RightY) || !WorldgenTools.NoFloatingIslands(EndX, RightY, 45) && RightY < Main.maxTilesY)
				{
					RightY++;
				}

				if ((WorldGen.SolidTile(EndX, RightY) || Main.tile[EndX, RightY].WallType > WallID.None) && WorldgenTools.NoFloatingIslands(EndX, RightY, 45))
				{
					foundGroundRight = true;
				}
			}

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
				int X = WorldGen.genRand.Next(MiddleX - BiomeWidth, MiddleX + BiomeWidth);
				int Y = WorldGen.genRand.Next(heightLimit, (int)Main.worldSurface + 25);

				Tile tile = Main.tile[X, Y];

				if (tile.HasTile && tile.TileType == ModContent.TileType<AncientDirt>())
				{
					WorldGen.TileRunner(X, Y, WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(20, 30), ModContent.TileType<AncientStone>(), false, 0f, 0f, false, true);
					stoneCount--;
				}
			}
			
			int j = 0;
			int dirtCount = Main.maxTilesX >= 8400 ? 28 : Main.maxTilesX >= 6400 ? 21 : 9;
			dirtCount += WorldGen.genRand.Next(25, 45);

			while (dirtCount > 0 && j++ < 10000)
            {
				int X = WorldGen.genRand.Next(MiddleX - BiomeWidth, MiddleX + BiomeWidth);
				int Y = WorldGen.genRand.Next((int)Main.worldSurface + 50, BiomeHeightLimit - 50);

				Tile tile = Main.tile[X, Y];

				if(tile.HasTile && tile.TileType == ModContent.TileType<AncientStone>())
                {
					WorldGen.TileRunner(X, Y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(30, 50), ModContent.TileType<AncientDirt>(), false, 0f, 0f, false, true);
					dirtCount--;
                }
            }

			progress.Set(0.75);

			//Smooth time
			for (int X = MiddleX - BiomeWidth - 5; X <= MiddleX + BiomeWidth + 5; X++)
			{
				for (int Y = heightLimit; Y <= Main.worldSurface; Y++)
				{
					if (IsSwampTile(X, Y))
					{
						Tile.SmoothSlope(X, Y);
					}
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

			//Hell
			List <int> values = new List<int>();

			//Start and end
			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth;

			//Structure values
			bool placedTunnel = false;
			bool placedLake = false;

			int numSurfaceStructures = Main.maxTilesX >= 8400 ? 6 : Main.maxTilesX >= 6400 ? 5 : 4;
			int structurePartition = BiomeWidth * 2 / (numSurfaceStructures + 1);

			for (int i = 0; i < numSurfaceStructures; i++)
			{
				values.Add(i + 1);
			}

			if (!placedTunnel)
			{
				int placeTunnel = 0;
				int rando = 0;

				if (GenVars.dungeonSide == -1)
				{
					placeTunnel = 1;
					rando = -structurePartition / 3;
				}
				else
				{
					placeTunnel = numSurfaceStructures;
					rando = structurePartition / 3;
				}

				//Remove this position from the list
				values.Remove(placeTunnel);

				GenerateTunnel(StartX + (structurePartition * placeTunnel) + rando);
			}

			//GRASS
			for(int X = StartX; X <= EndX; X++)
			{
				for(int Y = PlaceSwampY; Y <= Main.worldSurface; Y++)
                {
                    WorldGen.SpreadGrass(X, Y, ModContent.TileType<PreservedDirt>(), ModContent.TileType<PrehistoricMoss>());
                }
			}

			for(int X = StartX; X <= EndX; X++)
			{
				for(int Y = PlaceSwampY; Y <= BiomeHeightLimit; Y++)
				{
					Tile tile = Main.tile[X, Y];
					Tile tileRight = Main.tile[X + 1, Y];
					Tile tileAbove = Main.tile[X, Y - 1];

					if(tile.TileType == (ushort)ModContent.TileType<PrehistoricMoss>())
					{
						//Tree
						if(tileRight.TileType == (ushort)ModContent.TileType<PrehistoricMoss>() && !IsSloped(X, Y) && !IsSloped(X + 1, Y))
						{
							if (WorldGen.genRand.NextBool(6))
							{
								if (WorldgenTools.GrowTreeCheck(X, Y, 6, 35, ModContent.TileType<Lep>(), 1))
								{
									Lep.Grow(X, Y - 1, 25, 30, false);
								}
							}
						}

						if(!IsSloped(X, Y))
						{
							if (WorldGen.genRand.NextBool(6))
							{
								if (WorldgenTools.GrowTreeCheck(X, Y, 5, 25, ModContent.TileType<Skinny>()))
								{
									Skinny.Grow(X, Y - 1, 15, 20, false);
								}
							}
						}

						//Ambient decorations
						if (WorldGen.genRand.NextBool(5) && tileAbove.TileType != (ushort)ModContent.TileType<Lep>())
                        {
                            switch (WorldGen.genRand.Next(3))
                            {
								default: 
									WorldGen.PlaceObject(X, Y - 1, ModContent.TileType<LargeAmbientPlant>(), true, WorldGen.genRand.Next(9));
									break;
                                case 1:
									WorldGen.PlaceObject(X, Y - 1, ModContent.TileType<LargePrehistoricMossRock>(), true, WorldGen.genRand.Next(3));
									break;
								case 2:
									WorldGen.PlaceObject(X, Y - 1, ModContent.TileType<MediumAmbientPlant>(), true, WorldGen.genRand.Next(12));
									break;
                            }
                        }

						//Grass
						if (WorldGen.genRand.NextBool(3) && tileAbove.TileType != (ushort)ModContent.TileType<Lep>())
                        {
                            WorldGen.PlaceObject(X, Y - 1, ModContent.TileType<SwampGrass>(), true, WorldGen.genRand.Next(28));
                        }
					}

					if(tile.TileType == (ushort)ModContent.TileType<AncientDirt>())
                    {
						if (WorldGen.genRand.NextBool(3))
						{
							WorldGen.PlaceObject(X, Y - 1, ModContent.TileType<SmallAncientDirtRock>(), true, WorldGen.genRand.Next(3));
						}
						
						if (WorldGen.genRand.NextBool(3))
						{
							WorldGen.PlaceObject(X, Y - 1, ModContent.TileType<Cooksonia>(), true, WorldGen.genRand.Next(6));
						}
                    }

					if(tile.TileType == (ushort)ModContent.TileType<AncientStone>())
                    {
                        //Ambient decorations
						if (WorldGen.genRand.NextBool(5))
                        {
                            switch (WorldGen.genRand.Next(2))
                            {
								default: 
									WorldGen.PlaceObject(X, Y - 1, ModContent.TileType<LargeAncientStoneRock>(), true, WorldGen.genRand.Next(3));
									break;
                                case 1:
									WorldGen.PlaceObject(X, Y - 1, ModContent.TileType<MediumAncientStoneRock>(), true, WorldGen.genRand.Next(3));
									break;
                            }
                        }

						if (WorldGen.genRand.NextBool(3))
                        {
                            WorldGen.PlaceObject(X, Y - 1, (ushort)ModContent.TileType<SmallAncientStoneRock>(), true, WorldGen.genRand.Next(3));
                        }
                    }
                }
			}
		}

		//Check if the tile is a swamptile
		public static bool IsSwampTile(int X, int Y)
		{
			return Main.tile[X, Y].TileType == (ushort)ModContent.TileType<PreservedDirt>() ||
			Main.tile[X, Y].TileType == (ushort)ModContent.TileType<AncientDirt>() ||
			Main.tile[X, Y].TileType == (ushort)ModContent.TileType<AncientStone>() ||
			Main.tile[X, Y].TileType == (ushort)ModContent.TileType<PrehistoricMoss>();
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
					if (IsSwampTile((int)Position.X, Y) || Main.tile[(int)Position.X, Y].WallType == ModContent.WallType<PreservedDirtWall>() || Main.tile[(int)Position.X, Y].WallType == ModContent.WallType<AncientDirtWall>())
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
			int y = 0;

			//Search values
			bool foundGround = false;
			int attemptsLeft = 0;

			//Find values
			while (!foundGround && attemptsLeft++ < 100000)
			{
				if (!IsSwampTile(x, y) || !WorldgenTools.NoFloatingIslands(x, y, 45) && y < Main.maxTilesY)
				{
					y++;
				}

				if ((WorldGen.SolidTile(x, y) || Main.tile[x, y].WallType > WallID.None) && WorldgenTools.NoFloatingIslands(x, y, 45))
				{
					foundGround = true;
				}
			}

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

		//Check if it's sloped
		public static bool IsSloped(int X, int Y)
        {
            return Main.tile[X, Y].LeftSlope ||
			Main.tile[X, Y].RightSlope ||
			Main.tile[X, Y].TopSlope ||
			Main.tile[X, Y].BottomSlope;
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