using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.IO;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
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

						//Replace or place walls
						if (tile.WallType > WallID.None)
						{
							tile.WallType = (ushort)wallType;
						}
						else if(y > (int)Main.worldSurface)
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

							if(WorldGen.genRand.NextBool())
							{
								//Replace tiles
								if (tile.HasTile)
								{
									if (WorldGen.SolidTile(x, y))
									{
										tile.TileType = (ushort)tileType;
									}
								}

								//Replace or place walls
								if (tile.WallType > WallID.None)
								{
									tile.WallType = (ushort)wallType;
								}
								else if(y > (int)Main.worldSurface)
								{
									WorldGen.PlaceWall(x, y, wallType, true);
								}
							}
						}
					}
				}
			}
        }

		//Main method : Flatten the terrain, place soil tiles, patches of dirt and stone
		public static void SwampFlattening(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Language.GetOrRegister("Mods.ABMod.WorldgenTasks.Swamp2").Value;

			//Beginning and end of the biome
			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth + 1; //+1 so the linear bezier reaches the end of the biome

			//Ground points
			int LeftY = FindGround(StartX);
			int RightY = FindGround(EndX);

			ConnectPoints(new Vector2(StartX, LeftY), new Vector2(EndX, RightY));

			//Place the soil surface
			int highestPoint = Math.Min(LeftY, RightY) - 15;

			for (int x = StartX - 10; x < EndX + 10; x++)
			{
				int startY = FindGround(x, highestPoint);

				//Place soil tiles and walls
				for (int y = startY; y <= startY + 50; y++)
				{
					if (BiomeTile.IsSwampTile(x, y))
					{
						Tile tile = Framing.GetTileSafely(x, y);

						if (y > startY + 40)
						{
							if (WorldGen.genRand.NextBool())
							{
								tile.TileType = (ushort)ModContent.TileType<SwampSoil>();
								WorldGenTools.PlaceOrReplaceWall(x, y, ModContent.WallType<SwampSoilWall>());
							}
						}
						else
						{
							tile.TileType = (ushort)ModContent.TileType<SwampSoil>();
							WorldGenTools.PlaceOrReplaceWall(x, y, ModContent.WallType<SwampSoilWall>());
						}
					}
				}
			}

			//Remove walls touching air
			for (int x = StartX - 10; x < EndX + 10; x++)
			{
				for (int y = highestPoint; y < (int)Main.worldSurface; y++)
				{
					Tile tile = Framing.GetTileSafely(x, y);

					if (BiomeTile.IsSwampTile(x, y) && tile.WallType > WallID.None && WorldGenTools.IsTouchingAir(x, y))
					{
						WorldGen.KillWall(x, y);
					}
				}
			}

			progress.Set(0.5);

			//Patches of stone and dirt
			int i = 0;
			int patchCount = (int)(BiomeWidth * 0.135f) + WorldGen.genRand.Next(6); //Surface

			while (patchCount > 0 && i++ < 10000)
			{
				int x = WorldGen.genRand.Next(StartX, EndX);
				int y = WorldGen.genRand.Next(highestPoint, (int)Main.worldSurface + 25);

				if (BiomeTile.IsSwampTile(x, y))
				{
					Tile tile = Framing.GetTileSafely(x, y);
					int type = (tile.TileType == ModContent.TileType<SwampSoil>()) ? ModContent.TileType<SwampDirt>() : ModContent.TileType<SwampStone>();

					WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 16), WorldGen.genRand.Next(20, 41), type, false, 0f, 0f, false, true);
					patchCount--;
				}
			}

			i = 0;
			patchCount = (int)(BiomeWidth * 0.175f) + WorldGen.genRand.Next(6); //Underground

			while (patchCount > 0 && i++ < 10000)
			{
				int x = WorldGen.genRand.Next(StartX, EndX);
				int y = WorldGen.genRand.Next((int)Main.worldSurface + 50, BiomeHeightLimit - 25);

				if (BiomeTile.IsSwampTile(x, y))
				{
					int type = ModContent.TileType<SwampDirt>();
					WorldGen.TileRunner(x, y, WorldGen.genRand.Next(10, 21), WorldGen.genRand.Next(50, 71), type, false, 0f, 0f, false, true);
					patchCount--;
				}
			}
		}

		//Main method : Cave generation
		public static void SwampCaves(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Language.GetOrRegister("Mods.ABMod.WorldgenTasks.Swamp3").Value;

			//Beginning and end of the biome
			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth;

			//Clean up the vanilla gen shenanigans
			for (int x = StartX; x <= EndX; x++)
			{
				for (int y = PlaceSwampY; y <= BiomeHeightLimit; y++)
				{
					if (WorldGenTools.NoFloatingIslands(x, y))
					{
						Tile tile = Framing.GetTileSafely(x, y);

						//Make sure it's not a floating island or jungle structure tile
						if (!BiomeTile.IsFloatingIslandTile(x, y) && !BiomeTile.IsJungleStructureTile(x, y) && !BiomeTile.IsTempleTile(x, y))
						{
							//Remove vanilla tiles
							if (tile.HasTile && !BiomeTile.IsSwampTile(x, y))
							{
								WorldGen.KillTile(x, y, noItem: true);
								tile.HasTile = false;
							}

							//Place a tile if there's no tile and there's a wall
							if (!tile.HasTile && tile.WallType > WallID.None && tile.WallType != WallID.LihzahrdBrickUnsafe)
							{
								int tileType = CheckTileOrWall(y);
								WorldGen.PlaceTile(x, y, tileType);
							}
						}
					}
				}
			}

			progress.Set(0.25);

			//Cave creation
			int seed = WorldGen.genRand.Next();
			int octaves = 5;

			float divConstant = 225f;
			float clearChance = 0.6f;

			int[] caveStart = [(int)(Main.worldSurface - 5), (int)(Main.worldSurface + 30), (int)Main.rockLayer];
			int[] caveEnd = [(int)(Main.worldSurface + 45), (int)(Main.rockLayer + 15), BiomeHeightLimit];

			//A higher x div value creates more horizontal caves, a higher y div value creates more vertical ones
			float[] caveXDiv = [550f, 350f, 575f];
			float[] caveYDiv = [300f, 575f, 350f];

			for (int i = 0; i < 3; i++)
			{
				for (int x = StartX; x <= EndX; x++)
				{
					for (int y = caveStart[i]; y <= caveEnd[i]; y++)
					{
						if (BiomeTile.IsSwampTile(x, y))
						{
							//Perlin noise values
							float horizontalOffsetNoise = WorldGenTools.PerlinNoise2D(x / divConstant, y / divConstant, octaves, unchecked(seed + 1)) * 0.01f;
							float cavePerlinValue = WorldGenTools.PerlinNoise2D(x / caveXDiv[i], y / caveYDiv[i], octaves, seed) + 0.5f + horizontalOffsetNoise;
							float cavePerlinValue2 = WorldGenTools.PerlinNoise2D(x / caveXDiv[i], y / caveYDiv[i], octaves, unchecked(seed - 1)) + 0.5f;
							float caveNoiseMap = (cavePerlinValue + cavePerlinValue2) * 0.5f;
							float caveCreationThreshold = horizontalOffsetNoise * 3.5f + 0.2f;

							//Remove tiles based on the noise and a float value
							bool noiseCheck = caveNoiseMap * caveNoiseMap > caveCreationThreshold;
							bool floatCheck = WorldGen.genRand.NextFloat() < clearChance;

							if (noiseCheck && floatCheck)
							{
								WorldGen.KillTile(x, y, noItem: true);
							}
						}
					}
				}
			}

			progress.Set(0.75);

			//Smooth the noise
			for (int l = 0; l < 10; l++)
			{
				for (int x = StartX; x <= EndX; x++)
				{
					for(int y = (int)Main.worldSurface - 10; y <= BiomeHeightLimit; y++)
					{
						int tileCount = WorldGenTools.MooreTiles(x, y);

						if (tileCount > 4)
						{
							int tileType = TilesAround(x, y);
							WorldGen.PlaceTile(x, y, tileType, true);
						}
						else if (tileCount < 4)
						{
							WorldGen.KillTile(x, y, noItem: true);
						}
					}
				}
			}

			//Place liquids
			for (int x = StartX; x <= EndX; x++)
			{
				for(int y = (int)Main.worldSurface; y <= BiomeHeightLimit; y++)
				{
					if(WorldGen.genRand.NextBool(8))
						WorldGen.PlaceLiquid(x, y, 0, 255);
				}
			}
		}

		//Main method : Generate grass and structures
		public static void SwampAmbience(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Language.GetOrRegister("Mods.ABMod.WorldgenTasks.Swamp4").Value;

			//Beginning and end of the biome
			int StartX = PlaceSwampX - BiomeWidth;
			int EndX = PlaceSwampX + BiomeWidth;

			//Structure values
			List<int> partitions = [];
			List <String> structuresNames =
            [
				"SwampTunnel",
				"SwampLake",
                "SwampStruct1",
				"SwampStruct2",
				"SwampStruct3",
				"SwampStruct4",
				"SwampStruct5",
				"SwampStruct6"
			];

			int numSurfaceStructures = Main.maxTilesX >= 8400 ? 6 : Main.maxTilesX >= 6400 ? 5 : 4;

			bool placedTunnel = false;
			bool placedLake = false;

			//Create the partitions
			int partitionSize = BiomeWidth * 2 / (numSurfaceStructures + 1);

			for (int i = 0; i < numSurfaceStructures; i++)
			{
				partitions.Add(StartX + (partitionSize * (i + 1)));
			}

			//Place the structures
			int index;
			int closestToCenter = (GenVars.dungeonSide == -1) ? partitions[0] : partitions[^1];
			string structureFile;

			foreach (int pos in partitions)
			{
				if (!placedTunnel && pos == closestToCenter)
				{
					index = 0;
					structureFile = structuresNames[index];
					structuresNames.Remove(structureFile);

					GenerateTunnel(pos);
					placedTunnel = true;

					continue;
				}
			}

			//Grass
			for(int x = StartX; x <= EndX; x++)
			{
				for(int y = PlaceSwampY; y <= Main.worldSurface; y++)
				{
					WorldGen.SpreadGrass(x, y, ModContent.TileType<SwampSoil>(), ModContent.TileType<SwampMoss>());
				}
			}
		}

        //Helper method : Return a tile / wall based on deepness
		private static int CheckTileOrWall(int y, bool returnWall = false)
		{
			if (y >= (int)Main.worldSurface + 40)
			{
				return returnWall ? ModContent.WallType<SwampStoneWall>() : ModContent.TileType<SwampStone>();
			}
			else if (y >= (int)Main.worldSurface + 30)
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
		private static void ConnectPoints(Vector2 p0, Vector2 p1)
		{
			//HashSet to save the X coordinates we already visited
			HashSet <int> visitedX = [];

			//Values for the perlin noise
			int seed = WorldGen.genRand.Next();
			int height = 12;
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
		private static bool CanBePlaced(int i, int j)
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

		//Helper method : Check tiles around and return which one is more present
		private static int TilesAround(int x, int y)
		{
			int soilCount = 0;
			int dirtCount = 0;
			int stoneCount = 0;

			for (int nebX = x - 1; nebX <= x + 1; nebX++)
			{
				for (int nebY = y - 1; nebY <= y + 1; nebY++)
				{
					if (nebX != x || nebY != y)
					{
						if (Main.tile[nebX, nebY].TileType == ModContent.TileType<SwampSoil>())
							soilCount++;
						if (Main.tile[nebX, nebY].TileType == ModContent.TileType<SwampDirt>())
							dirtCount++;
						if (Main.tile[nebX, nebY].TileType == ModContent.TileType<SwampStone>())
							stoneCount++;
					}
				}
			}

			if (soilCount >= dirtCount && soilCount >= stoneCount)
			{
				return ModContent.TileType<SwampSoil>();
			}
			else if (dirtCount >= soilCount && dirtCount >= stoneCount)
			{
				return ModContent.TileType<SwampDirt>();
			}
			else
			{
				return ModContent.TileType<SwampStone>();
			}
		}

		//Helper method : Find ground
		private static int FindGround(int x, int startY = -1)
		{
			int y = (startY == -1) ? PlaceSwampY : startY;

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
		
		//Helper method : Generate a tunnel
		private static void GenerateTunnel(int x)
		{
			int y = FindGround(x) - 5;

			//Tunnel time
			int area = Main.maxTilesX >= 8400 ? 55 : Main.maxTilesX >= 6400 ? 45 : 35;
			int limit = Math.Abs((int)(Main.worldSurface + 45) - y);
			int currentX = 0;
			int tunnelWidth = 3;

			float roughness = 0.5f;
			float curvyness = 0.5f;

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
					
					//Check that isn't too close to the left side limit
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
					WorldGen.KillTile(x + currentX + i, y + j, noItem: true);
				}
			}

			//Clean up
			for (int l = 0; l < 10; l++)
			{
				for (int i = x - area; i <= x + area; i++)
				{
					for(int j = y - 5; j <= (int)(Main.worldSurface + 45); j++)
					{
						int tileCount = WorldGenTools.MooreTiles(i, j);

						if (tileCount > 4)
						{
							int tileType = TilesAround(i, j);
							WorldGen.PlaceTile(i, j, tileType, true);
						}
						else if (tileCount < 4)
						{
							WorldGen.KillTile(i, j, noItem: true);
						}
					}
				}
			}
		}
    }
}