using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ABMod.Tiles.AncientSwampBiome.Furniture
{
	public class ScaleClock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.Clock[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
			TileObjectData.newTile.Origin = new Point16(0, 4);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(96, 109, 78), name);
			DustType = DustID.Bone;
			AdjTiles = new int[] { TileID.GrandfatherClocks };
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) 
        {
            num = fail ? 1 : 3;
        }

		public override bool RightClick(int x, int y)
		{
			//Eldritch horror of a code
			string text = "AM";
			double time = Main.time;
			if (!Main.dayTime)
			{
				time += 54000.0;
			}

			time = time / 86400.0 * 24.0;
			time = time - 7.5 - 12.0;
			if (time < 0.0)
			{
				time += 24.0;
			}

			if (time >= 12.0)
			{
				text = "PM";
			}

			int intTime = (int)time;
			double deltaTime = time - intTime;
			deltaTime = (int)(deltaTime * 60.0);
			string text2 = string.Concat(deltaTime);
			if (deltaTime < 10.0)
			{
				text2 = "0" + text2;
			}

			if (intTime > 12)
			{
				intTime -= 12;
			}

			if (intTime == 0)
			{
				intTime = 12;
			}

			Main.NewText($"Time: {intTime}:{text2} {text}", 255, 240, 20);
			return true;
		}
		
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
			player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type, style);
		}
	}
}