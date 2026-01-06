using Terraria;
using Terraria.ModLoader;

using ABMod.Common;

namespace ABMod.Biomes.AncientSwamp
{
    public class AncientSwampBiome : ModBiome
    {
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ABMod/MurkyWaterStyle");

        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle
		=> ModContent.Find<ModSurfaceBackgroundStyle>("ABMod/SwampSurfaceBGStyle");

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Biomes/AncientSwampMusic");
		public override string BestiaryIcon => "ABMod/Biomes/AncientSwamp/AncientSwampBiomeIcon";

		public override bool IsBiomeActive(Player player)
		{
            return ModContent.GetInstance<TileCount>().AncientSwamp > 150;
		}
    }
}