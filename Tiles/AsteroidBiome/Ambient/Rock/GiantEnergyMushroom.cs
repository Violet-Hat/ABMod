using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AsteroidBiome.Ambient.Rock
{
    public class GiantEnergyMushroom1 : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLighted[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.RandomStyleRange = 4;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(27, 237, 191));
            HitSound = SoundID.Item8;
			DustType = DustID.HallowSpray;
		}
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.15f;
            g = 0.65f;
            b = 0.75f;
        }
	}
	
	public class GiantEnergyMushroom2 : GiantEnergyMushroom1
    {
	}
	
	public class GiantEnergyMushroom3 : GiantEnergyMushroom1
    {
	}
	
	public class GiantEnergyMushroom4 : GiantEnergyMushroom1
    {
	}
}