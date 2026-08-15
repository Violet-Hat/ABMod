using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.NPCs.Swamp.Enemies
{
    public class Pulmonoscorpious_Walker : ModNPC
    {
        private const int ClonedAIStyle = NPCAIStyleID.Fighter;
        private const int ClonedNPCID = NPCID.GoblinScout;
        private const int ClonedAnimationType = NPCID.DesertScorpionWalk;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[ClonedAnimationType];
        }

        public override void SetDefaults()
		{
            NPC.width = 50;
			NPC.height = 20;
			NPC.damage = 21;
			NPC.defense = 10;
			NPC.lifeMax = 95;
			NPC.knockBackResist = 0.5f;
			NPC.value = Item.buyPrice(0, 0, 35, 50);
            NPC.HitSound = SoundID.NPCHit13;
			NPC.DeathSound = SoundID.NPCDeath19;
            NPC.aiStyle = ClonedAIStyle;
            AIType = ClonedNPCID;
            AnimationType = ClonedAnimationType;
		}

        public override void AI()
        {
            if (NPC.NPCCanStickToWalls())
            {
                NPC.Transform(ModContent.NPCType<Pulmonoscorpious_Crawler>());
            }
        }
    }

    public class Pulmonoscorpious_Crawler : ModNPC
    {
        private const int ClonedAnimationType = NPCID.DesertScorpionWall;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[ClonedAnimationType];
        }

        public override void SetDefaults()
		{
            NPC.width = 50;
			NPC.height = 20;
			NPC.damage = 21;
			NPC.defense = 10;
			NPC.lifeMax = 95;
			NPC.knockBackResist = 0.5f;
			NPC.value = Item.buyPrice(0, 0, 35, 50);
            NPC.HitSound = SoundID.NPCHit13;
			NPC.DeathSound = SoundID.NPCDeath19;
            NPC.noGravity = true;
            AnimationType = ClonedAnimationType;
		}

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest();
			}
			float num610 = 2f;
			float num611 = 0.8f;
			Vector2 vector77 = new(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
			float num612 = Main.player[NPC.target].position.X + Main.player[NPC.target].width / 2;
			float num613 = Main.player[NPC.target].position.Y + Main.player[NPC.target].height / 2;
			num612 = (int)(num612 / 8f) * 8;
			num613 = (int)(num613 / 8f) * 8;
			vector77.X = (int)(vector77.X / 8f) * 8;
			vector77.Y = (int)(vector77.Y / 8f) * 8;
			num612 -= vector77.X;
			num613 -= vector77.Y;
			if (NPC.confused)
			{
				num612 *= -2f;
				num613 *= -2f;
			}
			float num614 = (float)Math.Sqrt(num612 * num612 + num613 * num613);
            if (num614 == 0f)
			{
				num612 = NPC.velocity.X;
				num613 = NPC.velocity.Y;
			}
			else
			{
				num614 = num610 / num614;
				num612 *= num614;
				num613 *= num614;
			}
			if (Main.player[NPC.target].dead)
			{
				num612 = NPC.direction * num610 / 2f;
				num613 = (0f - num610) / 2f;
			}
			NPC.spriteDirection = -1;
			if (!Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
			{
				NPC.ai[0] += 1f;
				if (NPC.ai[0] > 0f)
				{
					NPC.velocity.Y += 0.023f;
				}
				else
				{
					NPC.velocity.Y -= 0.023f;
				}
				if (NPC.ai[0] < -100f || NPC.ai[0] > 100f)
				{
					NPC.velocity.X += 0.023f;
				}
				else
				{
					NPC.velocity.X -= 0.023f;
				}
				if (NPC.ai[0] > 200f)
				{
					NPC.ai[0] = -200f;
				}
				NPC.velocity.X += num612 * 0.007f;
				NPC.velocity.Y += num613 * 0.007f;
				NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
				if (NPC.velocity.X > 1.5)
				{
					NPC.velocity.X *= 0.9f;
				}
				if (NPC.velocity.X < -1.5)
				{
					NPC.velocity.X *= 0.9f;
				}
				if (NPC.velocity.Y > 1.5)
				{
					NPC.velocity.Y *= 0.9f;
				}
				if (NPC.velocity.Y < -1.5)
				{
					NPC.velocity.Y *= 0.9f;
				}
				if (NPC.velocity.X > 3f)
				{
					NPC.velocity.X = 3f;
				}
				if (NPC.velocity.X < -3f)
				{
					NPC.velocity.X = -3f;
				}
				if (NPC.velocity.Y > 3f)
				{
					NPC.velocity.Y = 3f;
				}
				if (NPC.velocity.Y < -3f)
				{
					NPC.velocity.Y = -3f;
				}
			}
			else
			{
				if (NPC.velocity.X < num612)
				{
					NPC.velocity.X += num611;
					if (NPC.velocity.X < 0f && num612 > 0f)
					{
						NPC.velocity.X += num611;
					}
				}
				else if (NPC.velocity.X > num612)
				{
					NPC.velocity.X -= num611;
					if (NPC.velocity.X > 0f && num612 < 0f)
					{
						NPC.velocity.X -= num611;
					}
				}
				if (NPC.velocity.Y < num613)
				{
					NPC.velocity.Y += num611;
					if (NPC.velocity.Y < 0f && num613 > 0f)
					{
						NPC.velocity.Y += num611;
					}
				}
				else if (NPC.velocity.Y > num613)
				{
					NPC.velocity.Y -= num611;
					if (NPC.velocity.Y > 0f && num613 < 0f)
					{
						NPC.velocity.Y -= num611;
					}
				}
				NPC.rotation = (float)Math.Atan2(num613, num612);
			}
			NPC.rotation += (float)Math.PI / 2f;
			float num616 = 0.5f;
			if (NPC.collideX)
			{
				NPC.netUpdate = true;
				NPC.velocity.X = NPC.oldVelocity.X * (0f - num616);
				if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
				{
					NPC.velocity.X = 2f;
				}
				if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
				{
					NPC.velocity.X = -2f;
				}
			}
			if (NPC.collideY)
			{
				NPC.netUpdate = true;
				NPC.velocity.Y = NPC.oldVelocity.Y * (0f - num616);
				if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1.5)
				{
					NPC.velocity.Y = 2f;
				}
				if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1.5)
				{
					NPC.velocity.Y = -2f;
				}
			}
			if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit)
			{
				NPC.netUpdate = true;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && !NPC.NPCCanStickToWalls())
            {
                NPC.Transform(ModContent.NPCType<Pulmonoscorpious_Walker>());
            }
        }
    }
}