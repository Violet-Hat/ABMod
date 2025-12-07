using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ABMod.Tiles.AncientSwampBiome.Ambient
{
    public class LargeAncientStoneRock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.BreakableWhenPlacing[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.AnchorValidTiles = new int[]
            {
                ModContent.TileType<AncientStone>(),
            };
            TileObjectData.addTile(Type);
            HitSound = SoundID.Tink;
            DustType = DustID.Stone;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1: 3;
        }
	}
}