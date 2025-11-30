using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

using ABMod.Common;
using ABMod.Tiles.AsteroidBiome;
using ABMod.Tiles.AsteroidBiome.Moss;

namespace ABMod.Bases.Generation
{
	public class Asteroid : MicroBiome
	{
		public static ushort AsteroidStone = (ushort)ModContent.TileType<AsteroidStone>(),
		AsteroidRock = (ushort)ModContent.TileType<AsteroidRock>(),
		TerminusCrystal = (ushort)ModContent.TileType<TerminusCrystal>(),
		AsteroidRockWall = (ushort)ModContent.WallType<AsteroidRockWall>(),
		TerminusCrystalWall = (ushort)ModContent.WallType<TerminusCrystalWall>();
		
		static ushort[] Moss = new ushort[] {
			(ushort)ModContent.TileType<AsteroidMossRed>(),
			(ushort)ModContent.TileType<AsteroidMossOrg>(),
			(ushort)ModContent.TileType<AsteroidMossVio>(),
			(ushort)ModContent.TileType<AsteroidMossPur>(),
			(ushort)ModContent.TileType<AsteroidMossBlu>()
		};
		
		public override bool Place(Point origin, StructureMap structures)
		{
			return true;
		}
		
		public virtual void generateAsteroid(Point origin, int size)
		{
		}
		
		public ushort giveMoss(bool central = false)
		{
			if(!central)
				return WorldGen.genRand.Next(Moss);
			
			return (ushort)ModContent.TileType<AsteroidMossRed>();
		}
	}
}