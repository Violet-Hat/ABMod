using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Tiles;
using ABMod.Tiles.AsteroidBiome.Ambient.Soft;
using ABMod.Common;

namespace ABMod.Tiles.AsteroidBiome
{
	public class AsteroidStone : ModTile
	{
		static ushort[] RockTiles = new ushort[] {
			(ushort)ModContent.TileType<Rock1>(),
			(ushort)ModContent.TileType<Rock2>(),
			(ushort)ModContent.TileType<Rock3>()
		};
		
		static ushort[] BigRockTiles = new ushort[] {
			(ushort)ModContent.TileType<BigRock1>(),
			(ushort)ModContent.TileType<BigRock2>(),
			(ushort)ModContent.TileType<BigRock3>()
		};
		
		public override void SetStaticDefaults()
		{
			TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(87, 64, 54));
			HitSound = SoundID.Tink;
            DustType = DustID.Dirt;
			MineResist = 1.2f;
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		
		public override void RandomUpdate(int i, int j)
        {
			Tile Tile = Framing.GetTileSafely(i, j);
			Tile Above = Framing.GetTileSafely(i, j - 1);
			
			if (!Above.HasTile && !Tile.BottomSlope && !Tile.TopSlope && !Tile.IsHalfBlock)
			{
				//Small rock
				if(Main.rand.NextBool(35))
				{
					WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<SmallRock>(), true, style: Main.rand.Next(3));
					NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
				}
				
				//Average rock
				if(Main.rand.NextBool(55))
				{
					ushort RockTile = WorldGen.genRand.Next(RockTiles);
					
					WorldGen.PlaceTile(i, j - 1, RockTile, true);
					NetMessage.SendObjectPlacement(-1, i, j - 1, RockTile, 0, 0, -1, -1);
				}
				
				//Big rock
				if(Main.rand.NextBool(70))
				{
					ushort BigRockTile = WorldGen.genRand.Next(BigRockTiles);
					
					WorldGen.PlaceTile(i, j - 1, BigRockTile, true);
					NetMessage.SendObjectPlacement(-1, i, j - 1, BigRockTile, 0, 0, -1, -1);
				}
			}
		}
	}
}