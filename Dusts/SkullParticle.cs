using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Dusts
{
    public class SkullParticle : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale *= 0.75f;
            dust.frame = new Rectangle(0, 0, 14, 14);
        }

        public override bool Update(Dust dust)
        {
            dust.velocity *= 0.975f;
            dust.alpha += 3;

            dust.position += dust.velocity;

            if (dust.alpha >= 255)
            {
                dust.active = false;
            }

            return false;
        }
    }
}