using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

using ABMod.Common.Tiles;
using Terraria.GameContent;

namespace ABMod.Content.Tiles.Swamp.Ambient
{
    //Base used by the fake and natural ambient tiles
    public class CycadBase : ModTile
    {
        //Both ambient tiles will have the same textures
        public override string Texture => "ABMod/Content/Tiles/Swamp/Ambient/Cycad";
        public Asset<Texture2D> FoliageTexture;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(80, 115, 54));
            HitSound = SoundID.Dig;
            DustType = DustID.GreenMoss;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1: 3;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            FoliageTexture ??= ModContent.Request<Texture2D>(Texture + "_Full");

            //Get the tile, color and offsets
            Tile tile = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);

            Vector2 pos = TileGlobal.TileCustomPosition(i, j);
            Vector2 offset = new Vector2(40, 74);

            //Draw the full sprite
            int frame = tile.TileFrameX / 18;
            Main.spriteBatch.Draw(FoliageTexture.Value, pos, new Rectangle(frame * 98, 0, 96, 92), new Color(col.R, col.G, col.B, 255), 0f, offset, 1f, SpriteEffects.None, 0f);

            return false;
        }
    }

    //Placed by rubblemaker
    public class CycadFake : CycadBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            FlexibleTileWand.RubblePlacementSmall.AddVariations(ModContent.ItemType<SwampMossSpores>(), Type, 0, 1, 2);
            RegisterItemDrop(ModContent.ItemType<SwampMossSpores>());
        }
    }

    //Natural
    public class CycadNatural : CycadBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            TileID.Sets.BreakableWhenPlacing[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileObjectData.GetTileData(Type, 0).RandomStyleRange = 3;
            TileObjectData.GetTileData(Type, 0).AnchorValidTiles = new int[] {  ModContent.TileType<SwampMoss>() };
        }
    }
}