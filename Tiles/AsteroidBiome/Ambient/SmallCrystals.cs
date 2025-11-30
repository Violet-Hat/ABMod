using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace ABMod.Tiles.AsteroidBiome.Ambient
{
    public class SmallCrystals : ModTile
    {
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(225, 42, 63));
			HitSound = SoundID.Shatter;
			DustType = DustID.RedTorch;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.75f;
            g = 0.15f;
            b = 0.25f;
        }
		
		public override bool CanPlace(int i, int j)
        {
			//Hell
			Tile below = Main.tile[i, j + 1];
			Tile above = Main.tile[i, j - 1];
			Tile right = Main.tile[i + 1, j];
			Tile left = Main.tile[i - 1, j];

            if ((below.Slope == SlopeType.Solid && !below.IsHalfBlock && Main.tileSolid[below.TileType]) ||
				(above.Slope == SlopeType.Solid && !above.IsHalfBlock && above.HasTile && Main.tileSolid[above.TileType]) ||
				(right.Slope == SlopeType.Solid && !right.IsHalfBlock && right.HasTile && Main.tileSolid[right.TileType]) ||
				(left.Slope == SlopeType.Solid && !left.IsHalfBlock && left.HasTile && Main.tileSolid[left.TileType]))
				return true;

            return false;
        }
		
		public override void PlaceInWorld(int i, int j, Item item)
        {
			//Get the tiles around it
			Tile below = Main.tile[i, j + 1];
			Tile above = Main.tile[i, j - 1];
			Tile right = Main.tile[i + 1, j];
			Tile left = Main.tile[i - 1, j];
			
			if (below.Slope == SlopeType.Solid && !below.IsHalfBlock && below.HasTile && Main.tileSolid[below.TileType])
			{
				Main.tile[i, j].TileFrameY = 0;
			}
			else if (above.Slope == SlopeType.Solid && !above.IsHalfBlock && above.HasTile && Main.tileSolid[below.TileType])
			{
				Main.tile[i, j].TileFrameY = 18;
			}
			else if (right.Slope == SlopeType.Solid && !right.IsHalfBlock && right.HasTile && Main.tileSolid[below.TileType])
			{
				Main.tile[i, j].TileFrameY = 36;
			}
			else if (left.Slope == SlopeType.Solid && !left.IsHalfBlock && left.HasTile && Main.tileSolid[below.TileType])
			{
				Main.tile[i, j].TileFrameY = 54;
			}
			
			//Randomize the X frame
			Main.tile[i, j].TileFrameX = (short)(WorldGen.genRand.Next(6) * 18);
        }
	}
}