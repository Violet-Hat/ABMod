using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace ABMod.Content.Tiles.Swamp.Ambient
{
    //Base used by the fake and natural ambient tiles
    public class SmallSwampDirtRockBase : ModTile
    {
        //Both ambient tiles will have the same textures
        public override string Texture => "ABMod/Content/Tiles/Swamp/Ambient/SmallSwampDirtRock";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(66, 60, 49));
            HitSound = SoundID.Tink;
            DustType = DustID.Stone;
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if((i % 10) < 5)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }

    //Placed by rubblemaker
    public class SmallSwampDirtRockFake : SmallSwampDirtRockBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            FlexibleTileWand.RubblePlacementSmall.AddVariations(ModContent.ItemType<SwampDirtItem>(), Type, 0, 1, 2);
            RegisterItemDrop(ModContent.ItemType<SwampDirtItem>());
        }
    }

    //Natural
    public class SmallSwampDirtRockNatural : SmallSwampDirtRockBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileID.Sets.BreakableWhenPlacing[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileObjectData.GetTileData(Type, 0).LavaDeath = false;
        }
    }
}