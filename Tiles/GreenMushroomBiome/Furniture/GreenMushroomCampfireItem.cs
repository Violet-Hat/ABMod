using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Tiles.GreenMushroomBiome.Furniture
{
	public class GreenMushroomCampfireItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<GreenMushroomCampfire>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<MushroomItem>(), 10)
			.AddIngredient(ItemID.Torch, 5)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}