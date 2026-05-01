using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ABMod.Content.Tiles.Swamp.Ambient
{
    //Base used by the fake and natural ambient tiles
    public class MediumAmbientPlantBase : ModTile
    {
        //Both ambient tiles will have the same texture
        public override string Texture => "ABMod/Content/Tiles/Swamp/Ambient/MediumAmbientPlant";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(80, 115, 54));
            HitSound = SoundID.Grass;
            DustType = DustID.Grass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1: 3;
        }
	}

    //Placed by rubblemaker
    public class MediumAmbientPlantFake : MediumAmbientPlantBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            FlexibleTileWand.RubblePlacementMedium.AddVariations(ModContent.ItemType<SwampMossSpores>(), Type, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            RegisterItemDrop(ModContent.ItemType<SwampMossSpores>());
        }
    }

    //Natural
    public class MediumAmbientPlantNatural : MediumAmbientPlantBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            TileID.Sets.BreakableWhenPlacing[Type] = true;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
        }
    }
}