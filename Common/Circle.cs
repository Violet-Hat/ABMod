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

namespace ABMod.Generation
{
	public struct Circle
	{
		public Vector2 Center { get; set; }
		public int Radius { get; set; }
		
		public Circle(Vector2 center, int radius)
		{
			Center = center;
			Radius = radius;
		}
		
		public Point RandomPointInCircle()
		{
			int randRadius = WorldGen.genRand.Next(Radius);
			
			int angle = WorldGen.genRand.Next(360);
			
			int x = (int)(Center.X + randRadius * Math.Cos(angle));
			int y = (int)(Center.Y + randRadius * Math.Sin(angle));
			
			return new Point(x, y);
		}
		
		public Point PointOutsideCircle(int padding)
		{
			int angle = WorldGen.genRand.Next(360);
			int newRadius = Radius + padding;
			
			int x = (int)(Center.X + newRadius * Math.Cos(angle));
			int y = (int)(Center.Y + newRadius * Math.Sin(angle));
			
			return new Point(x, y);
		}
		
		public Point PointOnEdgeCircle()
		{
			int angle = WorldGen.genRand.Next(360);
			int safeRadius = Radius - 1;
			
			int x = (int)(Center.X + safeRadius * Math.Cos(angle));
			int y = (int)(Center.Y + safeRadius * Math.Sin(angle));
			
			return new Point(x, y);
		}
	}
}