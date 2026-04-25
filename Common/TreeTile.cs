using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.WorldBuilding;
using Terraria.ModLoader;

namespace ABMod.Common
{
    public class TreeTile
    {
        //Check if the tree can grow
        public static bool GrowTreeCheck(int X, int Y, int distanceX, int distanceY)
		{
			//If there's others around it, don't let it grow
			for (int i = X - distanceX; i < X + distanceX; i++)
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
			for (int i = X - (distanceX / 2); i < X + (distanceX / 2); i++)
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

        //Check if the special tree can grow
        public static bool GrowLepCheck(int X, int Y, int distanceX, int distanceY)
		{
			//If there's others around it, don't let it grow
			for (int i = X - distanceX; i < X + distanceX + 1; i++)
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
			for (int i = X - (distanceX / 2); i < X + (distanceX / 2) + 1; i++)
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

        public static bool IsTreeType(int X, int Y, int ignore = -1)
		{
			return true;
		}
    }
}