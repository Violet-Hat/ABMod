using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Tiles.AsteroidBiome.Ambient.Rock;
using ABMod.Common;

namespace ABMod.Tiles.AsteroidBiome
{
	public class AsteroidRock : ModTile
	{
		static ushort[] GiantMushrooms = new ushort[] {
			(ushort)ModContent.TileType<GiantEnergyMushroom1>(),
			(ushort)ModContent.TileType<GiantEnergyMushroom2>(),
			(ushort)ModContent.TileType<GiantEnergyMushroom3>(),
			(ushort)ModContent.TileType<GiantEnergyMushroom4>()
		};
		
		static ushort[] TerminusTiles = new ushort[] {
			(ushort)ModContent.TileType<Crystal1>(),
			(ushort)ModContent.TileType<Crystal2>(),
			(ushort)ModContent.TileType<Crystal3>()
		};
		
		static ushort[] BigTerminusTiles = new ushort[] {
			(ushort)ModContent.TileType<BigCrystal1>(),
			(ushort)ModContent.TileType<BigCrystal2>(),
			(ushort)ModContent.TileType<BigCrystal3>()
		};
		
		public override void SetStaticDefaults()
		{
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(25, 23, 34));
			HitSound = SoundID.Tink;
            DustType = DustID.Stone;
			MineResist = 1.8f;
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
				//Grow energy mushrooms
				if(Main.rand.NextBool(35))
				{
					WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<EnergyMushroom>(), true, style: Main.rand.Next(4));
					NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
				}
				
				//Grow giant ones
				if(Main.rand.NextBool(55))
				{
					ushort GiantMushroomTile = WorldGen.genRand.Next(GiantMushrooms);
					
					WorldGen.PlaceTile(i, j - 1, GiantMushroomTile, true);
					NetMessage.SendObjectPlacement(-1, i, j - 1, GiantMushroomTile, 0, 0, -1, -1);
				}
				
				//Average crystal
				if(Main.rand.NextBool(55))
				{
					ushort CrystalTile = WorldGen.genRand.Next(TerminusTiles);
					
					WorldGen.PlaceTile(i, j - 1, CrystalTile, true);
					NetMessage.SendObjectPlacement(-1, i, j - 1, CrystalTile, 0, 0, -1, -1);
				}
				
				//Big crystal
				if(Main.rand.NextBool(70))
				{
					ushort BigCrystalTile = WorldGen.genRand.Next(BigTerminusTiles);
					
					WorldGen.PlaceTile(i, j - 1, BigCrystalTile, true);
					NetMessage.SendObjectPlacement(-1, i, j - 1, BigCrystalTile, 0, 0, -1, -1);
				}
			}
        }
	}
}