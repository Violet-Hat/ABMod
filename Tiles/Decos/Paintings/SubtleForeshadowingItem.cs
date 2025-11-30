using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Tiles.Decos.Paintings
{
	public class SubtleForeshadowingItem : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<SubtleForeshadowing>());
			Item.width = 16;
			Item.height = 16;
			Item.value = Item.buyPrice(silver: 25);
		}
	}
}