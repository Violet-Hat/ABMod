using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using ABMod.Tiles.AsteroidBiome;
using ABMod.Tiles.AsteroidBiome.Moss;

namespace ABMod.Tiles.AsteroidBiome.Ambient.Grasses.Red
{
    public class MossGrassRed : ModTile
	{
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = true;
			TileID.Sets.SwaysInWindBasic[Type] = true;
			TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.RandomStyleRange = 12;
			TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<AsteroidMossRed>() };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
			AddMapEntry(new Color(220, 30, 46));
            HitSound = SoundID.Grass;
			DustType = DustID.RedTorch;
        }
		
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
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