using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Content.Tiles.Swamp;

namespace ABMod.Common.Tiles
{
	public class IsBiomeTile
    {
		//Swamp
		public static bool IsSwampTile(int x, int y)
		{
			Tile tile = Framing.GetTileSafely(x, y);

			return tile.TileType == (ushort)ModContent.TileType<SwampSoil>() ||
				tile.TileType == (ushort)ModContent.TileType<SwampDirt>() ||
				tile.TileType == (ushort)ModContent.TileType<SwampStone>() ||
				tile.TileType == (ushort)ModContent.TileType<SwampMoss>();
		}

		//Floating island
		public static bool IsFloatingIslandTile(int x, int y)
		{
			Tile tile = Framing.GetTileSafely(x, y);

			return tile.TileType == TileID.Cloud ||
				tile.TileType == TileID.RainCloud ||
				tile.TileType == TileID.SnowCloud ||
				tile.TileType == TileID.Sunplate;
		}

		//Desert
		public static bool IsDesertTile(int x, int y)
		{
			Tile tile = Framing.GetTileSafely(x, y);

			return tile.TileType == TileID.Sand ||
				tile.TileType == TileID.Sandstone ||
				tile.TileType == TileID.HardenedSand;
		}

		//Jungle
		public static bool IsJungleTile(int x, int y)
		{
			Tile tile = Framing.GetTileSafely(x, y);

			return tile.TileType == TileID.Mud ||
				tile.TileType == TileID.JungleGrass;
		}

		//Jungle temple
		public static bool IsTempleTile(int x, int y)
		{
			Tile tile = Framing.GetTileSafely(x, y);

			return tile.TileType == TileID.LihzahrdBrick ||
				tile.WallType == WallID.LihzahrdBrickUnsafe;
		}

		//Jungle structures
		public static bool IsJungleStructureTile(int x, int y)
		{
			Tile tile = Framing.GetTileSafely(x, y);
			
			return tile.TileType == TileID.LivingWood ||
				tile.TileType == TileID.LeafBlock ||
				tile.TileType == TileID.LivingMahogany ||
				tile.TileType == TileID.LivingMahoganyLeaves ||
				tile.TileType == TileID.Hive ||
				tile.WallType == WallID.LivingWoodUnsafe ||
				tile.WallType == WallID.LivingLeaf ||
				tile.WallType == WallID.LivingWood ||
				tile.WallType == WallID.HiveUnsafe;
		}
    }
}