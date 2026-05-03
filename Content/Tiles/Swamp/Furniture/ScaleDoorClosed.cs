using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ObjectData;
using Terraria.GameContent.ObjectInteractions;
using Microsoft.Xna.Framework;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleDoorClosed : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.OpenDoorID[Type] = ModContent.TileType<ScaleDoorOpen>();
            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.ClosedDoor, 0));
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(96, 109, 78), Language.GetText("MapObject.Door"));
			DustType = DustID.Bone;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
            AdjTiles = [TileID.ClosedDoor];
		}

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) 
        {
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) 
        {
            num = 1;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<ScaleDoorItem>();
        }
	}
}