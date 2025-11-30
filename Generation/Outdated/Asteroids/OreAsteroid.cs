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
using ABMod.Bases.Generation;
using ABMod.Tiles.AsteroidBiome;

namespace ABMod.Generation.Asteroids
{
	public class OreAsteroid : Asteroid
	{
		static ushort[] ores = new ushort[] {
			GenVars.iron == TileID.Iron ? TileID.Lead : TileID.Iron,
			GenVars.gold == TileID.Gold ? TileID.Platinum : TileID.Gold
		};
		
		public override bool Place(Point origin, StructureMap structures)
		{
			int size = WorldGen.genRand.Next(10, 16);
			
			//If the world it's small, make it smaller
			if(Main.maxTilesX < 6400)
			{
				size = size - 3;
			}
			
			bool check = WorldgenTools.IsItPlaceable(origin, size, structures);
			
			//Return false if it isn't placeable
			if (!check)
            {
                return false;
            }
			
			//Generate the asteroid
            generateAsteroid(origin, size);
			
			//Otherwise return :true:
            return true;
		}
		
		public override void generateAsteroid(Point origin, int size)
		{
			ushort moss = giveMoss();
			
			Circle asteroid = new Circle(new Vector2(origin.X, origin.Y), size + 6);
			
			//This randomizes the shape
			for(int i = 0; i < 6; i++)
			{
				Vector2 offset = WorldGen.genRand.NextVector2Circular(6f, 6f);
				Point newOrigin = new Point((int)(origin.X + offset.X), (int)(origin.Y + offset.Y));
				
				WorldUtils.Gen(newOrigin, new Shapes.Circle(size), Actions.Chain(new GenAction[]
				{
					new Actions.PlaceTile(AsteroidStone),
				}));
				
				WorldUtils.Gen(newOrigin, new Shapes.Circle(size - 2), Actions.Chain(new GenAction[]
				{
					new Actions.PlaceWall(WallID.Dirt),
				}));
			}
			
			//Place random patches of rock
			int rockRand = (int)(size * 0.45f);

			for(int i = 0; i < rockRand; i++)
			{
				Point coordinates = asteroid.RandomPointInCircle();
				
				WorldGen.TileRunner(coordinates.X, coordinates.Y, WorldGen.genRand.NextFloat(4f, 9f), WorldGen.genRand.Next(4, 12), AsteroidRock);
			}
			
			//Place random patches of ores
			int oreRand = (int)(size * 0.45f);
			
			for(int i = 0; i < rockRand; i++)
			{
				ushort oreType = WorldGen.genRand.Next(ores);
				Point coordinates = asteroid.RandomPointInCircle();
				
				WorldGen.TileRunner(coordinates.X, coordinates.Y, WorldGen.genRand.NextFloat(4f, 8f), WorldGen.genRand.Next(6, 12), oreType);
			}
			
			//Place moss
			WorldUtils.Gen(origin, new Shapes.Circle(size + 7), Actions.Chain(new GenAction[]
			{
				new Modifiers.OnlyTiles(new ushort[] { AsteroidStone }),
                new Modifiers.IsTouchingAir(true),
                new Actions.SetTile(moss, false, true),
                new Actions.SetFrames(true),
			}));
		}
	}
}