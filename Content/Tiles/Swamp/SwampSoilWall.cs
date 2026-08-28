using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp
{
    public class SwampSoilWall : ModWall 
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(64, 56, 41));
            DustType = DustID.Dirt;
        }
    }
}