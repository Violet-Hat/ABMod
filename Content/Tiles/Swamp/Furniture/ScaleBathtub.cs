using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleBathtub : ModTile
    {
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(96, 109, 78), Lang.GetItemName(ItemID.Bathtub));
			DustType = DustID.Bone;
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			AdjTiles = new int[] { TileID.Bathtubs };
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
	}
}