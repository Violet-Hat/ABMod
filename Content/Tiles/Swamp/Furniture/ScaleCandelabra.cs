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
			RegisterItemDrop(ModContent.ItemType<ScaleCandelabraItem>());
            DustType = DustID.Bone;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AdjTiles = new int[] { TileID.Candelabras };
		}
		
		public override void HitWire(int i, int j)
        {
            //Get the top left of the frame
            int left = i - Main.tile[i, j].TileFrameX / 18 % 2;
            int top = j - Main.tile[i, j].TileFrameY / 18 % 2;
            
            for (int x = left; x < left + 2; x++)
            {
                for (int y = top; y < top + 2; y++)
                {
                    //0 to 18 are ON frames, 36 to 54 are OFF frames
                    if (Main.tile[x, y].TileFrameX >= 36)
                    {
                        Main.tile[x, y].TileFrameX -= 36;
                    }
                    else
                    {
                        Main.tile[x, y].TileFrameX += 36;
                    }

                    Wiring.SkipWire(x, y);
                }
            }
            
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
				NetMessage.SendTileSquare(-1, left, top + 1, 2);
			}
        }
		
		public override void NumDust(int i, int j, bool fail, ref int num) 
        {
            num = fail ? 1 : 3;
        }

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