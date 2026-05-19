using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleSofaItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<ScaleSofa>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<ScaleWoodItem>(), 5)
			.AddIngredient(ItemID.Silk, 2)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}