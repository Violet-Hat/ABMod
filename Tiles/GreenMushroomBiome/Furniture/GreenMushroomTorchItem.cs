using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using ABMod.Tiles.GreenMushroomBiome;

namespace ABMod.Tiles.GreenMushroomBiome.Furniture
{
	public class GreenMushroomTorchItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.Torches[Type] = true;
			ItemID.Sets.WaterTorches[Type] = true;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ShimmerTorch;

			Item.ResearchUnlockCount = 100;
		}
		
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.holdStyle = 1;
			Item.value = Item.buyPrice(copper: 30);
			Item.DefaultToPlaceableTile(ModContent.TileType<GreenMushroomTorch>());
		}
		
		public override void HoldItem(Player player)
		{
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			float divide = 300f;
			Lighting.AddLight(position, new Vector3(90f / divide, 150f / divide, 120f / divide));
		}
		
		public override void PostUpdate()
		{
			if(!Item.wet)
			{
				Lighting.AddLight(Item.Center, 1f, 1f, 1f);
			}
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(5)
			.AddIngredient(ModContent.ItemType<MushroomItem>(), 1)
			.AddIngredient(ItemID.Torch, 5)
			.Register();
		}
	}
}