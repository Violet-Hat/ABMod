using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.Decos.Walls
{
    public class OddFungusWall : ModWall 
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(122, 68, 104));
            DustType = DustID.PurpleMoss;
        }
    }
}