using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.IO;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.ModLoader;

namespace ABMod.Generation
{
	public class WorldgenTools
	{
		public static void PlaceCircle(int X, int Y, int tileType, int wallType, int radiusX, int radiusY, bool clearTiles, bool clearWalls, bool blotches)
		{
			ShapeData circle = new ShapeData();
			GenAction blotchMod;
			
			if(blotches)
			{
				blotchMod = new Modifiers.Blotches(2, 0.4);
				
				WorldUtils.Gen(new Point(X, Y), new Shapes.Circle(radiusX, radiusY), Actions.Chain(new GenAction[]
				{
					blotchMod.Output(circle)
				}));
			}
			else
			{
				blotchMod = new Modifiers.Blotches(2, 0);
				
				WorldUtils.Gen(new Point(X, Y), new Shapes.Circle(radiusX, radiusY), Actions.Chain(new GenAction[]
				{
					blotchMod.Output(circle)
				}));
			}
			
			//Clear tiles
			if(clearTiles)
			{
				WorldUtils.Gen(new Point(X, Y), new ModShapes.All(circle), Actions.Chain(new GenAction[]
				{
					new Actions.ClearTile(), new Actions.SetLiquid(0, 0)
				}));
			}
			
			//place tiles
			if (tileType > -1)
			{
				WorldUtils.Gen(new Point(X, Y), new ModShapes.All(circle), Actions.Chain(new GenAction[]
				{
					new Actions.PlaceTile((ushort)tileType)
				}));
			}
			
			//Wall stuff
			ShapeData wallCircle = new ShapeData();
			GenAction wallBlotchMod;
			
			if(blotches)
			{
				wallBlotchMod = new Modifiers.Blotches(2, 0.4);
				
				WorldUtils.Gen(new Point(X, Y), new Shapes.Circle(radiusX - 1, radiusY - 1), Actions.Chain(new GenAction[]
				{
					wallBlotchMod.Output(wallCircle)
				}));
			}
			else
			{
				wallBlotchMod = new Modifiers.Blotches(2, 0);
				
				WorldUtils.Gen(new Point(X, Y), new Shapes.Circle(radiusX - 1, radiusY - 1), Actions.Chain(new GenAction[]
				{
					wallBlotchMod.Output(wallCircle)
				}));
			}
			
			//Clear walls
			if(clearWalls)
			{
				WorldUtils.Gen(new Point(X, Y), new ModShapes.All(wallCircle), Actions.Chain(new GenAction[]
				{
					new Actions.ClearWall()
				}));
			}
			
			//Place wall
			if(wallType > 0)
			{
				WorldUtils.Gen(new Point(X, Y), new ModShapes.All(wallCircle), Actions.Chain(new GenAction[]
				{
					new Actions.PlaceWall((ushort)wallType)
				}));
			}
		}
		
		public static bool NoFloatingIslands(int X, int Y, int area)
		{
			for (int i = X - area; i < X + area; i++)
			{
				for (int j = Y - area; j < Y + area; j++)
				{
					if (WorldGen.InWorld(i, j))
					{
						if (Main.tile[i, j].TileType == TileID.Cloud || Main.tile[i, j].TileType == TileID.RainCloud || Main.tile[i, j].TileType == TileID.Sunplate)
						{
							return false;
						}
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
					if (Main.tile[i, j].TileType == TileID.Cloud || Main.tile[i, j].TileType == TileID.RainCloud || Main.tile[i, j].TileType == TileID.Sunplate)
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
		
		public static bool GrowTreeCheck(int X, int Y, int distanceX, int distanceY, int tileType, int extra = 0)
		{
			int canPlace = 0;

			//If there's others around it, don't let it grow
			for (int i = X - distanceX; i < X + distanceX + extra; i++)
			{
				for (int j = Y - 5; j < Y + 5; j++)
				{
					if (Main.tile[i, j].HasTile && (Main.tile[i, j].TileType == tileType))
					{
						canPlace++;
						if (canPlace > 0)
						{
							return false;
						}
					}
				}
			}

			//If there's not enought space, don't let it grow
			for (int i = X - (int)(distanceX / 2); i < X + (int)(distanceX / 2) + extra; i++)
			{
				for (int j = Y - distanceY; j < Y - 2; j++)
				{
					//only check for solid blocks
					if (Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType])
					{
						canPlace++;
						if (canPlace > 0)
						{
							return false;
						}
					}
				}
			}

			return true;
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