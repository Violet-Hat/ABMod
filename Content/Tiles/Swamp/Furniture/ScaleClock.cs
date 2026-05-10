using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ObjectInteractions;

namespace ABMod.Content.Tiles.Swamp.Furniture
{
	public class ScaleClock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.Clock[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16];
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(96, 109, 78), Language.GetText("ItemName.GrandfatherClock"));
			DustType = DustID.Bone;
			AdjTiles = [TileID.GrandfatherClocks];
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

		public override bool RightClick(int x, int y)
		{
			string morningOrEvening = Language.GetTextValue("GameUI.TimeAtMorning");

			// Get current time expressed as ticks since the start of day or night.
			double time = Main.time;

			if (!Main.dayTime)
			{
				// If it's night, we add 15 hours since day lasts that long
				time += 54000.0;
			}
			
			time = time / 86400.0 * 24.0; // Divide by seconds in a day * 24
			time = time - 7.5 - 12.0; // Subtract 19.5 to convert from time since morning start to midnight.

			// And finally add 24 if that subtracting put time negative
			if (time < 0.0)
			{
				time += 24.0;
			}

			if (time >= 12.0)
			{
				morningOrEvening = Language.GetTextValue("GameUI.TimePastMorning");
			}

			int hours = (int)time;
			double timeRemainder = time - hours; // Get the decimal points of time.
			timeRemainder = (int)(timeRemainder * 60.0); // multiply it by 60 to convert it to minutes

			// This could easily be replaced by timeRemainder.ToString()
			string minutes = string.Concat(timeRemainder);

			// if timeRemainder is a single digit, which would cause time to display as HH:M instead of HH:MM, add a leading "0" 
			if (timeRemainder < 10.0)
			{
				minutes = "0" + minutes;
			}

			// This is for AM/PM time rather than 24hour time
			if (hours > 12)
			{
				hours -= 12;
			}

			// 0AM = 12AM
			if (hours == 0)
			{
				hours = 12;
			}

			// Combine it all together to get a HH:MM output
			Main.NewText(Language.GetTextValue("CLI.Time", $"{hours}:{minutes} {morningOrEvening}"), 255, 240, 20);
			return true;
		}
	}
}