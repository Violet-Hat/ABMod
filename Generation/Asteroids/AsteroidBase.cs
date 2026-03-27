using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

using ABMod.Tiles.AsteroidBiome.Moss;

namespace ABMod.Generation.Asteroids
{
	public abstract class AsteroidBase
	{
		//Moss	
		static readonly ushort[] Moss = new ushort[] {
			(ushort)ModContent.TileType<AsteroidMossRed>(),
			(ushort)ModContent.TileType<AsteroidMossOrg>(),
			(ushort)ModContent.TileType<AsteroidMossVio>(),
			(ushort)ModContent.TileType<AsteroidMossPur>(),
			(ushort)ModContent.TileType<AsteroidMossBlu>()
		};
		
		public abstract bool Place(Point origin, bool style);
		
		public abstract void GenerateAsteroid(Point origin, int size);
		
		public static ushort GiveMoss() { return WorldGen.genRand.Next(Moss); }
	}
}