using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AsteroidBiome.Ambient.Soft
{
    public class BigRock1 : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(87, 64, 54));
			HitSound = SoundID.Tink;
            DustType = DustID.Dirt;
        }
	}
	
	public class BigRock2 : BigRock1
    {
	}
	
	public class BigRock3 : BigRock1
    {
	}
}