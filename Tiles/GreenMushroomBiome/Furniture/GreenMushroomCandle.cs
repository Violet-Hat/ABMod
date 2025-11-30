using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.Enums;

namespace ABMod.Tiles.GreenMushroomBiome.Furniture
{
	public class GreenMushroomCandle : ModTile
	{	
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 22 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(13, 91, 44), name);
			DustType = DustID.GreenTorch;
            RegisterItemDrop(ModContent.ItemType<GreenMushroomCandleItem>());
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AdjTiles = new int[] { TileID.Candles };
		}
		
		public override void HitWire(int i, int j)
        {
            if (Main.tile[i, j].TileFrameX >= 18)
			{
                Main.tile[i, j].TileFrameX -= 18;
			}
            else
			{
                Main.tile[i, j].TileFrameX += 18;
			}
        }
		
        public override bool RightClick(int i, int j)
        {
			if (Main.tile[i, j].TileFrameX >= 18)
			{
                Main.tile[i, j].TileFrameX -= 18;
			}
            else
			{
                Main.tile[i, j].TileFrameX += 18;
			}
            return true;
        }
		
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<GreenMushroomCandleItem>();
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.TileFrameX < 18)
            {
                r = 0.15f;
				g = 0.35f;
				b = 0.30f;
            }
        }
	}
}