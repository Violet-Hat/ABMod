using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

using ABMod.Tiles.GreenMushroomBiome;
using ABMod.Backgrounds.GreenMushroomBiome;
using ABMod.Common;

namespace ABMod.Biomes.GreenMushroom
{
    public class GreenMushroomBiome : ModBiome
    {
		//Priority, Music, background (underground), bestiary icon
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Biomes/GreenMushroomMusic");
		public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<GreenFungusUndergroundBackground>();
		public override string BestiaryIcon => "ABMod/Biomes/GreenMushroom/GreenMushroomBiomeIcon";
		
		//The biome it's active only if the tiles are above 75
		public override bool IsBiomeActive(Player player)
		{
            bool tileCount = ModContent.GetInstance<TileCount>().GreenMushroom >= 75;
            return tileCount;
		}
		
		public override void OnInBiome(Player player)
        {
			//graveyard visuals
			Main.GraveyardVisualIntensity = 0.25f;
		}
     
    }
}