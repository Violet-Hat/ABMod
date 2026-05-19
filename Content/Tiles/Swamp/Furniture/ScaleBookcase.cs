using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleBookcase : ModTile
    {
		public override void SetStaticDefaults()
		{
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.addTile(Type);

			AddMapEntry(new Color(96, 109, 78), Lang.GetItemName(ItemID.Bookcase));
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			DustType = DustID.Bone;
            AdjTiles = [TileID.Bookcases];
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
	}
}