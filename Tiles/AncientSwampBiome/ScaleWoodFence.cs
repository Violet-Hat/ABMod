using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Tiles.AncientSwampBiome
{
    public class ScaleWoodFence : ModWall 
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(48, 37, 29));
            DustType = DustID.Bone;
        }
    }
}