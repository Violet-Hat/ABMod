using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Common.Tiles;

namespace ABMod.Content.Tiles.Swamp
{
	public class SwampMoss : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Grass[Type] = true;
            TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<SwampSoil>();
			TileID.Sets.SpreadOverground[Type] = true;
			TileID.Sets.SpreadUnderground[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(80, 115, 54));
            RegisterItemDrop(ModContent.ItemType<SwampSoilItem>());
            DustType = DustID.GreenMoss;
			MineResist = 0.1f;
		}
		
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail && !WorldGen.gen)
			{
				fail = true;
				Framing.GetTileSafely(i, j).TileType = (ushort)ModContent.TileType<SwampSoil>();
			}
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
		{
			if(Framing.GetTileSafely(i, j - 1).HasTile && TreeTile.IsTreeType(i, j - 1))
			{
				return false;
			}
			
			return tileTypeBeingPlaced != ModContent.TileType<SwampSoil>();
		}
		
		public override void RandomUpdate(int i, int j)
        {
			//Grow some vegetation
			Tile Tile = Framing.GetTileSafely(i, j);
			Tile Above = Framing.GetTileSafely(i, j - 1);
			
			if(!Above.HasTile && !Tile.BottomSlope && !Tile.TopSlope && !Tile.IsHalfBlock)
            {
				//Grow grass
				if(Main.rand.NextBool(15))
				{
					//WorldGen.PlaceTile(i, j - 1, (ushort)ModContent.TileType<SwampGrass>(), mute: true, style: Main.rand.Next(28));
					//NetMessage.SendTileSquare(-1, i, j - 1, 1, TileChangeType.None);
				}
			}
        }
	}
}