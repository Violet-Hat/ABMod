using Terraria;
using Terraria.ModLoader;

namespace ABMod.Tiles.AncientSwampBiome.Trees
{
    public class SkinnySeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SkinnySappling>());
            Item.width = 16;
            Item.height = 16;
        }
    }
}