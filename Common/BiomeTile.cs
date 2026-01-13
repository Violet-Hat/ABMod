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

using ABMod.Tiles.AncientSwampBiome;

namespace ABMod.Common
{
	public class BiomeTile
    {
        //Check if the tile is a swamp tile
		public static bool IsSwampTile(int x, int y)
		{
			return Main.tile[x, y].TileType == (ushort)ModContent.TileType<PreservedDirt>() ||
				Main.tile[x, y].TileType == (ushort)ModContent.TileType<AncientDirt>() ||
				Main.tile[x, y].TileType == (ushort)ModContent.TileType<AncientStone>() ||
				Main.tile[x, y].TileType == (ushort)ModContent.TileType<PrehistoricMoss>();
		}
    }
}