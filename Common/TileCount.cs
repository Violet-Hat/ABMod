using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

using ABMod.Tiles.GreenMushroomBiome;
using ABMod.Tiles.AncientSwampBiome;
using ABMod.Tiles.AsteroidBiome;
using ABMod.Tiles.AsteroidBiome.Moss;

namespace ABMod.Common
{
	public class TileCount : ModSystem
	{
		//Counters
		public int GreenMushroom { get; set; }
		public int AncientSwamp { get; set; }
        public int HorizonEdge { get; set; }
		
		//Reset the counters
		public override void ResetNearbyTileEffects()
        {
            GreenMushroom = 0;
            AncientSwamp = 0;
            HorizonEdge = 0;
        }
		
		//Add to the counters
		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
			GreenMushroom = tileCounts[ModContent.TileType<MushroomPasture>()];
			AncientSwamp = tileCounts[ModContent.TileType<PrehistoricMoss>()]
			+ tileCounts[ModContent.TileType<PreservedDirt>()]
			+ tileCounts[ModContent.TileType<AncientStone>()];
			HorizonEdge = tileCounts[ModContent.TileType<AsteroidStone>()]
			+tileCounts[ModContent.TileType<AsteroidRock>()]
			+tileCounts[ModContent.TileType<AsteroidMossRed>()]
			+tileCounts[ModContent.TileType<AsteroidMossOrg>()]
			+tileCounts[ModContent.TileType<AsteroidMossVio>()]
			+tileCounts[ModContent.TileType<AsteroidMossPur>()]
			+tileCounts[ModContent.TileType<AsteroidMossBlu>()];
		}
	}
}