using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Tiles.GreenMushroomBiome
{
	public class MushroomStemItem : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<MushroomStem>());
            Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.White;
        }

		public override void AddRecipes()
        {
            CreateRecipe(5)
			.AddIngredient(ModContent.ItemType<MushroomItem>(), 5)
            .AddTile(TileID.Sawmill)
            .Register();
        }
	}
}