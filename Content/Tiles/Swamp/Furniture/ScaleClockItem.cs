using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleClockItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<ScaleClock>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<ScaleWoodItem>(), 10)
			.AddRecipeGroup(RecipeGroupID.IronBar, 3)
			.AddIngredient(ItemID.Glass, 6)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}