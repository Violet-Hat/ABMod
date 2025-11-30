using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Items;
using ABMod.Buffs;

namespace ABMod.Items.GreenMushroom
{ 
	public class FungusPick : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 6;
            		Item.DamageType = DamageClass.Melee;
            		Item.autoReuse = true;
            		Item.width = 34;
            		Item.height = 34;
            		Item.useTime = 8;
            		Item.useAnimation = 20;
            		Item.useStyle = ItemUseStyleID.Swing;
					Item.useTurn = true;
            		Item.knockBack = 4;
            		Item.rare = ItemRarityID.Blue;
            		Item.value = Item.buyPrice(silver: 2);
            		Item.UseSound = SoundID.Item1;
			Item.pick = 45;
			Item.attackSpeedOnlyAffectsWeaponAnimation = true;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			player.AddBuff(ModContent.BuffType<Mushy>(), 300);
		}
	}
}
