using Terraria;
using Terraria.ModLoader;

using ABMod.Common.Tiles;

namespace ABMod.Content.Biomes.Swamp
{
    public class SwampBiome : ModBiome
    {
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("ABMod/SwampSurfaceBGStyle");
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ABMod/MurkyWaterStyle");
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/Biomes/SwampMusic");

		//Bestiary stuff
		public override string BestiaryIcon => "ABMod/Content/Biomes/Swamp/SwampBiomeIcon";

		public override bool IsBiomeActive(Player player)
		{
            return ModContent.GetInstance<TileCount>().AncientSwamp > 150;
		}
    }
}