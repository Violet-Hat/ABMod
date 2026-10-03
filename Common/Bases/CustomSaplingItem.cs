using Terraria;
using Terraria.ModLoader;

namespace ABMod.Common.Bases
{
    public class CustomSaplingItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CustomSapling>());
            Item.width = 16;
            Item.height = 30;
        }
    }
}