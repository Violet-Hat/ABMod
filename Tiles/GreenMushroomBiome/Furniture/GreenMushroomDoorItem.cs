using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Tiles.GreenMushroomBiome.Furniture
{
	public class GreenMushroomDoorItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<GreenMushroomDoorClosed>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<MushroomItem>(), 6)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}