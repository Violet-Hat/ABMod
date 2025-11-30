using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AncientSwampBiome
{
    public class AncientDirtWall : ModWall 
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(51, 44, 31));
            DustType = DustID.Stone;
        }
    }
}