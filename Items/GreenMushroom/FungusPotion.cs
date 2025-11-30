using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Items.GreenMushroom
{
    public class FungusPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = 9999;
			Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(silver: 25);
			Item.healLife = 100;
			Item.buffType = ModContent.BuffType<Buffs.Mushy>();
			Item.buffTime = 600;
			Item.potion = true;
        }

		public override void GetHealLife(Player player, bool quickHeal, ref int healValue)
		{
			healValue = 100;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.LesserHealingPotion, 2)
			.AddIngredient(ModContent.ItemType<MushroomItem>(), 5)
			.AddTile(TileID.Bottles)
			.Register();
		}
    }
}