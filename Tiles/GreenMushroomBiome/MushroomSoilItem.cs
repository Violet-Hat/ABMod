using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Tiles.GreenMushroomBiome
{
	public class MushroomSoilItem : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<MushroomSoil>());
            Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.White;
        }

		public override void AddRecipes()
        {
            CreateRecipe(25)
            .AddIngredient(ItemID.MudBlock, 25)
			.AddIngredient(ItemID.PoopBlock, 5)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
	}
}