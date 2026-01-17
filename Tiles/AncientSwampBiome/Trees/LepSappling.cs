using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

using ABMod.Generation;

namespace ABMod.Tiles.AncientSwampBiome.Trees
{
    public class LepSappling : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.CommonSapling[Type] = true;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateHeights = [16, 16];
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<PrehistoricMoss>()];
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(66, 98, 35), name);
            DustType = DustID.Bone;
            AdjTiles = [TileID.Saplings];
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (WorldGen.genRand.NextBool(15))
            {
                if (Main.tile[i, j + 1].TileType != ModContent.TileType<LepSappling>() && Main.tile[i + 1, j].TileType == ModContent.TileType<LepSappling>())
                {
					if(WorldgenTools.GrowTreeCheck(i, j + 1, 6, 35, 1))
					{
						Lep.Grow(i, j, 25, 30, true);
					}
                }
            }
        }
	}
}