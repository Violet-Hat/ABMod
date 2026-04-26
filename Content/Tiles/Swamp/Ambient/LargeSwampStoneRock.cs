using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ABMod.Content.Tiles.Swamp.Ambient
{
    //Base used by the fake and natural ambient tiles
    public class LargeSwampStoneRockBase : ModTile
    {
        //Both ambient tiles will have the same texture
        public override string Texture => "ABMod/Content/Tiles/Swamp/Ambient/LargeSwampStoneRock";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileObsidianKill[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(56, 49, 41));
            HitSound = SoundID.Tink;
            DustType = DustID.Stone;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1: 3;
        }
    }

    //Placed by rubblemaker
    public class LargeSwampStoneRockFake : LargeSwampStoneRockBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            FlexibleTileWand.RubblePlacementLarge.AddVariations(ModContent.ItemType<SwampStoneItem>(), Type, 0, 1, 2);
            RegisterItemDrop(ModContent.ItemType<SwampStoneItem>());
        }
    }

    //Natural
    public class LargeSwampStoneRockNatural : LargeSwampStoneRockBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            TileID.Sets.BreakableWhenPlacing[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileObjectData.newTile.AnchorValidTiles = new int[] {  ModContent.TileType<SwampStone>() };
            TileObjectData.GetTileData(Type, 0).LavaDeath = false;
        }

        public override void DropCritterChance(int i, int j, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance)
        {
            wormChance = 6;
		}
    }
}