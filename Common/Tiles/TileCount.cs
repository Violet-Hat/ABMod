using ABMod.Content.Tiles.Swamp;
using System;
using Terraria.ModLoader;

namespace ABMod.Common.Tiles
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
			//GreenMushroom = tileCounts[ModContent.TileType<MushroomPasture>()];
			AncientSwamp = tileCounts[ModContent.TileType<SwampMoss>()]
			+ tileCounts[ModContent.TileType<SwampSoil>()]
			+ tileCounts[ModContent.TileType<SwampDirt>()]
			+ tileCounts[ModContent.TileType<SwampStone>()];
			//HorizonEdge = tileCounts[ModContent.TileType<AsteroidStone>()]
			//+tileCounts[ModContent.TileType<AsteroidRock>()]
			//+tileCounts[ModContent.TileType<AsteroidMossRed>()]
			//+tileCounts[ModContent.TileType<AsteroidMossOrg>()]
			//+tileCounts[ModContent.TileType<AsteroidMossVio>()]
			//+tileCounts[ModContent.TileType<AsteroidMossPur>()]
			//+tileCounts[ModContent.TileType<AsteroidMossBlu>()];
		}
	}
}