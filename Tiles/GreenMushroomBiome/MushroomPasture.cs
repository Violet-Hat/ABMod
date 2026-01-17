using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Generation;
using ABMod.Tiles.GreenMushroomBiome.Trees;
using ABMod.Tiles.GreenMushroomBiome.Ambient;

namespace ABMod.Tiles.GreenMushroomBiome
{
	public class MushroomPasture : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Grass[Type] = true;
            TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<MushroomSoil>();
			TileID.Sets.SpreadOverground[Type] = true;
			TileID.Sets.SpreadUnderground[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            AddMapEntry(new Color(169, 122, 144));
            RegisterItemDrop(ModContent.ItemType<MushroomSoilItem>());
            DustType = DustID.PinkTorch;
			MineResist = 0.1f;
		}
		
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail && !WorldGen.gen)
			{
				fail = true;
				Framing.GetTileSafely(i, j).TileType = (ushort)ModContent.TileType<MushroomSoil>();
			}
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
		{
			if(Main.tile[i, j - 1].HasTile && (Main.tile[i, j - 1].TileType == ModContent.TileType<GreenFungusTree>()))
			{
				return false;
			}
			
			return tileTypeBeingPlaced != ModContent.TileType<MushroomSoil>();
		}
		
		public override void RandomUpdate(int i, int j)
        {
			//Spread grass
			GetSomeGrass(i, j);

			//Grow some fungus
			Tile Tile = Framing.GetTileSafely(i, j);
			Tile Above = Framing.GetTileSafely(i, j - 1);
			Tile Below = Framing.GetTileSafely(i, j + 1);
			
			if (!Below.HasTile && Below.LiquidAmount <= 0 && !Tile.BottomSlope)
            {
                //grow vines
                if (Main.rand.NextBool(20)) 
                {
                    WorldGen.PlaceTile(i, j + 1, (ushort)ModContent.TileType<GreenMushroomVine>(), true);
					WorldGen.SquareTileFrame(i, j - 1, true);
					WorldGen.SquareTileFrame(i, j, true);
                }
            }
			
			if (!Above.HasTile && !Tile.BottomSlope && !Tile.TopSlope && !Tile.IsHalfBlock)
            {
				//Grow regular mushrooms
				if(Main.rand.NextBool(8))
				{
					WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<GreenMushroom>(), mute: true, style: Main.rand.Next(4));
					NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
				}

				/*Grow mushroom trees rarely
                if (Main.rand.NextBool(30))
                {
                    if(WorldgenTools.GrowTreeCheck(i, j, 4, 18, ModContent.TileType<GreenFungusTree>()))
					{
						GreenFungusTree.Grow(i, j - 1, 8, 15, false);
					}
                }
				*/
            }
        }

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        	{
            		r = 0.17f;
            		g = 0.13f;
            		b = 0.13f;
        	}

		private void GetSomeGrass(int i, int j)
		{
			for(int x = -1; x < 2; x++)
			{
				for(int y = -1; y < 2; y++)
				{
					//Grab the tile and check if it's potential soil to spread grass
					Tile possibleSoil = Framing.GetTileSafely(x + i, y + j);

					if(possibleSoil.TileType == ModContent.TileType<MushroomSoil>())
					{
						if(IsOpen(x + i, y + j))
						{
							possibleSoil.TileType = (ushort)ModContent.TileType<MushroomPasture>();
							if (Main.netMode == NetmodeID.Server)
                    		{
                        		NetMessage.SendTileSquare(-1, x + i, y + j, 1, TileChangeType.None);
                    		}
						}
					}
				}
			}
		}

		//Basically the same code of Spooky Mod's 'HasOpening' class
		private bool IsOpen(int i, int j)
        	{
            	for (int x = -1; x < 2; x++)
            	{
                	for (int y = -1; y < 2; y++)
                	{
                    	if (!Framing.GetTileSafely(i + x, j + y).HasTile)
                    	{
                        	return true;
                    	}
                	}
            	}
                    
            return false;
        }
	}
}