using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ABMod.Content.Backgrounds.Swamp
{
    public class SwampSurfaceBGStyle : ModSurfaceBackgroundStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (var i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                }
                else
                {
                    fades[i] -= transitionSpeed;
                }

                fades[i] = MathHelper.Clamp(fades[i], 0f, 1f);
            }
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            scale = 0.75f;
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, "Content/Backgrounds/Swamp/SwampSurfaceBG1");
        }

        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, "Content/Backgrounds/Swamp/SwampSurfaceBG2");
        }

        public override int ChooseFarTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, "Content/Backgrounds/Swamp/SwampSurfaceBG3");
        }
    }
}