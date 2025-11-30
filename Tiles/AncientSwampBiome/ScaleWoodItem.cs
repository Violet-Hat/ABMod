using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Tiles.AncientSwampBiome
{
	public class ScaleWoodItem : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
			Item.DefaultToPlaceableTile(ModContent.TileType<ScaleWood>());
            Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.White;
        }

		public override void AddRecipes()
        {
			CreateRecipe()
            .AddIngredient(ModContent.ItemType<ScaleWoodWallItem>(), 4)
            .AddTile(TileID.WorkBenches)
            .Register();
			
			CreateRecipe()
            .AddIngredient(ModContent.ItemType<ScaleWoodFenceItem>(), 4)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
	}
}