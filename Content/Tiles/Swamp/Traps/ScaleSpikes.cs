using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using ABMod.Content.Dusts;

namespace ABMod.Content.Tiles.Swamp.Traps
{
    public class ScaleSpikes : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileID.Sets.TouchDamageImmediate[Type] = 20;
            TileID.Sets.TouchDamageBleeding[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawFlipHorizontal = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleMultiplier = 3;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            
            DustType = DustID.Bone;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
        {
            if (!visible)
                return;

            bool isPlayerNear = WorldGen.PlayerLOS(i, j);

            if (isPlayerNear)
            {
                if (Main.rand.NextBool(250))
                {
                    Vector2 vel = new(0f, -0.75f);
                    int newDust = Dust.NewDust(new Vector2(i * 16, j * 16), 14, 14, ModContent.DustType<SkullParticle>());
                    Main.dust[newDust].velocity = vel;
                    Main.dust[newDust].alpha = 25;
                }
            }
        }
    }
}