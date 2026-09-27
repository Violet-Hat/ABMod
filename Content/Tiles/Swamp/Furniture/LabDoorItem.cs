using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class LabDoorItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.DefaultToPlaceableTile(ModContent.TileType<LabDoorClosed>());
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<LabBlockItem>(), 6)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}