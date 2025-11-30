using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AsteroidBiome.Ambient.Grasses.Red
{
    public class GrassRockRed1 : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(87, 64, 54));
			HitSound = SoundID.Tink;
            DustType = DustID.Dirt;
        }
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.65f;
            g = 0.15f;
            b = 0.15f;
        }
	}
	
	public class GrassRockRed2 : GrassRockRed1
    {
	}
	
	public class GrassRockRed3 : GrassRockRed1
    {
	}
}