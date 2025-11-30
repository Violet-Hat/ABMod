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
	public class DonutAsteroid : Asteroid
	{
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
			
			Circle asteroid = new Circle(new Vector2(origin.X, origin.Y), size);
			
			int innerSize = (int)(size * 0.58f);
			int holeSize = (int)(size * 0.32f);
			
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
				new Actions.PlaceTile(AsteroidStone),
				new Actions.PlaceWall(WallID.Dirt),
			}));
			
			WorldUtils.Gen(origin, new ModShapes.All(innerLayer), Actions.Chain(new GenAction[]
			{
				new Actions.SetTile(AsteroidRock),
				new Actions.ClearWall(),
				new Actions.PlaceWall(AsteroidRockWall),
			}));

			//Place random patches of rock on the softer stone
			int rockRand = (int)(size * 0.45f);
			
			for(int i = 0; i < rockRand; i++)
			{
				Point coordinates = asteroid.RandomPointInCircle();
				WorldGen.TileRunner(coordinates.X, coordinates.Y, WorldGen.genRand.NextFloat(3.5f, 8.5f), WorldGen.genRand.Next(5, 15), AsteroidRock);
			}
			
			//Place moss
			WorldUtils.Gen(origin, new ModShapes.InnerOutline(outerLayer), Actions.Chain(new GenAction[]
            {
				new Actions.ClearWall(true),
				new Modifiers.OnlyTiles(new ushort[] { AsteroidStone }),
                new Modifiers.IsTouchingAir(true),
                new Actions.SetTile(moss, false, true),
                new Actions.SetFrames(true),
			}));
			
			//Place small hole in the center
			WorldUtils.Gen(origin, new ModShapes.All(holeLayer), Actions.Chain(new GenAction[]
			{
				new Actions.ClearTile(),
			}));
		}
	}
}