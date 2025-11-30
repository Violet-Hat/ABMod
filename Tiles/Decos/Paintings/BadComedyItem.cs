using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ABMod.Tiles.Decos.Paintings
{
	public class BadComedyItem : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<BadComedy>());
			Item.width = 16;
			Item.height = 16;
			Item.value = Item.buyPrice(silver: 25);
		}
	}
}