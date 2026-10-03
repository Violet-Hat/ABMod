using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent.Metadata;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ABMod.Common.Tiles;

namespace ABMod.Common.Bases
{
    public class CustomSapling : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

            TileID.Sets.SwaysInWindBasic[Type] = true;
            TileID.Sets.CommonSapling[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateHeights = [16, 18];
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorValidTiles = [TileID.Grass];
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawFlipHorizontal = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleMultiplier = 3;
            TileObjectData.addTile(Type);
            
            AddMapEntry(new(141, 107, 75), Language.GetText("MapObject.Sapling"));
            DustType = DustID.Grass;
            AdjTiles = [TileID.Saplings];
		}

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override void RandomUpdate(int i, int j)
        {
            if (WorldGen.genRand.NextBool())
            {
                Tile tile = Framing.GetTileSafely(i, j);

                //The tree will grow from the bottom of this sappling
                if (tile.TileFrameY == 18)
                {
					if(TreeGrowHelper.IsValidGrowingSpot(i, j, TileID.Grass))
					{
						CustomTree.Grow(i, j, 15, 20, true);
					}
                }
            }
        }
    }
}