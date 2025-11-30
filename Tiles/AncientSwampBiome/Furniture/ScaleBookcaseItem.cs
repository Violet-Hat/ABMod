using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.AncientSwampBiome;

namespace ABMod.Tiles.AncientSwampBiome.Furniture
{
	public class ScaleBookcaseItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<ScaleBookcase>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<ScaleWoodItem>(), 20)
			.AddIngredient(ItemID.Book, 10)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}