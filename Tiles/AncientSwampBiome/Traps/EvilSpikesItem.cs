using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Tiles.AncientSwampBiome.Traps
{
	public class EvilSpikesItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 50;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<EvilSpikes>());
			Item.width = 24;
			Item.height = 40;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.White;
		}
		
		public override void AddRecipes()
		{
		}
	}
}