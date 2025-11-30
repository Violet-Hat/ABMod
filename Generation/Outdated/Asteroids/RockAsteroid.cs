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
	public class RockAsteroid : Asteroid
	{
		public override bool Place(Point origin, StructureMap structures)
		{
			int size = WorldGen.genRand.Next(5, 9);
			
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
			Circle asteroid = new Circle(new Vector2(origin.X, origin.Y), size);
			
			ShapeData mainLayer = new ShapeData();
			
			//Shape making
			GenAction blotches = new Modifiers.Blotches(2, 0.4);
			
			WorldUtils.Gen(origin, new Shapes.Circle(size), Actions.Chain(new GenAction[]
			{
				blotches.Output(mainLayer),
			}));
			
			//Place them
			WorldUtils.Gen(origin, new ModShapes.All(mainLayer), Actions.Chain(new GenAction[]
			{
				new Actions.PlaceTile(AsteroidRock),
				new Actions.PlaceWall(AsteroidRockWall),
			}));
			
			//Remove walls
			WorldUtils.Gen(origin, new ModShapes.InnerOutline(mainLayer), Actions.Chain(new GenAction[]
            {
				new Actions.ClearWall(true),
			}));
			
			//Generate random crystal patches
			int randCrystal = (int)(size * 0.4f);
			
			for(int i = 0; i < randCrystal; i++)
			{
				Point coordinates = asteroid.PointOnEdgeCircle();
				
				WorldGen.TileRunner(coordinates.X, coordinates.Y, WorldGen.genRand.NextFloat(4f, 6f), 1, TerminusCrystal);
			}
		}
	}
}