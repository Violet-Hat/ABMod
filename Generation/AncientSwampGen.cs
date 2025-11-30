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

			//CLEAR THE TILES
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
			BiomeWidth = Main.maxTilesX >= 8400 ? 450 : Main.maxTilesX >= 6400 ? 300 : 150;
			BiomeHeightLimit = (int)(Main.maxTilesY / 1.75f);

			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth;

			//Place the biome base
			int progressFifth = (int)(BiomeWidth * 2 / 5);

			for (int X = StartX - 10; X < EndX + 10; X++)
			{
				//Progress setters
				if (X == StartX + progressFifth) progress.Set(0.2);
				if (X == StartX + (progressFifth * 2)) progress.Set(0.4);
				if (X == StartX + (progressFifth * 3)) progress.Set(0.6);
				if (X == StartX + (progressFifth * 4)) progress.Set(0.8);

				//Tile replacement loop
				for (int Y = PlaceSwampY; Y < BiomeHeightLimit + 10; Y++)
				{
					if (WorldgenTools.NoFloatingIslands(X, Y, 45))
					{
						Tile tile = Main.tile[X, Y];

						//Replace tiles
						if (tile.HasTile && tile.TileType != TileID.Cloud && tile.TileType != TileID.RainCloud && !IsSwampTile(X, Y))
						{
							if (X < StartX || X > EndX || Y > BiomeHeightLimit)
							{
								//Replace with a 50/50 chance
								if (WorldGen.SolidTile(X, Y) && WorldGen.genRand.NextBool())
								{
									ReplaceTile(X, Y);
								}
							}
							else
							{
								//Clear if not solid
								if (WorldGen.SolidTile(X, Y))
								{
									ReplaceTile(X, Y);
								}
								else
								{
									Main.tile[X, Y].ClearTile();
								}
							}
						}

						//Place tiles on ebonstone and crimstone walls because THEY SUCK
						if (X >= StartX && X <= EndX && Y <= BiomeHeightLimit)
						{
							if (!tile.HasTile && (tile.WallType == WallID.EbonstoneUnsafe || tile.WallType == WallID.CrimstoneUnsafe))
							{
								if (Y >= (int)Main.worldSurface + 40)
								{
									tile.TileType = (ushort)ModContent.TileType<AncientStone>();
								}
								else if (Y >= (int)Main.worldSurface + 35)
								{
									if (WorldGen.genRand.NextBool())
									{
										tile.TileType = (ushort)ModContent.TileType<AncientStone>();
									}
									else
									{
										tile.TileType = (ushort)ModContent.TileType<AncientDirt>();
									}
								}
								else
								{
									tile.TileType = (ushort)ModContent.TileType<AncientDirt>();
								}
							}
						}

						//Replace walls
						if (tile.WallType > WallID.None)
						{
							if (X < StartX || X > EndX || Y > BiomeHeightLimit)
							{
								//Replace wall with a 50/50 chance
								if (WorldGen.genRand.NextBool())
								{
									ReplaceWall(X, Y);
								}
							}
							else
							{
								ReplaceWall(X, Y);
							}
						}

						//Place walls
						if(tile.WallType == WallID.None && Y > (int)Main.worldSurface)
                        {
                            if (WorldGen.genRand.NextBool())
							{
								PlaceWall(X, Y);
							}
                        }
					}
				}

				//Fixed base to avoid making it too low
				if (X >= StartX && X <= EndX)
				{
					for (int Y = (int)Main.worldSurface - 70; Y < (int)Main.worldSurface; Y++)
					{
						Tile tile = Main.tile[X, Y];

						tile.TileType = (ushort)ModContent.TileType<AncientDirt>();
						WorldGen.PlaceTile(X, Y, ModContent.TileType<AncientDirt>());

						tile.WallType = (ushort)ModContent.WallType<AncientDirtWall>();
						WorldGen.PlaceWall(X, Y, ModContent.WallType<AncientDirtWall>());
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
			float smoothPow = 0.2f;

			int CaveWidth = BiomeWidth + 20;

			//Upper caves
			int PlaceCaveY = (int)((Main.worldSurface - 15 + Main.worldSurface + 35) / 2);
			int CaveHeight = (int)(Math.Abs(Main.worldSurface + 35 - Main.worldSurface - 15) / 2);

			for (int X = PlaceSwampX - CaveWidth; X <= PlaceSwampX + CaveWidth; X++)
			{
				for (int Y = PlaceCaveY - CaveHeight; Y < PlaceCaveY + CaveHeight; Y++)
				{
					//Perlin noise values
					//These are for transition purposes
					float horizontalOffsetNoise = WorldgenTools.PerlinNoise2D(X / 225f, Y / 225f, octaves, unchecked(seed + 1)) * 0.01f;
					float cavePerlinValue = WorldgenTools.PerlinNoise2D(X / 550f, Y / 300f, octaves, seed) + 0.5f + horizontalOffsetNoise;
					float cavePerlinValue2 = WorldgenTools.PerlinNoise2D(X / 550f, Y / 300f, octaves, unchecked(seed - 1)) + 0.5f;
					float caveNoiseMap = (cavePerlinValue + cavePerlinValue2) * 0.5f;
					float caveCreationThreshold = horizontalOffsetNoise * 3.5f + 0.235f;

					//Smooth X and Y value
					float distanceX = Math.Abs(X - PlaceSwampX);
					float smoothValueX = 1f - (distanceX / CaveWidth);
					float smoothX = (float)Math.Pow(Math.Clamp(smoothValueX, 0f, 1f), smoothPow);

					float distanceY = Math.Abs(Y - PlaceCaveY);
					float smoothValueY = 1f - (distanceY / CaveHeight);
					float smoothY = (float)Math.Pow(Math.Clamp(smoothValueY, 0f, 1f), smoothPow);

					//Smooth the noise
					float smooth = smoothX * smoothY;
					float smoothNoiseMap = caveNoiseMap * smooth;
					float smoothCreationThreshold = caveCreationThreshold * smooth;

					//Remove tiles based on the noise
					if (smoothNoiseMap * smoothNoiseMap > smoothCreationThreshold)
					{
						WorldGen.KillTile(X, Y);
					}
				}
			}

			PlaceCaveY = (int)((Main.worldSurface + 20 + Main.rockLayer + 15) / 2);
			CaveHeight = (int)(Math.Abs(Main.rockLayer + 15 - Main.worldSurface + 20) / 2);

			for (int X = PlaceSwampX - CaveWidth; X <= PlaceSwampX + CaveWidth; X++)
			{
				for (int Y = PlaceCaveY - CaveHeight; Y < PlaceCaveY + CaveHeight; Y++)
				{
					//Perlin noise values
					//Higher Y values for more vertical caves
					float horizontalOffsetNoise = WorldgenTools.PerlinNoise2D(X / 225f, Y / 225f, octaves, unchecked(seed + 1)) * 0.01f;
					float cavePerlinValue = WorldgenTools.PerlinNoise2D(X / 350f, Y / 500f, octaves, seed) + 0.5f + horizontalOffsetNoise;
					float cavePerlinValue2 = WorldgenTools.PerlinNoise2D(X / 350f, Y / 500f, octaves, unchecked(seed - 1)) + 0.5f;
					float caveNoiseMap = (cavePerlinValue + cavePerlinValue2) * 0.5f;
					float caveCreationThreshold = horizontalOffsetNoise * 3.5f + 0.235f;

					//Smooth X and Y value
					float distanceX = Math.Abs(X - PlaceSwampX);
					float smoothValueX = 1f - (distanceX / CaveWidth);
					float smoothX = (float)Math.Pow(Math.Clamp(smoothValueX, 0f, 1f), smoothPow);

					float distanceY = Math.Abs(Y - PlaceCaveY);
					float smoothValueY = 1f - (distanceY / CaveHeight);
					float smoothY = (float)Math.Pow(Math.Clamp(smoothValueY, 0f, 1f), smoothPow);

					//Smooth the noise
					float smooth = smoothX * smoothY;
					float smoothNoiseMap = caveNoiseMap * smooth;
					float smoothCreationThreshold = caveCreationThreshold * smooth;

					//Remove tiles based on the noise
					if (smoothNoiseMap * smoothNoiseMap > smoothCreationThreshold)
					{
						WorldGen.KillTile(X, Y);
					}
				}
			}

			progress.Set(0.25);

			//Lower caves
			PlaceCaveY = (int)((Main.rockLayer + BiomeHeightLimit) / 2);
			CaveHeight = (int)(Math.Abs(BiomeHeightLimit + 20 - Main.rockLayer) / 2);

			for (int X = PlaceSwampX - CaveWidth; X <= PlaceSwampX + CaveWidth; X++)
			{
				for (int Y = PlaceCaveY - CaveHeight; Y <= PlaceCaveY + CaveHeight; Y++)
				{
					//Perlin noise values
					//Higher X values for more horizontal caves
					float horizontalOffsetNoise = WorldgenTools.PerlinNoise2D(X / 225f, Y / 225f, octaves, unchecked(seed + 1)) * 0.01f;
					float cavePerlinValue = WorldgenTools.PerlinNoise2D(X / 550f, Y / 300f, octaves, seed) + 0.5f + horizontalOffsetNoise;
					float cavePerlinValue2 = WorldgenTools.PerlinNoise2D(X / 550f, Y / 300f, octaves, unchecked(seed - 1)) + 0.5f;
					float caveNoiseMap = (cavePerlinValue + cavePerlinValue2) * 0.5f;
					float caveCreationThreshold = horizontalOffsetNoise * 3.5f + 0.235f;

					//Smooth X and Y value
					float distanceX = Math.Abs(X - PlaceSwampX);
					float smoothValueX = 1f - (distanceX / CaveWidth);
					float smoothX = (float)Math.Pow(Math.Clamp(smoothValueX, 0f, 1f), smoothPow);

					float distanceY = Math.Abs(Y - PlaceCaveY);
					float smoothValueY = 1f - (distanceY / CaveHeight);
					float smoothY = (float)Math.Pow(Math.Clamp(smoothValueY, 0f, 1f), smoothPow);

					//Smooth the noise
					float smooth = smoothX * smoothY;
					float smoothNoiseMap = caveNoiseMap * smooth;
					float smoothCreationThreshold = caveCreationThreshold * smooth;

					//Remove tiles based on the noise
					if (smoothNoiseMap * smoothNoiseMap > smoothCreationThreshold)
					{
						WorldGen.KillTile(X, Y);
					}
				}
			}

			progress.Set(0.5);

			/*
			//Walls
			PlaceCaveY = (int)((Main.worldSurface + 5 + BiomeHeightLimit) / 2);
			CaveHeight = (int)(Math.Abs(BiomeHeightLimit - Main.worldSurface + 5) / 2);

			for (int X = PlaceSwampX - BiomeWidth; X <= PlaceSwampX + BiomeWidth; X++)
			{
				for (int Y = (int)Main.worldSurface + 5; Y <= BiomeHeightLimit; Y++)
				{
					//Perlin noise values
					float horizontalOffsetNoise = WorldgenTools.PerlinNoise2D(X / 50f, Y / 50f, octaves, unchecked(seed + 1)) * 0.01f;
					float cavePerlinValue = WorldgenTools.PerlinNoise2D(X / 750f, Y / 350f, octaves, seed) + 0.5f + horizontalOffsetNoise;
					float cavePerlinValue2 = WorldgenTools.PerlinNoise2D(X / 750f, Y / 350f, octaves, unchecked(seed - 1)) + 0.5f;
					float caveNoiseMap = (cavePerlinValue + cavePerlinValue2) * 0.5f;
					float caveCreationThreshold = horizontalOffsetNoise * 3.5f + 0.235f;

					//Smooth X and Y value
					float distanceX = Math.Abs(X - PlaceSwampX);
					float smoothValueX = 1f - (distanceX / BiomeWidth);
					float smoothX = (float)Math.Pow(Math.Clamp(smoothValueX, 0f, 1f), 0.15f);

					float distanceY = Math.Abs(Y - PlaceCaveY);
					float smoothValueY = 1f - (distanceY / CaveHeight);
					float smoothY = (float)Math.Pow(Math.Clamp(smoothValueY, 0f, 1f), 0.15f);

					//Smooth the noise
					float smooth = smoothX * smoothY;
					float smoothNoiseMap = caveNoiseMap * smooth;
					float smoothCreationThreshold = caveCreationThreshold * smooth;

					//Remove tiles based on the noise
					if (smoothNoiseMap * smoothNoiseMap > smoothCreationThreshold)
					{
						WorldGen.KillWall(X, Y);
					}
				}
			}
			*/
		}
		
		private void SwampAmbience(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Adding a nice ambience";

			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth;

			for(int X = StartX; X <= EndX; X++)
			{
				for(int Y = PlaceSwampY; Y <= BiomeHeightLimit; Y++)
				{
					Tile tile = Main.tile[X, Y];
					Tile above = Main.tile[X, Y - 1];
					Tile below = Main.tile[X, Y + 1];
					Tile left = Main.tile[X - 1, Y];
					Tile right = Main.tile[X + 1, Y];

					WorldGen.SpreadGrass(X, Y, ModContent.TileType<PreservedDirt>(), ModContent.TileType<PrehistoricMoss>());
				}
			}
			/*
			for (int X = i - radiusX + 1; X < i + radiusX; X++)
			{
				for (int Y = j; Y < limitY; Y++)
				{
					if (Main.tile[X, Y].TileType == SwampMoss && Main.tile[X + 1, Y].TileType == SwampMoss)
					{
						if (WorldGen.genRand.NextBool(6))
						{
							if (WorldgenTools.GrowTreeCheck(X, Y, 6, 35, ModContent.TileType<Lep_Trunk>(), 1))
							{
								Lep_Trunk.Grow(X, Y - 1, 25, 30, false);
							}
						}
					}
				}
			}
			*/
		}

		//Check if the tile is a swamptile
		public static bool IsSwampTile(int X, int Y)
		{
			return Main.tile[X, Y].TileType == (ushort)ModContent.TileType<PreservedDirt>() ||
			Main.tile[X, Y].TileType == (ushort)ModContent.TileType<AncientDirt>() ||
			Main.tile[X, Y].TileType == (ushort)ModContent.TileType<AncientStone>() ||
			Main.tile[X, Y].TileType == (ushort)ModContent.TileType<PrehistoricMoss>();
		}

		//Replace a tile
		public static void ReplaceTile(int X, int Y)
		{
			Tile tile = Main.tile[X, Y];

			if (Y >= (int)Main.worldSurface + 40)
			{
				tile.TileType = (ushort)ModContent.TileType<AncientStone>();
				WorldGen.PlaceWall(X, Y, ModContent.WallType<AncientStoneWall>());
			}
			else if (Y >= (int)Main.worldSurface + 30)
			{
				if (WorldGen.genRand.NextBool())
				{
					tile.TileType = (ushort)ModContent.TileType<AncientStone>();
					WorldGen.PlaceWall(X, Y, ModContent.WallType<AncientStoneWall>());
				}
				else
				{
					tile.TileType = (ushort)ModContent.TileType<AncientDirt>();
					WorldGen.PlaceWall(X, Y, ModContent.WallType<AncientDirtWall>());
				}
			}
			else
			{
				tile.TileType = (ushort)ModContent.TileType<AncientDirt>();
				WorldGen.PlaceWall(X, Y, ModContent.WallType<AncientDirtWall>());
			}
		}

		//Replace a wall
		public static void ReplaceWall(int X, int Y)
		{
			Tile tile = Main.tile[X, Y];

			if (Y >= (int)Main.worldSurface + 40)
			{
				tile.WallType = (ushort)ModContent.WallType<AncientStoneWall>();
			}
			else if (Y >= (int)Main.worldSurface + 30)
			{
				if (WorldGen.genRand.NextBool())
				{
					tile.WallType = (ushort)ModContent.WallType<AncientStoneWall>();
				}
				else
				{
					tile.WallType = (ushort)ModContent.WallType<AncientDirtWall>();
				}
			}
			else
			{
				tile.WallType = (ushort)ModContent.WallType<AncientDirtWall>();
			}
		}

		//Place a wall
		public static void PlaceWall(int X, int Y)
		{
			Tile tile = Main.tile[X, Y];

			if (Y >= (int)Main.worldSurface + 40)
			{
				WorldGen.PlaceWall(X, Y, ModContent.WallType<AncientStoneWall>());
			}
			else if (Y >= (int)Main.worldSurface + 30)
			{
				if (WorldGen.genRand.NextBool())
				{
					WorldGen.PlaceWall(X, Y, ModContent.WallType<AncientStoneWall>());
				}
				else
				{
					WorldGen.PlaceWall(X, Y, ModContent.WallType<AncientDirtWall>());
				}
			}
			else
			{
				WorldGen.PlaceWall(X, Y, ModContent.WallType<AncientDirtWall>());
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
				for (int Y = (int)Position.Y; Y <= BiomeHeightLimit; Y++)
				{
					Tile tile = Main.tile[(int)Position.X, Y];

					if (!tile.HasTile)
					{
						if (Y >= (int)Main.worldSurface + 40)
						{
							WorldGen.PlaceTile((int)Position.X, Y, ModContent.TileType<AncientStone>());
							WorldGen.PlaceWall((int)Position.X, Y, ModContent.WallType<AncientStoneWall>());
						}
						else if (Y >= (int)Main.worldSurface + 35)
						{
							if (WorldGen.genRand.NextBool())
							{
								WorldGen.PlaceTile((int)Position.X, Y, ModContent.TileType<AncientStone>());
								WorldGen.PlaceWall((int)Position.X, Y, ModContent.WallType<AncientStoneWall>());
							}
							else
							{
								WorldGen.PlaceTile((int)Position.X, Y, ModContent.TileType<AncientDirt>());
								WorldGen.PlaceWall((int)Position.X, Y, ModContent.WallType<AncientDirtWall>());
							}
						}
						else
						{
							WorldGen.PlaceTile((int)Position.X, Y, ModContent.TileType<AncientDirt>());
							WorldGen.PlaceWall((int)Position.X, Y, ModContent.WallType<AncientDirtWall>());
						}
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
							WorldGen.PlaceTile(point.X, point.Y + j, ModContent.TileType<PreservedDirt>());

							tile.WallType = (ushort)ModContent.WallType<PreservedDirtWall>();
							WorldGen.PlaceWall(point.X, point.Y + j, ModContent.WallType<PreservedDirtWall>());
						}
					}
					else
					{
						tile.TileType = (ushort)ModContent.TileType<PreservedDirt>();
						WorldGen.PlaceTile(point.X, point.Y + j, ModContent.TileType<PreservedDirt>());

						tile.WallType = (ushort)ModContent.WallType<PreservedDirtWall>();
						WorldGen.PlaceWall(point.X, point.Y + j, ModContent.WallType<PreservedDirtWall>());
					}
				}

				if (Main.tile[point.X, point.Y].WallType > WallID.None)
				{
					WorldGen.KillWall(point.X, point.Y);
				}

				if(WorldGen.genRand.NextBool() && Main.tile[point.X + 1, point.Y].HasTile)
                {
					WorldGen.PlaceTile(point.X, point.Y - 1, ModContent.TileType<PreservedDirt>());
					WorldGen.PlaceTile(point.X + 1, point.Y - 1, ModContent.TileType<PreservedDirt>());
                }
			}
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