using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ABMod.Content.Waters.Swamp
{
	public class MurkyWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle() => Find<ModWaterfallStyle>("ABMod/MurkyWaterfallStyle").Slot;
		public override int GetSplashDust() => Find<ModDust>("ABMod/MurkyWaterSplash").Type;
		public override int GetDropletGore() => Find<ModGore>("ABMod/MurkyWaterDroplet").Type;

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor() => Color.GreenYellow;

		public override Asset<Texture2D> GetRainTexture() => Request<Texture2D>("ABMod/Content/Biomes/Swamp/MurkyRain");
	}
}