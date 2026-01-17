using Terraria;
using Terraria.ModLoader;

namespace ABMod.Tiles.AncientSwampBiome.Traps
{
    public class PitfallTrapItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<PitfallTrap>(), WorldGen.genRand.Next(2));
            Item.width = 30;
            Item.height = 16;
        }
    }
}