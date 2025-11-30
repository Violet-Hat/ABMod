using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.Enums;

namespace ABMod.Tiles.AncientSwampBiome.Furniture
{
	public class ScaleCandle : ModTile
	{
		private Asset<Texture2D> BulbTexture;
		
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 18 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(96, 109, 78), name);
			DustType = DustID.Bone;
            RegisterItemDrop(ModContent.ItemType<ScaleCandleItem>());
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
            player.cursorItemIconID = ModContent.ItemType<ScaleCandleItem>();
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
		
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
			BulbTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/AncientSwampBiome/Furniture/ScaleCandle_Bulb");
			
			Tile tile = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);
			
			Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
			spriteBatch.Draw(BulbTexture.Value, new Vector2(i * 16, j * 16 - 4) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, 0, 18, 20), new Color(col.R, col.G, col.B, 255));
		}
	}
}