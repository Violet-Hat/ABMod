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
using ReLogic.Utilities;

using ABMod.Common;
using ABMod.Bases.Generation;
using ABMod.Tiles.AsteroidBiome;

namespace ABMod.Generation.Asteroids
{
	public class GeodeAsteroid : Asteroid
	{
		public override bool Place(Point origin, StructureMap structures)
		{
			int size = WorldGen.genRand.Next(20, 26);
			
			//If the world it's small, make it smaller
			if(Main.maxTilesX < 6400)
			{
				size = size - 5;
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
			Circle asteroid = new Circle(new Vector2(origin.X, origin.Y), size);
			
			int innerSize = (int)(size * 0.6f);
			int holeSize = (int)(size * 0.35f);
			
			ShapeData outerLayer = new ShapeData();
			ShapeData innerLayer = new ShapeData();
			ShapeData holeLayer = new ShapeData();
			
			//Shape making
			GenAction blotches = new Modifiers.Blotches(2, 0.4);
			
			WorldUtils.Gen(origin, new Shapes.Circle(size), Actions.Chain(new GenAction[]
			{
				blotches.Output(outerLayer),
			}));
			WorldUtils.Gen(origin, new Shapes.Circle(innerSize), Actions.Chain(new GenAction[]
			{
				blotches.Output(innerLayer),
			}));
			WorldUtils.Gen(origin, new Shapes.Circle(holeSize), Actions.Chain(new GenAction[]
			{
				blotches.Output(holeLayer),
			}));
			
			//Place them
			WorldUtils.Gen(origin, new ModShapes.All(outerLayer), Actions.Chain(new GenAction[]
			{
				new Actions.PlaceTile(AsteroidRock),
				new Actions.PlaceWall(AsteroidRockWall),
			}));
			
			WorldUtils.Gen(origin, new ModShapes.All(innerLayer), Actions.Chain(new GenAction[]
			{
				new Actions.SetTile(TerminusCrystal),
				new Actions.ClearWall(),
				new Actions.PlaceWall(TerminusCrystalWall),
			}));
			
			//Place random patches of crystal on the rock
			int crysRand = (int)(size * 0.35f);
			
			for(int i = 0; i < crysRand; i++)
			{
				Point coordinates = asteroid.RandomPointInCircle();
				WorldGen.TileRunner(coordinates.X, coordinates.Y, WorldGen.genRand.NextFloat(3.5f, 8.5f), WorldGen.genRand.Next(4, 12), TerminusCrystal);
			}
			
			//Remove walls
			WorldUtils.Gen(origin, new ModShapes.InnerOutline(outerLayer), Actions.Chain(new GenAction[]
            {
				new Actions.ClearWall(true),
			}));
			
			//Place small hole in the center
			WorldUtils.Gen(origin, new ModShapes.All(holeLayer), Actions.Chain(new GenAction[]
			{
				new Actions.ClearTile(),
			}));
			
			//Create a cave with a 50% chance
			if(WorldGen.genRand.NextBool())
			{
				Point coordinates = asteroid.PointOutsideCircle(5);
				int offsetX = origin.X - coordinates.X;
				int offsetY = origin.Y - coordinates.Y;
				
				Vector2D offSet = new Vector2D(offsetX, offsetY);

				ShapeData caveLayer = new ShapeData();
				
				//Generate shape
				int caveScale = WorldGen.genRand.Next(4, 6);
				GenAction shapeScale = new Modifiers.ShapeScale(caveScale);
				
				WorldUtils.Gen(origin, new Shapes.Tail(2, offSet), Actions.Chain(new GenAction[]
				{
					shapeScale.Output(caveLayer),
					blotches.Output(caveLayer),
				}));
				
				//Place
				WorldUtils.Gen(origin, new ModShapes.All(caveLayer), Actions.Chain(new GenAction[]
				{
					new Actions.ClearTile(),
				}));
			}
		}
	}
}