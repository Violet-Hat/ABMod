using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Tiles.GreenMushroomBiome.Furniture
{
	public class GreenMushroomWorkbenchItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<GreenMushroomWorkbench>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<MushroomItem>(), 10)
			.Register();
		}
	}
}