using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.AncientSwampBiome;

namespace ABMod.Tiles.AncientSwampBiome.Furniture
{
	public class ScaleBathtubItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<ScaleBathtub>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<ScaleWoodItem>(), 14)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}