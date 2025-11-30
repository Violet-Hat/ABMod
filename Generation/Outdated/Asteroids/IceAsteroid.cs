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
	public class IceAsteroid : Asteroid
	{
		public override bool Place(Point origin, StructureMap structures)
		{
			int size = WorldGen.genRand.Next(24, 30);
			
			//If the world it's medium or small, make it smaller
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
			
			float variation = WorldGen.genRand.NextFloat(70f, 80f);
			variation = variation/100f;
			
			int holeSize = (int)(size * variation);
			
			ShapeData shellLayer = new ShapeData();
			ShapeData holeLayer = new ShapeData();
			
			//Shape making
			GenAction blotches = new Modifiers.Blotches(2, 0.4);
			
			WorldUtils.Gen(origin, new Shapes.Circle(size), Actions.Chain(new GenAction[]
			{
				blotches.Output(shellLayer),
			}));
			WorldUtils.Gen(origin, new Shapes.Circle(holeSize), Actions.Chain(new GenAction[]
			{
				blotches.Output(holeLayer),
			}));
			
			//Place layers
			WorldUtils.Gen(origin, new ModShapes.All(shellLayer), Actions.Chain(new GenAction[]
			{
				new Actions.PlaceTile(TileID.IceBlock),
				new Actions.PlaceWall(WallID.IceUnsafe),
			}));
			
			WorldUtils.Gen(origin, new ModShapes.All(holeLayer), Actions.Chain(new GenAction[]
			{
				new Actions.ClearTile(),
			}));
			
			//Clear walls around the asteroid
			WorldUtils.Gen(origin, new ModShapes.InnerOutline(shellLayer), Actions.Chain(new GenAction[]
            {
				new Actions.ClearWall(true),
			}));
			
			//Place water
			WorldUtils.Gen(origin, new ModShapes.All(holeLayer), Actions.Chain(new GenAction[]
			{
				new Actions.SetLiquid(0, 255),
			}));
		}
	}
}