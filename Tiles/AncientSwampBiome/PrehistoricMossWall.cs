using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AncientSwampBiome
{
    public class PrehistoricMossWall : ModWall 
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(62, 89, 42));
            DustType = DustID.Grass;
        }
    }
}