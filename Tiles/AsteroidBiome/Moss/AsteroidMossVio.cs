using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Tiles.AsteroidBiome.Ambient.Grasses.Violet;
using ABMod.Generation;

namespace ABMod.Tiles.AsteroidBiome.Moss
{
	public class AsteroidMossVio : ModTile
	{
		//Ambient
		static readonly ushort[] GrassRocks = new ushort[] {
			(ushort)ModContent.TileType<GrassRockVio1>(),
			(ushort)ModContent.TileType<GrassRockVio2>(),
			(ushort)ModContent.TileType<GrassRockVio3>()
		};
		
		public override void SetStaticDefaults()
		{
			TileID.Sets.Grass[Type] = true;
            TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<AsteroidStone>();
			TileID.Sets.SpreadOverground[Type] = true;
			TileID.Sets.SpreadUnderground[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            RegisterItemDrop(ModContent.ItemType<AsteroidStoneItem>());
            AddMapEntry(new Color(212, 30, 220));
            DustType = DustID.PurpleTorch;
			MineResist = 0.1f;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail && !WorldGen.gen)
			{
				fail = true;
				Framing.GetTileSafely(i, j).TileType = (ushort)ModContent.TileType<AsteroidStone>();
			}
		}

		public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
		{
			if(Main.tile[i, j - 1].HasTile && WorldgenTools.IsTreeType(i, j - 1))
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
					WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<MossGrassVio>(), mute: true, style: Main.rand.Next(12));
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
            r = 0.65f;
            g = 0.15f;
            b = 0.65f;
        }
	}
}