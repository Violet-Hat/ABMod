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

using ABMod.Common.Tiles;
using ABMod.Content.Tiles.Swamp;

namespace ABMod.Content.Generation
{
    public class AncientSwampGen
    {
        //Generation values
		private static int PlaceSwampX = 0;
		private static int PlaceSwampY = 0;

		private static int BiomeWidth = 0;
        private static int BiomeHeightLimit = 0;

        //Main method : Generate the terrain
        public static void SwampGen(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = Language.GetOrRegister("Mods.ABMod.WorldgenTasks.Swamp1").Value;

            //Width & height of the swamp
			BiomeWidth = Main.maxTilesX >= 8400 ? 355 : Main.maxTilesX >= 6400 ? 250 : 175;
			BiomeHeightLimit = (int)(Main.maxTilesY / 1.75f);

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
                
				for (int y = PlaceSwampY; y < BiomeHeightLimit; y++)
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
						if(tile.WallType == WallID.None && x > (int)Main.worldSurface)
                        {
                            WorldGen.PlaceWall(x, y, wallType, true);
                        }
                    }
                }
            }

            //Fixed base and hole fill
			for (int x = StartX; x <= EndX; x++)
			{
				for (int y = (int)Main.worldSurface - 70; y < BiomeHeightLimit; y++)
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
        }

        //Helper methods: Return a tile / wall based on deepness
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

        //Helper method: Tile check for jungle and desert
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
    }
}