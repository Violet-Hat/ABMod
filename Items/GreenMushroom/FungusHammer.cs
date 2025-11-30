using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Items;
using ABMod.Buffs;

namespace ABMod.Items.GreenMushroom
{ 
	public class FungusHammer : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 9;
            		Item.hammer = 45;
            		Item.DamageType = DamageClass.Melee;
            		Item.autoReuse = true;
            		Item.width = 38;
            		Item.height = 38;
            		Item.useTime = 15;
            		Item.useAnimation = 20;
            		Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
            		Item.knockBack = 5;
            		Item.rare = ItemRarityID.Blue;
            		Item.value = Item.buyPrice(silver: 2);
            		Item.UseSound = SoundID.Item1;
			Item.attackSpeedOnlyAffectsWeaponAnimation = true;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			player.AddBuff(ModContent.BuffType<Mushy>(), 300);
		}
	}
}
