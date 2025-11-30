using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Tiles.GreenMushroomBiome.Ambient
{
	public class GreenMushroomVine : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileNoFail[Type] = true;
			TileID.Sets.IsVine[Type] = true;
			TileID.Sets.VineThreads[Type] = true;
			TileID.Sets.ReplaceTileBreakDown[Type] = true;
			TileID.Sets.MultiTileSway[Type] = true;
			RegisterItemDrop(ModContent.ItemType<MushroomItem>());
			AddMapEntry(new Color(20, 144, 144));
			DustType = DustID.GreenTorch;
			HitSound = SoundID.Grass;
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.tile[i, j].TileFrameX % 18 == 0 && Main.tile[i, j].TileFrameY % 54 == 0)
			{
				Main.instance.TilesRenderer.CrawlToTopOfVineAndAddSpecialPoint(j, i);
			}

			return false;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.10f;
            g = 0.30f;
            b = 0.25f;
        }

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			int[] ValidTiles = { ModContent.TileType<GreenMushroomVine>(), ModContent.TileType<MushroomPasture>() };

			if (!ValidTiles.Contains(Main.tile[i, j - 1].TileType))
			{
				WorldGen.KillTile(i, j, false, false, false);
			}
			
            return base.TileFrame(i, j, ref resetFrame, ref noBreak);
        }

		public override void RandomUpdate(int i, int j)
		{
			Tile Tile = Framing.GetTileSafely(i, j);
			Tile tileBelow = Framing.GetTileSafely(i, j + 1);
			
			if (Main.rand.NextBool(5) && !tileBelow.HasTile && tileBelow.TileType != (ushort)ModContent.TileType<GreenMushroomVine>() && tileBelow.LiquidType != LiquidID.Lava)
            {
				bool GrowVine = false;
				
				for(int k = j; k > j - 10; j--)
				{
					if(Main.tile[i, k].BottomSlope)
					{
						break;
					}
					
					if(Main.tile[i, k].HasTile && !Tile.BottomSlope)
					{
						GrowVine = true;
						break;
					}
				}
				
				if (GrowVine) 
                {
					tileBelow.TileType = Type;
					tileBelow.HasTile = true;
					WorldGen.SquareTileFrame(i, j - 1, true);
					WorldGen.SquareTileFrame(i, j, true);
					if (Main.netMode == NetmodeID.Server) 
                    {
						NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
					}
				}
			}
		}
	}
}