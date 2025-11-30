using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AsteroidBiome
{
    public class AsteroidRockWallItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<AsteroidRockWall>());
            Item.width = 16;
			Item.height = 16;
			Item.rare = ItemRarityID.White;
        }
		
		public override void AddRecipes()
        {
            CreateRecipe(4)
            .AddIngredient(ModContent.ItemType<AsteroidRockItem>())
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}