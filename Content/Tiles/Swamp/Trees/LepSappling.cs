using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

using ABMod.Common.Tiles;

namespace ABMod.Content.Tiles.Swamp.Trees
{
    public class LepSappling : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
            TileID.Sets.CommonSapling[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<SwampMoss>()];
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(96, 109, 78), name);
            DustType = DustID.Bone;
            AdjTiles = [TileID.Saplings];
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (WorldGen.genRand.NextBool())
            {
                Tile tile = Framing.GetTileSafely(i, j);

                //The tree will grow from the bottom left of this sappling
                if (tile.TileFrameX == 0 && tile.TileFrameY == 18)
                {
					if(TreeTile.GrowTreeCheck(i, j + 1, 6, 35))
					{
						Lep.Grow(i, j, 25, 30, true);
					}
                }
            }
        }
    }
}