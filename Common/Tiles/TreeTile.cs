using Terraria;
using Terraria.ModLoader;

using ABMod.Content.Tiles.Swamp.Trees;

namespace ABMod.Common.Tiles
{
    public class TreeTile
    {
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

			if (ignore != 0)
				return tile.TileType == (ushort)ModContent.TileType<Lep>();
			if (ignore != 1)
				return tile.TileType == (ushort)ModContent.TileType<Astero>();

			return false;
		}
    }
}