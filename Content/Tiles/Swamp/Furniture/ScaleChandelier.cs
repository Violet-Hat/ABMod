using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Enums;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleChandelier : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.MultiTileSway[Type] = true;
			TileID.Sets.IsAMechanism[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.DrawYOffset = -2;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
			AddMapEntry(new Color(96, 109, 78), Language.GetText("MapObject.Chandelier"));
			DustType = DustID.Bone;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AdjTiles = [TileID.Chandeliers];
		}
		
		public override void HitWire(int i, int j)
        {
            //Get the top left of the frame
            int left = i - Framing.GetTileSafely(i, j).TileFrameX / 18 % 3;
            int top = j - Framing.GetTileSafely(i, j).TileFrameY / 18 % 3;

            for (int x = left; x < left + 3; x++)
            {
                for (int y = top; y < top + 3; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);

                    //0 to 36 are ON frames, 54 to 90 are OFF frames
                    if (tile.TileFrameX >= 54)
                    {
                        tile.TileFrameX -= 54;
                    }
                    else
                    {
                        tile.TileFrameX += 54;
                    }

                    Wiring.SkipWire(x, y);
                }
            }

            //Net shenanigans
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
				NetMessage.SendTileSquare(-1, left, top, 3, 3);
			}
        }
		
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.TileFrameX < 36)
            {
				r = 0.45f;
				g = 0.35f;
				b = 0.10f;
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
			Tile tile = Framing.GetTileSafely(i, j);

			if (TileObjectData.IsTopLeft(tile))
            {
				// Makes this tile sway in the wind and with player interaction when used with TileID.Sets.MultiTileSway
				Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.MultiTileVine);
			}

			return false;
		}

        public override void AdjustMultiTileVineParameters(int i, int j, ref float? overrideWindCycle, ref float windPushPowerX, ref float windPushPowerY, ref bool dontRotateTopTiles, ref float totalWindMultiplier, ref Texture2D glowTexture, ref Color glowColor)
        {
            // Vanilla chandeliers all share these parameters.
			overrideWindCycle = 1f;
			windPushPowerY = 0;
        }
	}
}