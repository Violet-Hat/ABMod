using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Tiles.AncientSwampBiome
{
    public class AncientStoneWallItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<AncientStoneWall>());
            Item.width = 16;
			Item.height = 16;
			Item.rare = ItemRarityID.White;
        }
		
		public override void AddRecipes()
        {
            CreateRecipe(4)
            .AddIngredient(ModContent.ItemType<AncientStoneItem>())
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}