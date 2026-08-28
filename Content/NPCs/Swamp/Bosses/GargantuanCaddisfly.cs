using System.Diagnostics;
using ABMod.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Content.NPCs.Swamp.Bosses
{
    [AutoloadBossHead]
    public class GargantuanCaddisfly : ModNPC
    {
        public int SpeedX = 0;
        public int SpeedY = 0;
        public int MaxSpeedX = 8;
        public int MaxSpeedY = 2;

        public int BossOffsetY = 250;

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 280;
            NPC.height = 214;
            NPC.lifeMax = 3400;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.npcSlots = 8f;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.value = Item.buyPrice(0, 0, 55, 30);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath16;
            Music = MusicID.Boss4;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 8)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 2)
            {
                NPC.frame.Y = 0 * frameHeight;
            }
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            NPC.spriteDirection = NPC.direction;

            //Make it tilt slightly
            NPC.rotation = NPC.velocity.X * 0.025f;
            
            switch(AIState)
            {
                case (int)GargantuanCaddisflyStates.Hovering:
                    Hover(player);
                    break;
                case (int)GargantuanCaddisflyStates.Spitting:
                    //Spit(player);
                    break;
                case (int)GargantuanCaddisflyStates.Slashing:
                    //Slash(player);
                    break;
                case (int)GargantuanCaddisflyStates.Dashing:
                    //Dash(player);
                    break;
                case (int)GargantuanCaddisflyStates.Roaring:
                    //Roar(player);
                    break;
            }
        }

        private void Hover(Player player)
        {
            Vector2 goTo = new(player.Center.X, player.Center.Y - BossOffsetY);

            if (NPC.Center.X >= goTo.X && SpeedX >= -MaxSpeedX) 
            {
                SpeedX--;
            }
            else if (NPC.Center.X <= goTo.X && SpeedX <= MaxSpeedX) 
            {
                SpeedX++;
            }

            NPC.velocity.X += SpeedX * 0.015f;
            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -MaxSpeedX, MaxSpeedX);

            if (NPC.Center.Y >= goTo.Y && SpeedY >= -MaxSpeedY) 
            {
                SpeedY--;
            }
            else if (NPC.Center.Y <= goTo.Y && SpeedY <= MaxSpeedY) 
            {
                SpeedY++;
            }

            NPC.velocity.Y += SpeedY * 0.05f;
            NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -MaxSpeedY, MaxSpeedY);
        }
    }
}