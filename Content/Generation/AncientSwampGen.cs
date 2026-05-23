using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.IO;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.Localization;

using ABMod.Common;
using ABMod.Common.Tiles;
using ABMod.Content.Tiles.Swamp;

namespace ABMod.Content.Generation
{
    public class AncientSwampGen
    {
        //Generation values
		static int PlaceSwampX;
		static readonly int PlaceSwampY = (int)(Main.maxTilesY * 0.15f);

		static readonly int BiomeWidth = Main.maxTilesX >= 8400 ? 355 : (Main.maxTilesX >= 6400 ? 250 : 175);
        static readonly int BiomeHeightLimit = (int)(Main.maxTilesY / 1.75f);

        //Main method : Generate the base terrain
        public static void SwampGen(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = Language.GetOrRegister("Mods.ABMod.WorldgenTasks.Swamp1").Value;

            //X & Y value of the swamp origin, make sure X is on the jungle side
			bool foundValidPosition = false;
			int temporalSpawnX;

			if (GenVars.dungeonSide == -1)
			{
				//Time to search a valid position
				temporalSpawnX = Main.maxTilesX - (Main.maxTilesX / 8);

				if (!foundValidPosition && temporalSpawnX > ((Main.maxTilesX / 2) + BiomeWidth + 200))
				{
					if (!CanBePlaced(temporalSpawnX, (int)Main.worldSurface))
					{
						temporalSpawnX -= 10;
					}
					else
					{
						foundValidPosition = true;
					}
				}

				if(foundValidPosition)
				{
					PlaceSwampX = temporalSpawnX;
				}
				else
				{
					PlaceSwampX = Main.maxTilesX - (Main.maxTilesX / 8);
				}
			}
			else
			{
				//Time to search a valid position
				temporalSpawnX = Main.maxTilesX / 8;

				if (!foundValidPosition && temporalSpawnX < ((Main.maxTilesX / 2) - BiomeWidth - 200))
				{
					if (!CanBePlaced(temporalSpawnX, (int)Main.worldSurface))
					{
						temporalSpawnX += 10;
					}
					else
					{
						foundValidPosition = true;
					}
				}

				if(foundValidPosition)
				{
					PlaceSwampX = temporalSpawnX;
				}
				else
				{
					PlaceSwampX = Main.maxTilesX / 8;
				}
			}

			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth;

            //Place the biome base with a tile replacement loop
			int tileType;
			int wallType;

            for (int x = StartX; x <= EndX; x++)
            {
				progress.Set((float)(x - StartX) / (EndX - StartX));
                
				for (int y = PlaceSwampY; y <= BiomeHeightLimit; y++)
                {
                    tileType = CheckTileOrWall(y);
					wallType = CheckTileOrWall(y, true);

                    if (WorldGenTools.NoFloatingIslands(x, y) && !BiomeTile.IsFloatingIslandTile(x, y))
                    {
                        Tile tile = Framing.GetTileSafely(x, y);

                        //Replace tiles
						if (tile.HasTile)
						{
							//Replace if solid, clear if not
							if (WorldGen.SolidTile(x, y))
							{
								tile.TileType = (ushort)tileType;
							}
							else
							{
								Main.tile[x, y].ClearTile();
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
						if(tile.WallType == WallID.None && y > (int)Main.worldSurface)
                        {
                            WorldGen.PlaceWall(x, y, wallType, true);
                        }
                    }
                }
            }

            //Fixed base and hole fill
			for (int x = StartX; x <= EndX; x++)
			{
				for (int y = (int)Main.worldSurface - 70; y <= BiomeHeightLimit; y++)
				{
					Tile tile = Main.tile[x, y];

					if (!tile.HasTile)
					{
						tileType = CheckTileOrWall(y);
						wallType = CheckTileOrWall(y, true);

						WorldGen.PlaceTile(x, y,tileType, true);
						WorldGen.PlaceWall(x, y, wallType, true);
					}
				}
			}

			//Dithering
			for (int x = StartX - 10; x <= EndX + 10; x++)
			{
				for (int y = PlaceSwampY; y <= BiomeHeightLimit + 10; y++)
				{
					//If outside of the main area, add dither
					if (x < StartX || x > EndX || y > BiomeHeightLimit)
					{
						tileType = CheckTileOrWall(y);
						wallType = CheckTileOrWall(y, true);

						if (WorldGenTools.NoFloatingIslands(x, y) && !BiomeTile.IsFloatingIslandTile(x, y))
						{
							Tile tile = Framing.GetTileSafely(x, y);

							bool replaceBool = WorldGen.genRand.NextBool();
							bool placeBool = WorldGen.genRand.NextBool();

							//Replace tiles
							if (tile.HasTile)
							{
								if (WorldGen.SolidTile(x, y) && replaceBool)
								{
									tile.TileType = (ushort)tileType;
								}
							}

							//Replace walls
							if (tile.WallType > WallID.None && replaceBool)
							{
								tile.WallType = (ushort)wallType;
							}

							//Place walls
							if(tile.WallType == WallID.None && y > (int)Main.worldSurface && placeBool)
							{
								WorldGen.PlaceWall(x, y, wallType, true);
							}
						}
					}
				}
			}
        }

		//Main method : Flatten the terrain and place soil tiles
		public static void SwampFlattening(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Language.GetOrRegister("Mods.ABMod.WorldgenTasks.Swamp2").Value;

			//Beginning and end of the biome
			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth + 1;

			//Ground points
			int LeftY = FindGround(StartX);
			int RightY = FindGround(EndX);

			ConnectPoints(new Vector2(StartX, LeftY), new Vector2(EndX, RightY));
		}

        //Helper method : Return a tile / wall based on deepness
		private static int CheckTileOrWall(int Y, bool returnWall = false)
		{
			if (Y >= (int)Main.worldSurface + 30)
			{
				return returnWall ? ModContent.WallType<SwampStoneWall>() : ModContent.TileType<SwampStone>();
			}
			else if (Y >= (int)Main.worldSurface + 40)
			{
				if (WorldGen.genRand.NextBool())
				{
					return returnWall ? ModContent.WallType<SwampStoneWall>() :  ModContent.TileType<SwampStone>();
				}
				else
				{
					return returnWall ? ModContent.WallType<SwampDirtWall>() :  ModContent.TileType<SwampDirt>();
				}
			}
			else
			{
				return returnWall ? ModContent.WallType<SwampDirtWall>() :  ModContent.TileType<SwampDirt>();
			}
		}

		//Helper method : Connect 2 points with a bezier curve
		public static void ConnectPoints(Vector2 p0, Vector2 p1)
		{
			//HashSet to save the X coordinates we already visited
			HashSet <int> visitedX = [];

			//Values for the perlin noise
			int seed = WorldGen.genRand.Next();
			int height = 9;
			int perlinHeight;

			int segments = 10000;

			for (int i = 0; i < segments; i++)
			{
				float t = i / (float)segments;
				Vector2 Position = BezierCurve.LinearBezier(t, p0, p1);

				int posX = (int)Position.X;
				int posY = (int)Position.Y;

				if (visitedX.Contains(posX))
					continue;
				
				//Noise moment
				float dx = MathF.Abs(posX - PlaceSwampX);
				float normalized = dx / BiomeWidth;

				if (normalized > 1f)
					continue;
				
				float topMask = MathF.Sqrt(1f - MathF.Pow(normalized, 2f));
				float topNoise = WorldGenTools.Perlin(posX * 0.04f, seed, 3, 0.4f);
				perlinHeight = (int)(topMask * height * topNoise);

				//Place tiles
				for(int y = posY - perlinHeight; y <= Main.worldSurface; y++)
				{
					Tile tile = Framing.GetTileSafely(posX, y);

					if (!tile.HasTile)
					{
						WorldGen.PlaceTile(posX, y, ModContent.TileType<SwampDirt>(), true);
						WorldGen.PlaceWall(posX, y, ModContent.WallType<SwampDirtWall>(), true);
					}
				}

				//Clear tiles above the line
				int heightLimit = (int)(Main.worldSurface * 0.35f);

				for (int y = heightLimit; y < posY - perlinHeight; y++)
				{
					if (BiomeTile.IsSwampTile(posX, y) || Main.tile[posX, y].WallType == ModContent.WallType<SwampDirtWall>())
					{
						Framing.GetTileSafely(posX, y).ClearEverything();
					}
				}
			}
		}

        //Helper method : Tile check for jungle and desert
		public static bool CanBePlaced(int i, int j)
		{
			int jungleTiles = 0;

			for(int x = i - 50; x <= i + 50; x++)
			{
				for(int y = j - 50; y <= j + 50; y++)
				{
					if (WorldGen.InWorld(i, j) && Main.tile[i, j].HasTile && BiomeTile.IsJungleTile(x, y))
					{
						jungleTiles++;
					}
				}
			}

			int desertTiles = 0;

			for(int x = i - 200; x <= i + 200; x++)
			{
				for(int y = j - 50; y <= j + 50; y++)
				{
					if (WorldGen.InWorld(i, j) && Main.tile[i, j].HasTile && BiomeTile.IsDesertTile(x, y))
					{
						desertTiles++;
					}
				}
			}

			if (jungleTiles > 500 || desertTiles > 500)
			{
				return false;
			}

			return true;
		}

		//Helper method : Find ground
		private static int FindGround(int x)
		{
			int y = PlaceSwampY;

			bool foundGround = false;
			int attemptsLeft = 0;

			while (!foundGround && attemptsLeft++ < 100000)
			{
				if (!BiomeTile.IsSwampTile(x, y) || BiomeTile.IsFloatingIslandTile(x, y) || !WorldGenTools.NoFloatingIslands(x, y) && y < Main.maxTilesY)
				{
					y++;
				}

				if ((WorldGen.SolidTile(x, y) || Main.tile[x, y].WallType > WallID.None) && WorldGenTools.NoFloatingIslands(x, y) && !BiomeTile.IsFloatingIslandTile(x, y))
				{
					foundGround = true;
				}
			}
			
			return y;
		}
    }
}