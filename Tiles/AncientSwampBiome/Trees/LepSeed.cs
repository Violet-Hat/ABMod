using Terraria;
using Terraria.ModLoader;

namespace ABMod.Tiles.AncientSwampBiome.Trees
{
    public class LepSeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<LepSappling>());
            Item.width = 16;
            Item.height = 16;
        }
    }
}