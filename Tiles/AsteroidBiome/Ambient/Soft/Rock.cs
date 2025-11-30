using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AsteroidBiome.Ambient.Soft
{
    public class Rock1 : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(87, 64, 54));
			HitSound = SoundID.Tink;
            DustType = DustID.Dirt;
		}
	}
	
	public class Rock2 : Rock1
    {
	}
	
	public class Rock3 : Rock1
    {
	}
}