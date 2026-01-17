using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Bases.Tiles;
using ABMod.Tiles.AsteroidBiome.Trees;
using ABMod.Tiles.AsteroidBiome.Ambient.Grasses.Blue;
using ABMod.Generation;

namespace ABMod.Tiles.AsteroidBiome.Moss
{
	public class AsteroidMossBlu : BaseMoss
	{
		static ushort[] GrassRocks = new ushort[] {
			(ushort)ModContent.TileType<GrassRockBlu1>(),
			(ushort)ModContent.TileType<GrassRockBlu2>(),
			(ushort)ModContent.TileType<GrassRockBlu3>()
		};
	
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
            
			AddMapEntry(new Color(47, 30, 220));
            DustType = DustID.BlueTorch;
			MineResist = 0.1f;
		}

		public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
		{
			if(Main.tile[i, j - 1].HasTile && (Main.tile[i, j - 1].TileType == ModContent.TileType<BluMossTree>()))
			{
				return false;
			}
			
			return tileTypeBeingPlaced != ModContent.TileType<AsteroidStone>();
		}
		
		public override void RandomUpdate(int i, int j)
        {
			//Grow some ambient
			Tile Tile = Framing.GetTileSafely(i, j);
			Tile Above = Framing.GetTileSafely(i, j - 1);
			Tile Below = Framing.GetTileSafely(i, j + 1);
			
			if (!Above.HasTile && !Tile.BottomSlope && !Tile.TopSlope && !Tile.IsHalfBlock)
            {
				//Grow grass
				if(Main.rand.NextBool(15))
				{
					WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<MossGrassBlu>(), mute: true, style: Main.rand.Next(12));
					NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
				}
				
				//Big moss rock
				if(Main.rand.NextBool(70))
				{
					ushort GrassRockTile = WorldGen.genRand.Next(GrassRocks);
					
					WorldGen.PlaceTile(i, j - 1, GrassRockTile, true);
					NetMessage.SendObjectPlacement(-1, i, j - 1, GrassRockTile, 0, 0, -1, -1);
				}
            }
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.15f;
            g = 0.15f;
            b = 0.65f;
        }
	}
}