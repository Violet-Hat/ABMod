using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ABMod.Buffs
{
	public class EvilMushy : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex) {
			npc.GetGlobalNPC<EvilMushyNPCDebuff>().evilMushy = true;
		}

        public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 7;
		}
	}

	public class EvilMushyNPCDebuff : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool evilMushy;

		public override void ResetEffects(NPC npc)
		{
			evilMushy = false;
		}

		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
		{
			if(evilMushy)
			{
				modifiers.Defense -= 7;
			}
		}
	}
}