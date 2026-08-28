using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
    public class ScaleSofa : ModTile
    {
        public const int NextStyleHeight = 38;
        public const int NextStyleWidth = 54;
        
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

            TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.CanBeSatOnForNPCs[Type] = true; // Facilitates calling ModifySittingTargetInfo for NPCs
			TileID.Sets.CanBeSatOnForPlayers[Type] = true; // Facilitates calling ModifySittingTargetInfo for Players

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(96, 109, 78), Lang.GetItemName(ItemID.Sofa));
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            DustType = DustID.Bone;
			AdjTiles = [TileID.Benches];
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range
		}

        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            // It is very important to know that this is called on both players and NPCs, so do not use Main.LocalPlayer for example, use info.restingEntity
			Tile tile = Framing.GetTileSafely(i, j);

            info.TargetDirection = info.RestingEntity.direction;
			info.DirectionOffset = 0;

            float offset;

            if ((tile.TileFrameX % NextStyleWidth == 0 && info.TargetDirection == -1) || (tile.TileFrameX % NextStyleWidth == 36 && info.TargetDirection == 1))
            {
                offset = -8f;
            }
            else if ((tile.TileFrameX % NextStyleWidth == 0 && info.TargetDirection == 1) || (tile.TileFrameX % NextStyleWidth == 36 && info.TargetDirection == -1))
            {
                offset = 8f;
            }
            else
            {
                offset = 0f;
            }

            info.VisualOffset = new Vector2(offset, 0f);

            info.AnchorTilePosition.X = i;
			info.AnchorTilePosition.Y = j;

			if (tile.TileFrameY % NextStyleHeight == 0)
			{
				info.AnchorTilePosition.Y++;
			}
        }

        public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			{
				player.GamepadEnableGrappleCooldown();
				player.sitting.SitDown(player, i, j);
			}

			return true;
		}

        public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			
			if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			{
				return;
			}

			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<ScaleSofaItem>();
		}
    }
}