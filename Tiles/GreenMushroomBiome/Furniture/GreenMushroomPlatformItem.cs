using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Tiles.GreenMushroomBiome.Furniture
{
	public class GreenMushroomPlatformItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<GreenMushroomPlatform>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(2)
			.AddIngredient(ModContent.ItemType<MushroomItem>())
			.Register();
		}
	}
}