using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

using ABMod.Tiles.AsteroidBiome;
using ABMod.Common;

namespace ABMod.Biomes.HorizonEdge
{
    public class AsteroidBiome : ModBiome
    {
		//Priority, Music, background (underground), bestiary icon
		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Biomes/HorizonsEdgeMusic");
		public override string BestiaryIcon => "ABMod/Biomes/HorizonEdge/AsteroidBiomeIcon";
		
		//The biome it's active only if the tiles are above 150
		public override bool IsBiomeActive(Player player)
		{
            bool tileCount = ModContent.GetInstance<TileCount>().HorizonEdge >= 150;
            return tileCount;
		}
    }
}