using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Bases.Tiles;
using ABMod.Tiles.AsteroidBiome.Trees;
using ABMod.Tiles.AsteroidBiome.Ambient.Grasses.Red;
using ABMod.Generation;

namespace ABMod.Tiles.AsteroidBiome.Moss
{
	public class AsteroidMossRed : BaseMoss
	{
		static ushort[] GrassRocks = new ushort[] {
			(ushort)ModContent.TileType<GrassRockRed1>(),
			(ushort)ModContent.TileType<GrassRockRed2>(),
			(ushort)ModContent.TileType<GrassRockRed3>()
		};
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			
            AddMapEntry(new Color(220, 30, 46));
            DustType = DustID.RedTorch;
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
					WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<MossGrassRed>(), mute: true, style: Main.rand.Next(12));
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
                    if(WorldgenTools.GrowTreeCheck(i, j, 4, 18, ModContent.TileType<RedMossTree>()))
					{
						RedMossTree.Grow(i, j - 1, 10, 15, false);
					}
                }
            }
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.65f;
            g = 0.15f;
            b = 0.15f;
        }
	}
}