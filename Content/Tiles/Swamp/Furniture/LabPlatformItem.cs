using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class LabPlatformItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<LabPlatform>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(2)
			.AddIngredient(ModContent.ItemType<LabBlockItem>())
			.Register();
		}
	}
}