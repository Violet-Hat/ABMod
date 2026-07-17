using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace ABMod.Content.Generation.Helpers
{
	public class WorldGenTools
	{	
		public static bool NoFloatingIslands(int x, int y)
		{
			for (int i = 0; i < GenVars.numIslandHouses; i++)
			{
				if (x > (GenVars.floatingIslandHouseX[i] - 100) && x < (GenVars.floatingIslandHouseX[i] + 100))
				{
					if (y > (GenVars.floatingIslandHouseY[i] - 50) && y < (GenVars.floatingIslandHouseY[i] + 50))
					{
						return false;
					}
				}
			}

			return true;
		}
		
		public static bool IsItPlaceable(Point origin, int r)
        {
			//Check if it is inside the world borders
			if (!WorldGen.InWorld(origin.X, origin.Y, r))
			{
				return false;
			}

			for (int i = origin.X - r; i <= origin.X + r; i++)
			{
				for (int j = origin.Y - r; j <= origin.Y + r; j++)
				{
					if (i < 41 || i > Main.maxTilesX - 42 || j < 41 || j > Main.maxTilesY)
					{
						return false;
					}
				}
			}
			
			//Check if it is far away from the islands
			for (int i = origin.X - r; i <= origin.X + r; i++)
			{
				for (int j = origin.Y - r; j <= origin.Y + r; j++)
				{
					if (!NoFloatingIslands(i, j))
					{
						return false;
					}
				}
			}
			
			//Check for solids
			int count = 0;
			
            for (int i = origin.X - r; i <= origin.X + r; i++)
			{
				for (int j = origin.Y - r; j <= origin.Y + r; j++)
				{
					Tile tile = Framing.GetTileSafely(i, j);

					if(tile.HasTile && Main.tileSolid[tile.TileType])
					{
						count++;
					}
                }
            }
			
			if (count > 9)
            {
                return false;
            }
			
            return true;
        }

		public static void PlaceOrReplaceWall(int x, int y, int wallType)
		{
			Tile tile = Framing.GetTileSafely(x, y);

			if (tile.WallType > WallID.None)
			{
				tile.WallType = (ushort)wallType;
			}
			else
			{
				WorldGen.PlaceWall(x, y, (ushort)wallType, true);
			}
		}

		public static bool IsTouchingAir(int x, int y)
		{
			for (int i = x - 1; i <= x + 1; i++)
			{
				for (int j = y - 1; j <= y + 1; j++)
				{
					if (!Framing.GetTileSafely(i, j).HasTile)
					{
						return true;
					}
				}
			}
			
			return false;
		}

		public static int MooreTiles(int x, int y)
        {
            int count = 0;

            for (int nebX = x - 1; nebX <= x + 1; nebX++)
            {
                for (int nebY = y - 1; nebY <= y + 1; nebY++)
                {
                    if (nebX != x || nebY != y)
                    {
                        if (Framing.GetTileSafely(nebX, nebY).HasTile)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        public static int NeumanTiles(int x, int y)
        {
            int count = 0;

            if (Framing.GetTileSafely(x, y - 1).HasTile)
				count++;
			if (Framing.GetTileSafely(x - 1, y).HasTile)
				count++;
			if (Framing.GetTileSafely(x + 1, y).HasTile)
				count++;
			if (Framing.GetTileSafely(x, y + 1).HasTile)
				count++;

            return count;
        }

		public static bool IsSloped(int x, int y)
        {
            return Framing.GetTileSafely(x, y).LeftSlope ||
			Framing.GetTileSafely(x, y).RightSlope ||
			Framing.GetTileSafely(x, y).TopSlope ||
			Framing.GetTileSafely(x, y).BottomSlope ||
			Framing.GetTileSafely(x, y).IsHalfBlock;
        }

		public static bool TooMuchLiquid(int x, int y, int amount = 1, int liquidAmount = 0)
		{
			int count = 0;

			for (int j = 1; j <= amount; j++)
			{
				if (Framing.GetTileSafely(x, y - j).LiquidAmount > liquidAmount)
				{
					count++;
				}
			}

			return count == amount;
		}

        #region "Perlin Noise 1D"
		private static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);

		private static float Lerp(float a, float b, float t) => a + t * (b - a);

		private static float Grad(int p) => (p & 1) == 0 ? 1f : -1f;

		private static uint StableHash(int x)
		{
			uint u = (uint)x;
			u = ((u >> 16) ^ u) * 0x45d9f3b;
			u = ((u >> 16) ^ u) * 0x45d9f3b;
			u = (u >> 16) ^ u;
			return u;
		}

		private static float Perlin1D(float x, int seed)
		{
			int xi = (int)MathF.Floor(x);
			float xf = x - xi;

			int h0 = (int)StableHash(xi + seed);
			int h1 = (int)StableHash(xi + 1 + seed);

			float d0 = Grad(h0) * xf;
			float d1 = Grad(h1) * (xf - 1);

			float u = Fade(xf);

			return Lerp(d0, d1, u);
		}

		public static float Perlin(float x, int seed, int octaves = 4, float persistence = 0.5f)
		{
			float total = 0f;
			float frequency = 1f;
			float amplitude = 1f;
			float maxValue = 0f;

			for (int i = 0; i < octaves; i++)
			{
				total += Perlin1D(x * frequency, seed + i) * amplitude;
				maxValue += amplitude;

				amplitude *= persistence;
				frequency *= 2f;
			}

			return total / maxValue * 0.5f + 0.5f;
		}
		#endregion

        # region "Perlin Noise 2D"
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
        #endregion
	}
}