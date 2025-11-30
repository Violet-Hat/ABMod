using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using ABMod.Tiles.AsteroidBiome;

namespace ABMod.Tiles.AsteroidBiome.Ambient.Rock
{
    public class EnergyMushroom : ModTile
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
            TileObjectData.newTile.RandomStyleRange = 4;
			TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<AsteroidRock>() };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
			AddMapEntry(new Color(27, 237, 191));
            HitSound = SoundID.Item8;
			DustType = DustID.HallowSpray;
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
            offsetY = 2;
        }

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.15f;
            g = 0.65f;
            b = 0.75f;
        }
    }
}