using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp
{
    public class SwampDirtWallUnsafe : ModWall
    {
        public override string Texture => "ABMod/Content/Tiles/Swamp/SwampDirtWall";

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(51, 44, 31));
            DustType = DustID.Stone;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }

    public class SwampDirtWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(51, 44, 31));
            DustType = DustID.Stone;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}