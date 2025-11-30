using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AsteroidBiome.Ambient.Rock
{
    public class Crystal1 : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(225, 42, 63));
			HitSound = SoundID.Shatter;
			DustType = DustID.RedTorch;
		}
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.75f;
			g = 0.15f;
			b = 0.25f;
		}
	}
	
	public class Crystal2 : Crystal1
    {
	}
	
	public class Crystal3 : Crystal1
    {
	}
}