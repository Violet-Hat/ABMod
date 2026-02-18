using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.AncientSwampBiome;

namespace ABMod.Common
{
	public class BiomeTile
    {
		public static bool IsSwampTile(int x, int y)
		{
			return Main.tile[x, y].TileType == (ushort)ModContent.TileType<PreservedDirt>() ||
				Main.tile[x, y].TileType == (ushort)ModContent.TileType<AncientDirt>() ||
				Main.tile[x, y].TileType == (ushort)ModContent.TileType<AncientStone>() ||
				Main.tile[x, y].TileType == (ushort)ModContent.TileType<PrehistoricMoss>();
		}

		public static bool IsFloatingIslandTile(int x, int y)
		{
			return Main.tile[x, y].TileType == TileID.Cloud ||
				Main.tile[x, y].TileType == TileID.RainCloud ||
				Main.tile[x, y].TileType == TileID.SnowCloud ||
				Main.tile[x, y].TileType == TileID.Sunplate;
		}

		public static bool IsDesertTile(int x, int y)
		{
			return Main.tile[x, y].TileType == TileID.Sand ||
				Main.tile[x, y].TileType == TileID.Sandstone ||
				Main.tile[x, y].TileType == TileID.HardenedSand;
		}

		public static bool IsJungleTile(int x, int y)
		{
			return Main.tile[x, y].TileType == TileID.Mud ||
				Main.tile[x, y].TileType == TileID.JungleGrass;
		}

		public static bool IsTempleTile(int x, int y)
		{
			return Main.tile[x, y].TileType == TileID.LihzahrdBrick ||
				Main.tile[x, y].WallType == WallID.LihzahrdBrickUnsafe;
		}

		public static bool IsJungleStructureTile(int x, int y)
		{
			return Main.tile[x, y].TileType == TileID.LivingWood ||
				Main.tile[x, y].TileType == TileID.LeafBlock ||
				Main.tile[x, y].TileType == TileID.LivingMahogany ||
				Main.tile[x, y].TileType == TileID.LivingMahoganyLeaves ||
				Main.tile[x, y].TileType == TileID.Hive ||
				Main.tile[x, y].WallType == WallID.LivingWoodUnsafe ||
				Main.tile[x, y].WallType == WallID.LivingLeaf ||
				Main.tile[x, y].WallType == WallID.LivingWood ||
				Main.tile[x, y].WallType == WallID.HiveUnsafe;
		}
    }
}