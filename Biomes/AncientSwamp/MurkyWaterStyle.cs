using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ABMod.Biomes.AncientSwamp
{
	public class MurkyWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("ABMod/MurkyWaterfallStyle").Slot;

		public override int GetSplashDust() => ModContent.Find<ModDust>("ABMod/MurkyWaterSplash").Type;

		public override int GetDropletGore() => ModContent.Find<ModGore>("ABMod/MurkyWaterDroplet").Type;

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor() => Color.GreenYellow;

        public override byte GetRainVariant()
        {
            return (byte)Main.rand.Next(3);
        }

        public override Asset<Texture2D> GetRainTexture()
        {
            return Request<Texture2D>("ABMod/Biomes/AncientSwamp/MurkyRain");
        }
	}
}