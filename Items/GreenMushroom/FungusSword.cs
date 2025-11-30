using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ABMod.Items.GreenMushroom;
using ABMod.Buffs;

namespace ABMod.Items.GreenMushroom
{ 
	public class FungusSword : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Melee;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.knockBack = 5;
			Item.value = Item.buyPrice(silver: 2);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<EvilMushy>(), 300);
		}
	}
}
