using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
	}
}