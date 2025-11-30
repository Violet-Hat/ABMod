using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Tiles.GreenMushroomBiome.Furniture
{
	public class GreenMushroomCandelabraItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<GreenMushroomCandelabra>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<MushroomItem>(), 5)
			.AddIngredient(ItemID.Torch, 3)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}