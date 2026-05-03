using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleCandle : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.CoordinateHeights = [20];
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
			AddMapEntry(new Color(96, 109, 78), Lang.GetItemName(ItemID.Candle));
            RegisterItemDrop(ModContent.ItemType<ScaleCandleItem>());
			DustType = DustID.Bone;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AdjTiles = [TileID.Candles];
		}
		
		public override void HitWire(int i, int j)
        {
            //0 is ON, 18 is OFF
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
            Main.LocalPlayer.PickTile(i, j, 100);
            return true;
        }
		
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<ScaleCandleItem>();
            player.noThrow = 2;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            
            if (tile.TileFrameX < 18)
            {
                r = 0.45f;
				g = 0.35f;
				b = 0.10f;
            }
        }
	}
}