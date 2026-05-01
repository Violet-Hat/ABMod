using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ABMod.Content.Tiles.Swamp.Ambient
{
    //Base used by the fake and natural ambient tiles
    public class SmallSwampStoneRockBase : ModTile
    {
        //Both ambient tiles will have the same texture
        public override string Texture => "ABMod/Content/Tiles/Swamp/Ambient/SmallSwampStoneRock";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(56, 49, 41));
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

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1: 3;
        }
    }

    //Placed by rubblemaker
    public class SmallSwampStoneRockFake : SmallSwampStoneRockBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            FlexibleTileWand.RubblePlacementSmall.AddVariations(ModContent.ItemType<SwampStoneItem>(), Type, 0, 1, 2);
            RegisterItemDrop(ModContent.ItemType<SwampStoneItem>());
        }
    }

    //Natural
    public class SmallSwampStoneRockNatural : SmallSwampStoneRockBase
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