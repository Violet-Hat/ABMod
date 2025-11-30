using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Tiles.AsteroidBiome;

namespace ABMod.Tiles.Decos.Blocks
{
	public class HardenedMeteoriteBrickItem : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<HardenedMeteoriteBrick>());
            Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.White;
        }

		public override void AddRecipes()
        {
			CreateRecipe()
            .AddIngredient(ModContent.ItemType<AsteroidRockItem>(), 2)
            .AddTile(TileID.Furnaces)
            .Register();
        }
	}
}