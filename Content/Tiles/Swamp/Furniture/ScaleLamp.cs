using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ObjectData;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleLamp : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileWaterDeath[Type] = true;
			Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.StyleLineSkip = 2;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addTile(Type);
            AddMapEntry(new Color(96, 109, 78), Language.GetText("MapObject.FloorLamp"));
            DustType = DustID.Bone;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AdjTiles = [TileID.Lamps];
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void HitWire(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
			int topY = j - tile.TileFrameY / 18 % 3;
			short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);

            Framing.GetTileSafely(i, topY).TileFrameX += frameAdjustment;
			Framing.GetTileSafely(i, topY + 1).TileFrameX += frameAdjustment;
			Framing.GetTileSafely(i, topY + 2).TileFrameX += frameAdjustment;

			Wiring.SkipWire(i, topY);
			Wiring.SkipWire(i, topY + 1);
			Wiring.SkipWire(i, topY + 2);

			if (Main.netMode != NetmodeID.SinglePlayer)
            {
				NetMessage.SendTileSquare(-1, i, topY, 1, 3);
			}
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
			if (i % 2 == 0)
            {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
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