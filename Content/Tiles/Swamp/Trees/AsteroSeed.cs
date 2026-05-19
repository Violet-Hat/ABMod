using Terraria;
using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp.Trees
{
    public class AsteroSeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<AsteroSappling>());
            Item.width = 16;
            Item.height = 16;
        }
    }
}