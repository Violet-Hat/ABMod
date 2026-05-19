using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp.Traps
{
    public class ScaleSpikesItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<ScaleSpikes>());
            Item.width = 24;
			Item.height = 40;
        }

        public override void AddRecipes()
		{
			CreateRecipe(2)
			.AddIngredient(ModContent.ItemType<ScaleWoodItem>())
			.AddTile(TileID.WorkBenches)
            .AddCondition(Condition.InGraveyard)
			.Register();
		}
    }
}