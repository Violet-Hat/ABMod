using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Bases.Tiles;
using ABMod.Tiles.AsteroidBiome.Trees;
using ABMod.Tiles.AsteroidBiome.Ambient.Grasses.Purple;
using ABMod.Generation;

namespace ABMod.Tiles.AsteroidBiome.Moss
{
	public class AsteroidMossPur : BaseMoss
	{
		static ushort[] GrassRocks = new ushort[] {
			(ushort)ModContent.TileType<GrassRockPur1>(),
			(ushort)ModContent.TileType<GrassRockPur2>(),
			(ushort)ModContent.TileType<GrassRockPur3>()
		};
	
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			
            AddMapEntry(new Color(113, 30, 220));
            DustType = DustID.DemonTorch;
			MineResist = 0.1f;
		}

		public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
		{
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
					WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<MossGrassPur>(), mute: true, style: Main.rand.Next(12));
					NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
				}
				
				//Big moss rock
				if(Main.rand.NextBool(70))
				{
					ushort GrassRockTile = WorldGen.genRand.Next(GrassRocks);
					
					WorldGen.PlaceTile(i, j - 1, GrassRockTile, true);
					NetMessage.SendObjectPlacement(-1, i, j - 1, GrassRockTile, 0, 0, -1, -1);
				}
				
				//Grow trees rarely
                if (Main.rand.NextBool(100))
                {
                    if(WorldgenTools.GrowTreeCheck(i, j, 4, 18, ModContent.TileType<PurMossTree>()))
					{
						PurMossTree.Grow(i, j - 1, 10, 15, false);
					}
                }
            }
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.45f;
            g = 0.15f;
            b = 0.65f;
        }
	}
}