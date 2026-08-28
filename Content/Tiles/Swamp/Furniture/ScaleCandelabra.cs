using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleCandelabra : ModTile
	{
		public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(96, 109, 78), Lang.GetItemName(ItemID.Candelabra));
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			RegisterItemDrop(ModContent.ItemType<ScaleCandelabraItem>());
            DustType = DustID.Bone;
			AdjTiles = [TileID.Candelabras];
		}
		
		public override void HitWire(int i, int j)
        {
            //Get the top left of the frame
            int left = i - Framing.GetTileSafely(i, j).TileFrameX / 18 % 2;
            int top = j - Framing.GetTileSafely(i, j).TileFrameY / 18 % 2;
            
            for (int x = left; x < left + 2; x++)
            {
                for (int y = top; y < top + 2; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    
                    //0 to 18 are ON frames, 36 to 54 are OFF frames
                    if (tile.TileFrameX >= 36)
                    {
                        tile.TileFrameX -= 36;
                    }
                    else
                    {
                        tile.TileFrameX += 36;
                    }

                    Wiring.SkipWire(x, y);
                }
            }
            
            //Net shenanigans
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
				NetMessage.SendTileSquare(-1, left, top, 2, 2);
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
	}
}	