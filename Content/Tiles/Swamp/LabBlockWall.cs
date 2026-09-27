using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp
{
    public class LabBlockWallUnsafe : ModWall
    {
        public override string Texture => "ABMod/Content/Tiles/Swamp/LabBlockWall";

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(88, 89, 71));
            DustType = DustID.Stone;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override bool Drop(int i, int j, ref int type)
        {
            type = ModContent.ItemType<LabBlockWallItem>();
            return true;
        }
    }

    public class LabBlockWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(43, 48, 64));
            DustType = DustID.Stone;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}