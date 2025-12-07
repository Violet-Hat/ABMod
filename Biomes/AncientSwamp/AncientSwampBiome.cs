using Terraria;
using Terraria.ModLoader;
using ABMod.Common;

namespace ABMod.Biomes.AncientSwamp
{
    public class AncientSwampBiome : ModBiome
    {
		//Priority, Music, background (underground), bestiary icon
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ABMod/MurkyWaterStyle");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Biomes/AncientSwampMusic");
		public override string BestiaryIcon => "ABMod/Biomes/AncientSwamp/AncientSwampBiomeIcon";
		
		//The biome it's active only if the tiles are above 150
		public override bool IsBiomeActive(Player player)
		{
            bool tileCount = ModContent.GetInstance<TileCount>().AncientSwamp >= 150;
            return tileCount;
		}
    }
}