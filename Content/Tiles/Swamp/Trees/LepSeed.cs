using Terraria;
using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp.Trees
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
            Item.width = 18;
            Item.height = 28;
        }
    }
}