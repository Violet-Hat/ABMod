using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp
{
	public class LabBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileBlendAll[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(148, 149, 123));
			DustType = DustID.Stone;
			MineResist = 1f;
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			//Do the special 8 way framing. Return false to prevent the game from doing its normal framing after this.
            Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
            return false;
        }
	}
}