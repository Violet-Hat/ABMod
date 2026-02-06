using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.WorldBuilding;
using Terraria.ModLoader;

using ABMod.Tiles.AncientSwampBiome.Trees;

namespace ABMod.Generation
{
	public class WorldgenTools
	{	
		public static bool NoFloatingIslands(int X, int Y)
		{
			for (int i = 0; i < GenVars.numIslandHouses; i++)
			{
				if (X > (GenVars.floatingIslandHouseX[i] - 100) && X < (GenVars.floatingIslandHouseX[i] + 100))
				{
					if (Y > (GenVars.floatingIslandHouseY[i] - 50) && Y < (GenVars.floatingIslandHouseY[i] + 50))
					{
						return false;
					}
				}
			}

			return true;
		}
		
		public static bool IsItPlaceable(Point origin, int r, StructureMap structures)
        {
			//Generate the new radius with padding, the diameter and the area
			int radius = r + 15;
			int diameter = radius * 2;
			Rectangle area = new Rectangle(origin.X - radius, origin.Y - radius, diameter, diameter);

			//Check if it is inside the world borders
			if (!WorldGen.InWorld(origin.X, origin.Y, r))
			{
				return false;
			}

			for (int i = area.Left; i < area.Right; i++)
			{
				for (int j = area.Top; j < area.Bottom; j++)
				{
					if (i < 41 || i > Main.maxTilesX - 42 || j < 41 || j > Main.maxTilesY)
					{
						return false;
					}
				}
			}
			
			//Check if it is far away from the islands
			for (int i = area.Left; i < area.Right; i++)
			{
				for (int j = area.Top; j < area.Bottom; j++)
				{
					if (NoFloatingIslands(i, j))
					{
						return false;
					}
				}
			}
			
			//Check if it can be placed here using the structure map
            if (!structures.CanPlace(area))
            {
                return false;
            }
			
			//Check for solids
			int count = 0;
			
            for (int i = area.Left; i < area.Right; i++)
			{
				for (int j = area.Top; j < area.Bottom; j++)
				{
					if(Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType])
					{
						count++;
					}
                }
            }
			
			if (count > 4)
            {
                return false;
            }
			
            return true;
        }
		
		public static bool GrowTreeCheck(int X, int Y, int distanceX, int distanceY, int extra = 0)
		{
			//If there's others around it, don't let it grow
			for (int i = X - distanceX; i < X + distanceX + extra; i++)
			{
				for (int j = Y - 5; j < Y + 5; j++)
				{
					if (Main.tile[i, j].HasTile && IsTreeType(i, j))
					{
						return false;
					}
				}
			}

			//If there's not enought space, don't let it grow
			for (int i = X - (distanceX / 2); i < X + (distanceX / 2) + extra; i++)
			{
				for (int j = Y - distanceY; j < Y; j++)
				{
					//only check for solid blocks
					if (Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType])
					{
						return false;
					}
				}
			}

			return true;
		}

		public static bool GrowSkinnyCheck(int X, int Y, int distanceX, int distanceY)
		{
			//If there's others around it, don't let it grow
			for (int i = X - distanceX; i < X + distanceX; i++)
			{
				for (int j = Y - 5; j < Y + 5; j++)
				{
					if (Main.tile[i, j].HasTile && IsTreeType(i, j, true))
					{
						return false;
					}
				}
			}

			for (int i = X - 1; i < X + 1; i++) //This one special
			{
				for (int j = Y - 5; j < Y + 5; j++)
				{
					if (Main.tile[i, j].HasTile && IsTreeType(i, j))
					{
						return false;
					}
				}
			}

			//If there's not enought space, don't let it grow
			for (int i = X - (int)(distanceX / 2f); i < X + (int)(distanceX / 2f); i++)
			{
				for (int j = Y - distanceY; j < Y; j++)
				{
					//only check for solid blocks
					if (Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType])
					{
						return false;
					}
				}
			}

			return true;
		}

		public static bool IsTreeType(int X, int Y, bool special = false)
		{
			if (!special)
			{
				return Main.tile[X, Y].TileType == (ushort)ModContent.TileType<Lep>() ||
					Main.tile[X, Y].TileType == (ushort)ModContent.TileType<Skinny>() ||
					Main.tile[X, Y].TileType == (ushort)ModContent.TileType<Equi>();
			}
			else
			{
				return Main.tile[X, Y].TileType == (ushort)ModContent.TileType<Lep>();
			}
		}

		static public int CheckTiles(int x, int y)
        {
            int count = 0;

            for (int nebX = x - 1; nebX <= x + 1; nebX++)
            {
                for (int nebY = y - 1; nebY <= y + 1; nebY++)
                {
                    if (nebX != x || nebY != y)
                    {
                        if (Main.tile[nebX, nebY].HasTile)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

		public static bool IsSloped(int X, int Y)
        {
            return Main.tile[X, Y].LeftSlope ||
			Main.tile[X, Y].RightSlope ||
			Main.tile[X, Y].TopSlope ||
			Main.tile[X, Y].BottomSlope ||
			Main.tile[X, Y].IsHalfBlock;
        }

		public static bool TooMuchLiquid(int x, int y, int amount = 1, int liquidAmount = 99)
		{
			int count = 0;

			for (int j = 1; j <= amount; j++)
			{
				if (Main.tile[x, y - j].LiquidAmount > liquidAmount)
				{
					count++;
				}
			}

			return count == amount;
		}

		internal static readonly List<Vector2> Directions = new List<Vector2>()
		{
			new Vector2(-1f, -1f),
			new Vector2(1f, -1f),
			new Vector2(-1f, 1f),
			new Vector2(1f, 1f),
			new Vector2(0f, -1f),
			new Vector2(-1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
		};
		
		public static float PerlinNoise2D(float x, float y, int octaves, int seed)
		{
			float SmoothFunction(float n) => 3f * n * n - 2f * n * n * n;

			float NoiseGradient(int s, int noiseX, int noiseY, float xd, float yd)
			{
				int hash = s;
				hash ^= 1619 * noiseX;
				hash ^= 31337 * noiseY;

				hash = hash * hash * hash * 60493;
				hash = (hash >> 13) ^ hash;

				Vector2 g = Directions[hash & 7];

				return xd * g.X + yd * g.Y;
			}

			int frequency = (int)Math.Pow(2D, octaves);
			x *= frequency;
			y *= frequency;

			int flooredX = (int)x;
			int flooredY = (int)y;
			int ceilingX = flooredX + 1;
			int ceilingY = flooredY + 1;
			float interpolatedX = x - flooredX;
			float interpolatedY = y - flooredY;
			float interpolatedX2 = interpolatedX - 1;
			float interpolatedY2 = interpolatedY - 1;

			float fadeX = SmoothFunction(interpolatedX);
			float fadeY = SmoothFunction(interpolatedY);

			float smoothX = MathHelper.Lerp(NoiseGradient(seed, flooredX, flooredY, interpolatedX, interpolatedY), NoiseGradient(seed, ceilingX, flooredY, interpolatedX2, interpolatedY), fadeX);
			float smoothY = MathHelper.Lerp(NoiseGradient(seed, flooredX, ceilingY, interpolatedX, interpolatedY2), NoiseGradient(seed, ceilingX, ceilingY, interpolatedX2, interpolatedY2), fadeX);

			return MathHelper.Lerp(smoothX, smoothY, fadeY);
		}
	}
}