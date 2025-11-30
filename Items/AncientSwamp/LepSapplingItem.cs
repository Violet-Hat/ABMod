using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Items.AncientSwamp
{
	public class LepSapplingItem : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
			Item.height = 28;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.maxStack = 9999;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.rare = ItemRarityID.White;
			Item.createTile = ModContent.TileType<Tiles.AncientSwampBiome.Trees.LepSappling>();
        }

		public override void AddRecipes()
        {
        }
	}
}