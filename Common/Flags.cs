using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ABMod.Common
{
    public class Flags : ModSystem
    {
		//Positions of the holes in the Green Mushrooms Caves and soft dirt points for the Ancient Swamps
		public static List<Vector2> GreenCavesPositions = new List<Vector2>();
		public static List<Point> SoftDirtPoints = new List<Point>();
		
		public override void SaveWorldData(TagCompound tag)
        {
			tag["ABMod:GreenCavesPositions"] = GreenCavesPositions;
			tag["ABMod:SoftDirtPoints"] = SoftDirtPoints;
		}
		
		public override void LoadWorldData(TagCompound tag) 
        {
			if (tag.ContainsKey("ABMod:GreenCavesPositions"))
			{
				GreenCavesPositions = tag.Get<List<Vector2>>("ABMod:GreenCavesPositions");
			}

			if (tag.ContainsKey("ABMod:SoftDirtPoints"))
			{
				SoftDirtPoints = tag.Get<List<Point>>("ABMod:SoftDirtPoints");
			}
		}
	}
}