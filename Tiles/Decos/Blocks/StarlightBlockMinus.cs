using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.Decos.Blocks
{
	public class StarlightBlockMinus : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            AddMapEntry(new Color(255, 235, 242));
			HitSound = SoundID.Shatter;
            DustType = DustID.RedTorch;
			MineResist = 1f;
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.70f;
            g = 0.70f;
            b = 0.70f;
        }
		
		public override void HitWire(int i, int j)
        {
            if (Main.tile[i, j].TileFrameY >= 90)
			{
                Main.tile[i, j].TileFrameY -= 90;
			}
            else
			{
                Main.tile[i, j].TileFrameY += 90;
			}
        }
	}
}