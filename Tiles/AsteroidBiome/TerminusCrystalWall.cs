using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AsteroidBiome
{
    public class TerminusCrystalWall : ModWall 
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(133, 6, 21));
            DustType = DustID.RedTorch;
        }
    }
}