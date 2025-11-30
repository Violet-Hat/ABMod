using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Tiles.GreenMushroomBiome.Ambient
{
    public class GreenMushroom : ModTile
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
            TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<MushroomPasture>() };
            TileObjectData.addTile(Type);
			AddMapEntry(new Color(20, 144, 144));
            RegisterItemDrop(ModContent.ItemType<MushroomItem>());
            HitSound = SoundID.Grass;
			DustType = DustID.GreenTorch;
        }

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.10f;
            g = 0.30f;
            b = 0.25f;
        }

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = -14;
            height = 32;
        }
    }
}