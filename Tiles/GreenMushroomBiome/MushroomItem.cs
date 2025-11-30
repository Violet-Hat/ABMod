using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

using ABMod.Tiles.GreenMushroomBiome.Furniture;

namespace ABMod.Tiles.GreenMushroomBiome
{
    public class MushroomItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
			Item.DefaultToPlaceableTile(ModContent.TileType<Mushroom>());
            Item.width = 22;
			Item.height = 24;
			Item.maxStack = 9999;
            Item.rare = ItemRarityID.White;
        }
		
		public override void AddRecipes()
        {
			CreateRecipe()
            .AddIngredient(ModContent.ItemType<GreenMushroomPlatformItem>(), 2)
            .Register();
        }
    }
}