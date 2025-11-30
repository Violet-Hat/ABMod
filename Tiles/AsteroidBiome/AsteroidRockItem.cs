using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Tiles.AsteroidBiome
{
	public class AsteroidRockItem : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<AsteroidRock>());
            Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.White;
        }

		public override void AddRecipes()
        {
			CreateRecipe()
            .AddIngredient(ModContent.ItemType<AsteroidRockWallItem>(), 4)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
	}
}