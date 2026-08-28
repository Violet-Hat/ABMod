using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.NPCs.Swamp.Critters
{
    public class Caddisfly : ModNPC
    {
        private const int ClonedAIStyle = NPCAIStyleID.Dragonfly;
        private const int ClonedNPCID = NPCID.RedDragonfly;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[ClonedNPCID];

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;
        }

        public override void SetDefaults()
		{
            NPC.CloneDefaults(ClonedNPCID);
            NPC.width = 30;
            NPC.height = 20;
            NPC.aiStyle = ClonedAIStyle;
            AIType = ClonedNPCID;
            AnimationType = ClonedNPCID;
		}

        public override void AI()
        {
            NPC.rotation = NPC.velocity.X * 0.05f;
        }
    }
}