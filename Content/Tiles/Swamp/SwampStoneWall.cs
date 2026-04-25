using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp
{
    public class SwampStoneWall : ModWall 
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(38, 34, 29));
            DustType = DustID.Stone;
        }
    }
}