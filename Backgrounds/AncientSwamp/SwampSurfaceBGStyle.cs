using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using System.Reflection;

namespace ABMod.Backgrounds.AncientSwamp
{
    public class SwampSurfaceBGStyle : ModSurfaceBackgroundStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++)
			{
				if (i == Slot)
				{
					fades[i] += transitionSpeed;
					if (fades[i] > 1f)
					{
						fades[i] = 1f;
					}
				}
				else
				{
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f)
					{
						fades[i] = 0f;
					}
				}
			}
		}

		public override int ChooseFarTexture()
		{
			return -1;
		}

		public override int ChooseMiddleTexture()
		{
			return -1;
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			return -1;
		}

        public override bool PreDrawCloseBackground(SpriteBatch spriteBatch)
        {
            FieldInfo screenOffField = typeof(Main).GetField("screenOff", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo scAdjField = typeof(Main).GetField("scAdj", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo BGColorModifiedField = typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Static | BindingFlags.NonPublic);

            float screenOff = (float)screenOffField.GetValue(Main.instance);
            float scAdj = (float)scAdjField.GetValue(Main.instance);
            Color BGColorModified = (Color)BGColorModifiedField.GetValue(null);

            int[] textureSlots = new int[]
            {
                BackgroundTextureLoader.GetBackgroundSlot("ABMod/Backgrounds/AncientSwamp/SwampSurfaceBG_Far"),
                BackgroundTextureLoader.GetBackgroundSlot("ABMod/Backgrounds/AncientSwamp/SwampSurfaceBG_Mid"),
                BackgroundTextureLoader.GetBackgroundSlot("ABMod/Backgrounds/AncientSwamp/SwampSurfaceBG_Close"),
            };

            Color BGActualColor = new Color(BGColorModified.R, BGColorModified.G, BGColorModified.B, BGColorModified.A);

            bool canBGDraw = false;

            if ((!Main.remixWorld || (Main.gameMenu && !WorldGen.remixWorldGen)) && (!WorldGen.remixWorldGen || !WorldGen.drunkWorldGen))
            {
                canBGDraw = true;
            }
            if (Main.mapFullscreen)
            {
                canBGDraw = false;
            }
            int offset = 30;
            if (Main.gameMenu)
            {
                offset = 0;
            }
            if (WorldGen.drunkWorldGen)
            {
                offset = -180;
            }

            float surfacePosition = (float)Main.worldSurface;
            if (surfacePosition == 0f)
            {
                surfacePosition = 1f;
            }

            float screenPosition = Main.screenPosition.Y + (float)(Main.screenHeight / 2) - 600f;
            double backgroundTopMagicNumber = (0f - screenPosition + screenOff / 2f) / (surfacePosition * 16f);
            float bgGlobalScaleMultiplier = 2f;
            int pushBGTopHack;
            int offset2 = -180;

            int menuOffset = 0;
            if (Main.gameMenu)
            {
                menuOffset -= offset2;
            }

            pushBGTopHack = menuOffset;
            pushBGTopHack += offset;
            pushBGTopHack += offset2;

            if (canBGDraw)
            {
                int Width = 1024;
                int Height = 600;

                int length = textureSlots.Length;
                float[] layerScales = [1f, 1.2f, 1.34f];
                float[] layerParallax = [0.35f, 0.43f, 0.49f];

                float[] layerTopMultipliers = [1800f, 1950f, 2100f];
                float[] layerTopOffsets = [1500f, 1750f, 2000f];

                int[] layerMenuTop = [320, 400, 480];
                int[] layerMenuXOffset = [0, -80, -100];

                int[] layerYOffset = [-165, -335, -790];

                for(int i = 0; i < length; i++)
                {
                    float bgScale = layerScales[i] * bgGlobalScaleMultiplier;
                    float bgParallax = layerParallax[i];

                    var bgTopY = (int)(backgroundTopMagicNumber * layerTopMultipliers[i] + layerTopOffsets[i]) + (int)scAdj + pushBGTopHack;
                    int bgWidthScaled = (int)(Width * bgScale);

                    SkyManager.Instance.DrawToDepth(Main.spriteBatch, i == 0 ? 1.2f / bgParallax : 1f / bgParallax);

                    int bgStartX = (int)(0.0 - Math.IEEERemainder(Main.screenPosition.X * bgParallax, bgWidthScaled) - (double)(bgWidthScaled / 2));

                    if (Main.gameMenu)
                    {
                        bgTopY = layerMenuTop[i] + pushBGTopHack;
                        bgStartX += layerMenuXOffset[i];
                    }

                    int bgLoops = Main.screenWidth / bgWidthScaled + 2;

                    if (Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
                    {
                        for (int j = -bgLoops; j < bgLoops; j++)
                        {
                            Main.spriteBatch.Draw(TextureAssets.Background[textureSlots[i]].Value, new Vector2(bgStartX + bgWidthScaled * j, bgTopY + layerYOffset[i]), new Rectangle(0, 0, Width, Height), BGActualColor, 0f, default, bgScale, SpriteEffects.None, 0f);
                        }
                    }
                }
            }

            return false;
        }
    }
}