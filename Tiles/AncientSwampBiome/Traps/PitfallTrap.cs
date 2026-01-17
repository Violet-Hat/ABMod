using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using System.Diagnostics;

namespace ABMod.Tiles.AncientSwampBiome.Traps
{
	public class PitfallTrap : ModTile
	{
		//Texture
		public override string Texture => "ABMod/Tiles/AncientSwampBiome/Traps/PitfallTrap";

		public override void SetStaticDefaults()
		{
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
			TileID.Sets.DoesntGetReplacedWithTileReplacement[Type] = true;
			TileID.Sets.CrackedBricks[Type] = true;
            Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileCracked[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 115, 54));
            DustType = DustID.GreenMoss;
			MineResist = 1.3f;
		}

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            switch (WorldGen.genRand.Next(4))
			{
				default:
					bool left = Framing.GetTileSafely(i - 1, j).TileType == ModContent.TileType<PitfallTrap>();

					if (left && WorldGen.genRand.NextBool())
						WorldGen.KillTile(i - 1, j);

					break;

				case 1:
					bool right = Framing.GetTileSafely(i + 1, j).TileType == ModContent.TileType<PitfallTrap>();

					if (right && WorldGen.genRand.NextBool())
						WorldGen.KillTile(i + 1, j);
					
					break;

				case 2:
					bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<PitfallTrap>();

					if (up && WorldGen.genRand.NextBool())
						WorldGen.KillTile(i, j - 1);
					
					break;

				case 3:
					bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<PitfallTrap>();

					if (down && WorldGen.genRand.NextBool())
						WorldGen.KillTile(i, j + 1);
					
					break;
			}
        }
		
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}