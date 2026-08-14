using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.NPCs.Swamp.Critters
{
    public class Cockroach : ModNPC
    {
        private const int ClonedNPCID = NPCID.Stinkbug;

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
            NPC.width = 26;
            NPC.height = 20;
            
            AIType = ClonedNPCID;
            AnimationType = ClonedNPCID;
		}
    }
}