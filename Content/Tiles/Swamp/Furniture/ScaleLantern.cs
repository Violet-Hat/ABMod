using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ObjectData;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleLantern : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = -2;
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.addTile(Type);
			AddMapEntry(new Color(96, 109, 78), Language.GetText("MapObject.Lantern"));
			DustType = DustID.Bone;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AdjTiles = [TileID.HangingLanterns];
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void HitWire(int i, int j)
        {
            int left = i - Framing.GetTileSafely(i, j).TileFrameX / 18 % 1;
            int top = j - Framing.GetTileSafely(i, j).TileFrameY / 18 % 2;

            for (int x = left; x < left + 1; x++)
            {
                for (int y = top; y < top + 2; y++)
                {
                    if (Framing.GetTileSafely(x, y).TileFrameX >= 18)
                    {
                        Framing.GetTileSafely(x, y).TileFrameX -= 18;
                    }
                    else
                    {
                        Framing.GetTileSafely(x, y).TileFrameX += 18;
                    }

                    Wiring.SkipWire(x, y);
                }
            }

            //Net shenanigans
            NetMessage.SendTileSquare(-1, left, top, 1, 2);
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