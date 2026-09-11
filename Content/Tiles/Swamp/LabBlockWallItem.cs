using Terraria;
using Terraria.ModLoader;

namespace ABMod.Content.Tiles.Swamp
{
    public class LabBlockWallItemUnsafe : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<LabBlockWallUnsafe>());
            Item.width = 34;
			Item.height = 34;
			Item.maxStack = 9999;
        }
	}

	public class LabBlockWallItem : ModItem
	{
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<LabBlockWall>());
            Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
        }
	}
}