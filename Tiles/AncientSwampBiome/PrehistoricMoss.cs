using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using ABMod.Biomes;
using ABMod.Tiles.AncientSwampBiome.Trees;
using ABMod.Tiles.AncientSwampBiome.Ambient;
using ABMod.Common;

namespace ABMod.Tiles.AncientSwampBiome
{
	public class PrehistoricMoss : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Grass[Type] = true;
            TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<PreservedDirt>();
			TileID.Sets.SpreadOverground[Type] = true;
			TileID.Sets.SpreadUnderground[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 115, 54));
            RegisterItemDrop(ModContent.ItemType<PreservedDirtItem>());
            DustType = DustID.GreenMoss;
			MineResist = 0.1f;
		}

		
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail && !WorldGen.gen)
			{
				fail = true;
				Framing.GetTileSafely(i, j).TileType = (ushort)ModContent.TileType<PreservedDirt>();
			}
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
		{
			if(Main.tile[i, j - 1].HasTile && (Main.tile[i, j - 1].TileType == ModContent.TileType<Lep>()))
			{
				return false;
			}
			
			return tileTypeBeingPlaced != ModContent.TileType<PreservedDirt>();
		}
		
		public override void RandomUpdate(int i, int j)
        {
			//Spread grass
			GetSomeGrass(i, j);

			//Grow some vegetation
			Tile Tile = Framing.GetTileSafely(i, j);
			Tile Above = Framing.GetTileSafely(i, j - 1);
			
			if(!Above.HasTile && !Tile.BottomSlope && !Tile.TopSlope && !Tile.IsHalfBlock)
            {
				//Grow grass
				if(Main.rand.NextBool(15))
				{
					WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<SwampGrass>(), mute: true, style: Main.rand.Next(28));
					NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
				}
			}
        }

		private void GetSomeGrass(int i, int j)
		{
			for(int x = -1; x < 2; x++)
			{
				for(int y = -1; y < 2; y++)
				{
					//Grab the tile and check if it's potential soil to spread grass
					Tile possibleSoil = Framing.GetTileSafely(x + i, y + j);

					if(possibleSoil.TileType == ModContent.TileType<PreservedDirt>())
					{
						if(IsOpen(x + i, y + j))
						{
							possibleSoil.TileType = (ushort)ModContent.TileType<PrehistoricMoss>();

							//Idk what this does but I trust Dylan
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