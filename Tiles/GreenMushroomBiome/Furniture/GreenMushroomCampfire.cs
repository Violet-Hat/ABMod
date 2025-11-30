﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;

namespace ABMod.Tiles.GreenMushroomBiome.Furniture
{
	public class GreenMushroomCampfire : ModTile
	{
		private Asset<Texture2D> flameTexture;
		
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileID.Sets.Campfire[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleWrapLimit = 16;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleLineSkip = 9;
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(13, 91, 44), name);
			AdjTiles = [TileID.Campfire];
			flameTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/GreenMushroomBiome/Furniture/GreenMushroomCampfire_Flame");
		}
		
		public override void NearbyEffects(int i, int j, bool closer) {
			if (closer)
			{
				return;
			}

			if (Main.tile[i, j].TileFrameY < 36)
			{
				Main.SceneMetrics.HasCampfire = true;
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
			player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type, style);
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override bool RightClick(int i, int j)
		{
			SoundEngine.PlaySound(SoundID.Mech, new Vector2(i * 16, j * 16));
			ToggleTile(i, j);
			return true;
		}

		public override void HitWire(int i, int j)
		{
			ToggleTile(i, j);
		}

		public void ToggleTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int topX = i - tile.TileFrameX % 54 / 18;
			int topY = j - tile.TileFrameY % 36 / 18;

			short frameAdjustment = (short)(tile.TileFrameY >= 36 ? -36 : 36);

			for(int x = topX; x < topX + 3; x++)
			{
				for(int y = topY; y < topY + 2; y++)
				{
					Main.tile[x, y].TileFrameY += frameAdjustment;
					if(Wiring.running)
					{
						Wiring.SkipWire(x, y);
					}
				}
			}

			if(Main.netMode != NetmodeID.SinglePlayer)
			{
				NetMessage.SendTileSquare(-1, topX, topY, 3, 2);
			}
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if(++frameCounter >= 4)
			{
				frameCounter = 0;
				frame = ++frame % 8;
			}
		}
		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			var tile = Main.tile[i, j];
			if(tile.TileFrameY < 36)
			{
				frameYOffset = Main.tileFrame[type] * 36;
			}
			else
			{
				frameYOffset = 252;
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if(tile.TileFrameY < 36)
			{
				float divide = 300f;

                r = 90f / divide;
                g = 150f / divide;
                b = 120f / divide;
			}
		}
		
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
			var tile = Main.tile[i, j];

			if(!TileDrawing.IsVisible(tile))
			{
				return;
			}

			if(tile.TileFrameY < 36)
			{
				Color col = Lighting.GetColor(i, j);
				Color color = new Color(col.R, col.G, col.B, 255);

				Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

				int width = 16;
				int offsetY = 0;
				int height = 16;
				short frameX = tile.TileFrameX;
				short frameY = tile.TileFrameY;
				int addFrX = 0;
				int addFrY = 0;

				TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);
				TileLoader.SetAnimationFrame(Type, i, j, ref addFrX, ref addFrY);
				
				Rectangle drawRectangle = new Rectangle(tile.TileFrameX, tile.TileFrameY + addFrY, 16, 16);
				spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + offsetY) + zero, drawRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}