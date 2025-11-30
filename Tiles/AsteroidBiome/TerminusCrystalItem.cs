using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Tiles.AsteroidBiome
{
	public class TerminusCrystalItem : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<TerminusCrystal>());
            Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Green;
        }

		public override void AddRecipes()
        {
			CreateRecipe()
            .AddIngredient(ModContent.ItemType<TerminusCrystalWallItem>(), 4)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
	}
}