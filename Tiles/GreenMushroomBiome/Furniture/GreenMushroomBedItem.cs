using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Tiles.GreenMushroomBiome.Furniture
{
	public class GreenMushroomBedItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<GreenMushroomBed>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<MushroomItem>(), 15)
			.AddIngredient(ItemID.Silk, 5)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}