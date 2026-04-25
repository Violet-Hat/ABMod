using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp
{
    public class SwampMossWallItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<SwampMossWall>());
            Item.width = 16;
			Item.height = 16;
			Item.rare = ItemRarityID.White;
        }
    }
}