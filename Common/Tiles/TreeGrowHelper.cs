using Terraria;
using Terraria.ModLoader;

using ABMod.Content.Tiles.Swamp.Trees;
using ABMod.Enums;
using Terraria.ID;

namespace ABMod.Common.Tiles
{
    public class TreeGrowHelper
    {
		//Grow conditions
        public static bool IsValidGrowingSpot(int i, int j, int validGroundType, int minHeight = 8, bool canItGrowUnderground = false, bool ignoreWalls = false)
        {
			Tile sapplingTile = Framing.GetTileSafely(i, j);

            //Check if it's underground
            if (!canItGrowUnderground && j > Main.worldSurface)
			{
				return false;
			}

			//Check if it's inside the world bounds
			if (!WorldGen.InWorld(i, j - minHeight))
			{
				return false;
			}

			//Check if there's a wall behind
			if (sapplingTile.WallType != WallID.None)
			{
				return false;
			}

			//Check if it has three valid tiles to grow on.
			//A tile is valid if its tile type is the valid ground type AND if it is a full block (no slopes, half block or slope)
			bool validGroundTile = Framing.GetTileSafely(i, j + 1).TileType == validGroundType && WorldGen.SolidTile(i, j + 1);
			bool validGroundTileLeft = Framing.GetTileSafely(i - 1, j + 1).TileType == validGroundType && WorldGen.SolidTile(i - 1, j + 1);
			bool validGroundTileRight = Framing.GetTileSafely(i + 1, j + 1).TileType == validGroundType && WorldGen.SolidTile(i + 1, j + 1);

			if (!validGroundTile || !validGroundTileLeft || !validGroundTileRight)
			{
				return false;
			}

			//The sappling requires at least two unobstructed tiles to the left and right
			for (int x = -2; x <= 2; x++)
			{
				for (int y = 0; y <= minHeight; y++)
				{
					//Ignore the sappling
					if (x == 0 && y < 2)
						continue;
					
					//If there's a tile and it is not a wild plants, return false
					Tile sampleTile = Framing.GetTileSafely(i + x, j - y);

					if (sampleTile.HasTile && sampleTile.TileType != TileID.Plants)
					{
						return false;
					}
				}
			}

            return true;
        }

        //Check if the tree can grow
        public static bool GrowTreeCheck(int x, int y, int distanceX, int distanceY)
		{
			//If there's others around it, don't let it grow
			for (int i = x - distanceX; i < x + distanceX; i++)
			{
				for (int j = y - 5; j < y + 5; j++)
				{
					Tile tile = Framing.GetTileSafely(i, j);

					if (tile.HasTile && IsTreeType(i, j))
					{
						return false;
					}
				}
			}

			//If there's not enought space, don't let it grow
			for (int i = x - (distanceX / 2); i < x + (distanceX / 2); i++)
			{
				for (int j = y - distanceY; j < y; j++)
				{
					Tile tile = Framing.GetTileSafely(i, j);

					//only check for solid blocks
					if (tile.HasTile && Main.tileSolid[tile.TileType])
					{
						return false;
					}
				}
			}

			return true;
		}

        //Check if the special tree can grow
        public static bool GrowLepCheck(int x, int y, int distanceX, int distanceY)
		{
			//If there's others around it, don't let it grow
			for (int i = x - distanceX; i < x + distanceX + 1; i++)
			{
				for (int j = y - 5; j < y + 5; j++)
				{
					Tile tile = Framing.GetTileSafely(i, j);

					if (tile.HasTile && IsTreeType(i, j))
					{
						return false;
					}
				}
			}

			//If there's not enought space, don't let it grow
			for (int i = x - (distanceX / 2); i < x + (distanceX / 2) + 1; i++)
			{
				for (int j = y - distanceY; j < y; j++)
				{
					Tile tile = Framing.GetTileSafely(i, j);

					//only check for solid blocks
					if (tile.HasTile && Main.tileSolid[tile.TileType])
					{
						return false;
					}
				}
			}

			return true;
		}

		//Check if the tile is a tree type
        public static bool IsTreeType(int x, int y, int ignore = -1)
		{
			Tile tile = Framing.GetTileSafely(x, y);

			if (ignore != (int)ModTreeTypes.Lep)
				return tile.TileType == (ushort)ModContent.TileType<Lep>();
			if (ignore != (int)ModTreeTypes.Astero)
				return tile.TileType == (ushort)ModContent.TileType<Astero>();
			if (ignore != (int)ModTreeTypes.Equi)
				return tile.TileType == (ushort)ModContent.TileType<Equi>();

			return false;
		}
    }
}