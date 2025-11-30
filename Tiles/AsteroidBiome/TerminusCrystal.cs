using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Tiles.AsteroidBiome.Ambient;
using ABMod.Tiles;
using ABMod.Common;

namespace ABMod.Tiles.AsteroidBiome
{
	public class TerminusCrystal : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            AddMapEntry(new Color(225, 42, 63));
			HitSound = SoundID.Tink;
            DustType = DustID.RedTorch;
			MineResist = 1.8f;
			MinPick = 65;
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		
		public override void RandomUpdate(int i, int j)
        {
			//Grow some ambient
			Tile Tile = Framing.GetTileSafely(i, j);
			Tile Above = Framing.GetTileSafely(i, j - 1);
			Tile Below = Framing.GetTileSafely(i, j + 1);
			Tile Right = Framing.GetTileSafely(i + 1, j);
			Tile Left = Framing.GetTileSafely(i - 1, j);
			
			/*if(!Tile.BottomSlope && !Tile.TopSlope && !Tile.IsHalfBlock)
			{
				if(!Above.HasTile)
				{
					//The crystal appears
					if(Main.rand.NextBool(15))
					{
						WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<SmallCrystals>(), true);
						NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
						
						Main.tile[i, j - 1].TileFrameY = 0;
						Main.tile[i, j - 1].TileFrameX = (short)(WorldGen.genRand.Next(6) * 18);
					}
				}
				
				if(!Below.HasTile)
				{
					//The crystal appears
					if(Main.rand.NextBool(15))
					{
						WorldGen.PlaceTile(i, j + 1, (ushort)ModContent.TileType<SmallCrystals>(), true);
						NetMessage.SendTileSquare(-1, i, j + 1, 1, TileChangeType.None);
						
						Main.tile[i, j + 1].TileFrameY = 18;
						Main.tile[i, j + 1].TileFrameX = (short)(WorldGen.genRand.Next(6) * 18);
					}
				}
				
				if(!Left.HasTile)
				{
					//The crystal appears
					if(Main.rand.NextBool(15))
					{
						WorldGen.PlaceTile(i - 1, j, (ushort)ModContent.TileType<SmallCrystals>(), true);
						NetMessage.SendTileSquare(-1, i - 1, j, 1, TileChangeType.None);
						
						Main.tile[i - 1, j].TileFrameY = 36;
						Main.tile[i - 1, j].TileFrameX = (short)(WorldGen.genRand.Next(6) * 18);
					}
				}
				
				if(!Right.HasTile)
				{
					//The crystal appears
					if(Main.rand.NextBool(15))
					{
						WorldGen.PlaceTile(i + 1, j, (ushort)ModContent.TileType<SmallCrystals>(), true);
						NetMessage.SendTileSquare(-1, i + 1, j, 1, TileChangeType.None);
						
						Main.tile[i + 1, j].TileFrameY = 54;
						Main.tile[i + 1, j].TileFrameX = (short)(WorldGen.genRand.Next(6) * 18);
					}
				}
			}*/
		}
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.75f;
            g = 0.15f;
            b = 0.25f;
        }
	}
}