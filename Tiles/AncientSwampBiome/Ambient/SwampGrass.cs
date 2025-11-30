using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace ABMod.Tiles.AncientSwampBiome.Ambient
{
    public class SwampGrass : ModTile
	{
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileCut[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.RandomStyleRange = 28;
			TileObjectData.newTile.AnchorValidTiles = new int[]
            {
                ModContent.TileType<PrehistoricMoss>() 
            };
            TileObjectData.addTile(Type);
            HitSound = SoundID.Grass;
			DustType = DustID.Grass;
        }
		
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if ((i % 10) < 5)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
		
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = -14;
            height = 32;
        }
    }
}