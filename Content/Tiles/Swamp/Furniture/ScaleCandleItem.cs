using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleCandleItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<ScaleCandle>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<ScaleWoodItem>(), 4)
			.AddIngredient(ItemID.Torch, 1)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}