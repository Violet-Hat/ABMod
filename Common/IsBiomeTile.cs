using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Content.Tiles.Swamp;

namespace ABMod.Common
{
	public class BiomeTile
    {
		//Swamp
		public static bool IsSwampTile(int x, int y)
		{
			return Main.tile[x, y].TileType == (ushort)ModContent.TileType<SwampSoil>() ||
				Main.tile[x, y].TileType == (ushort)ModContent.TileType<SwampDirt>() ||
				Main.tile[x, y].TileType == (ushort)ModContent.TileType<SwampStone>() ||
				Main.tile[x, y].TileType == (ushort)ModContent.TileType<SwampMoss>();
		}

		//Floating island
		public static bool IsFloatingIslandTile(int x, int y)
		{
			return Main.tile[x, y].TileType == TileID.Cloud ||
				Main.tile[x, y].TileType == TileID.RainCloud ||
				Main.tile[x, y].TileType == TileID.SnowCloud ||
				Main.tile[x, y].TileType == TileID.Sunplate;
		}

		//Desert
		public static bool IsDesertTile(int x, int y)
		{
			return Main.tile[x, y].TileType == TileID.Sand ||
				Main.tile[x, y].TileType == TileID.Sandstone ||
				Main.tile[x, y].TileType == TileID.HardenedSand;
		}

		//Jungle
		public static bool IsJungleTile(int x, int y)
		{
			return Main.tile[x, y].TileType == TileID.Mud ||
				Main.tile[x, y].TileType == TileID.JungleGrass;
		}

		//Jungle temple
		public static bool IsTempleTile(int x, int y)
		{
			return Main.tile[x, y].TileType == TileID.LihzahrdBrick ||
				Main.tile[x, y].WallType == WallID.LihzahrdBrickUnsafe;
		}

		//Jungle structures
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