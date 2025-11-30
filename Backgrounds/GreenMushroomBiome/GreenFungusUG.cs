using Terraria.ModLoader;

namespace ABMod.Backgrounds.GreenMushroomBiome
{
	public class GreenFungusUndergroundBackground : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots)
        {
			for (int i = 0; i <= 3; i++)
			{
                textureSlots[i] = BackgroundTextureLoader.GetBackgroundSlot("ABMod/Backgrounds/GreenMushroomBiome/GreenFungusUG" + i.ToString());
			}
		}
	}
}