using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using ABMod.Common;
﻿using ABMod.Items;
﻿using ABMod.Tiles.AncientSwampBiome;

namespace ABMod.Tiles.AncientSwampBiome.Trees
{
	public class Lep : ModTile
	{
		//Textures
		private Asset<Texture2D> TopTexture;
        private Asset<Texture2D> TopRootTexture;
		private Asset<Texture2D> BottomRootTexture;
        private Asset<Texture2D> StemTexture;

		public override void SetStaticDefaults()
		{
			//This makes the tile a tree trunk
			TileID.Sets.IsATreeTrunk[Type] = true;
			Main.tileAxe[Type] = true;
			
			Main.tileFrameImportant[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileBlockLight[Type] = false;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(66, 98, 35), name);
			DustType = DustID.Grass;
			HitSound = SoundID.Dig;
		}
		
		//Update tile because it was placed or the neighbor got nuked (set to false)
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			resetFrame = false;
			noBreak = true;
			return false;
		}
		
		//Check if tile is solid
		public static bool SolidTile(int i, int j) 
        {
            return Framing.GetTileSafely(i, j).HasTile && Main.tileSolid[Framing.GetTileSafely(i, j).TileType];
        }
		
		public static bool SolidTopTile(int i, int j) 
        {
            return Framing.GetTileSafely(i, j).HasTile && (Main.tileSolidTop[Framing.GetTileSafely(i, j).TileType] || 
            Main.tileSolid[Framing.GetTileSafely(i, j).TileType]);
        }
		
		//Let it grow
		public static bool Grow(int i, int j, int minSize, int maxSize, bool saplingExists = false)
		{
			//Check if there's a sapling and destroy it
			if(saplingExists)
			{
				WorldGen.KillTile(i, j, false, false, true);
				WorldGen.KillTile(i, j - 1, false, false, true);
				WorldGen.KillTile(i + 1, j, false, false, true);
				WorldGen.KillTile(i + 1, j - 1, false, false, true);
				
				if(Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j, 2, 1, TileChangeType.None);
				}
			}
			
			//Get a random height for it
			int height = WorldGen.genRand.Next(minSize, maxSize);
			for(int k = 1; k < height; k++)
			{
				//If there´s a tile blocking it or it's not on the world, make it shorter
				if(SolidTile(i, j - k) || !WorldGen.InWorld(i, j - k))
				{
					height = k - 2;
					break;
				}
			}
			
			//If it's too short, don't let it grow
			if(height < minSize)
			{
				return false;
			}
			
			//Make sure the tile is valid
			if((SolidTopTile(i, j + 1) || SolidTile(i, j + 1)) && !Framing.GetTileSafely(i, j).HasTile)
			{
				if((SolidTopTile(i + 1, j + 1) || SolidTile(i + 1, j + 1)) && !Framing.GetTileSafely(i, j).HasTile)
				{
					short YFrame = (short)(WorldGen.genRand.Next(6) * 18);
				
					WorldGen.PlaceTile(i, j, ModContent.TileType<Lep>(), true);
					Framing.GetTileSafely(i, j).TileFrameY = YFrame;
					Framing.GetTileSafely(i, j).TileFrameX = 36;
					
					WorldGen.PlaceTile(i + 1, j, ModContent.TileType<Lep>(), true);
					Framing.GetTileSafely(i + 1, j).TileFrameY = YFrame;
					Framing.GetTileSafely(i + 1, j).TileFrameX = 54;
				
					if (Main.netMode != NetmodeID.SinglePlayer)
					{
						NetMessage.SendTileSquare(-1, i, j, 1, 1, TileChangeType.None);
						NetMessage.SendTileSquare(-1, i + 1, j, 1, 1, TileChangeType.None);
					}
				}
			}
			//Else don't let it grow
			else
			{
				return false;
			}
			
			//Time to put the tile frames
			for(int numSegments = 1; numSegments < height; numSegments++)
			{
				//Place the tree segment with a random Y frame
				short YFrame = (short)(WorldGen.genRand.Next(6) * 18);
				
				WorldGen.PlaceTile(i, j - numSegments, ModContent.TileType<Lep>(), true);
                Framing.GetTileSafely(i, j - numSegments).TileFrameY = YFrame;
				
				WorldGen.PlaceTile(i + 1, j - numSegments, ModContent.TileType<Lep>(), true);
                Framing.GetTileSafely(i + 1, j - numSegments).TileFrameY = YFrame;
				
				//If it's the first segment, put the frame X to 0 & 18
				if(numSegments == 1)
				{
					Framing.GetTileSafely(i, j - numSegments).TileFrameX = 0;
					Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 18;
				}
				
				//If it's the middle segments randomize between three duos of X frames
				if(numSegments > 1 && numSegments < height - 1)
				{
					if(Main.rand.NextBool(4))
					{
						//Irregular trunk segment 1
						Framing.GetTileSafely(i, j - numSegments).TileFrameX = 144;
						Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 162;
					}
					else
					{
						if(Main.rand.NextBool(4))
						{
							//Irregular trunk segment 2
							Framing.GetTileSafely(i, j - numSegments).TileFrameX = 180;
							Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 198;
						}
						else
						{
							//Regular trunk segment
							Framing.GetTileSafely(i, j - numSegments).TileFrameX = 108;
							Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 126;
						}
					}
				}
				
				//If it's the last segment, make it the top of the tree
				if(numSegments == height - 1)
				{
					Framing.GetTileSafely(i, j - numSegments).TileFrameX = 72;
					Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 90;
				}
				
				if(Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j, 1, 1, TileChangeType.None);
					NetMessage.SendTileSquare(-1, i + 1, j, 1, 1, TileChangeType.None);
				}
			}
			
			return true;
		}
		
		//Effects (death)
		public override void NearbyEffects(int i, int j, bool closer)
		{
		}
		
		//Check the tree
		private void CheckEntireTree(ref int x, ref int y)
		{
			while(Main.tile[x, y].TileType == Type)
			{
				y--;
			}
			
			y++;
		}
		
		//Kill the tile
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			//In case of failure
			if(fail && effectOnly && !noItem)
			{
				(int x, int y) = (i, j);
				CheckEntireTree(ref x, ref y);
			}
			
			if(fail)
			{
				return;
			}
			
			//Drop item
			int NewItem = Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), (new Vector2(i, j) * 16), ModContent.ItemType<ScaleWoodItem>());
			Framing.GetTileSafely(i, j).HasTile = false;
			
			bool left = Framing.GetTileSafely(i - 1, j).TileType == ModContent.TileType<Lep>();
			bool right = Framing.GetTileSafely(i + 1, j).TileType == ModContent.TileType<Lep>();
			bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<Lep>();
			bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<Lep>();
			bool downer = Framing.GetTileSafely(i, j + 2).TileType == ModContent.TileType<Lep>();
			
			if (left)
				WorldGen.KillTile(i - 1, j);
			if (right)
				WorldGen.KillTile(i + 1, j);
			if (up)
				WorldGen.KillTile(i, j - 1);
			if(down && !downer)
				WorldGen.KillTile(i, j + 1);
			
			//If there's a remaining segment below, turn it into a cut segment
			int belowFrame = Framing.GetTileSafely(i, j + 1).TileFrameX;
			
			if(belowFrame < 216)
			{
				if(belowFrame == 72 || belowFrame == 108 || belowFrame == 144 || belowFrame == 180)
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 216;
				}
				else if(belowFrame == 90 || belowFrame == 126 || belowFrame == 162 || belowFrame == 198)
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 234;
				}
				else if(belowFrame == 0)
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 252;
				}
				else if(belowFrame == 18)
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 270;
				}
			}
		}
		
		//Draw the tree
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			//X frame 0 & 18 = Bottom 1 segment
			//X frame 36 & 54 = Bottom 2 segment
			//X frame 72 & 90 = Top Segment
			//X frame 108 & 126 = Normal trunk segment
			//X frame 144 & 162 = Irregular trunk segment 1
			//X frame 180 & 198 = Irregular trunk segment 2
			//X frame 216 & 234 = Cut segment
			//X frame 252 & 270 = Cut Bottom 1 segment
			//X frame 288 & 306 = Cut Bottom 2 segment
		
			//Get the textures
			TopTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/AncientSwampBiome/Trees/Lep_Top");
			TopRootTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/AncientSwampBiome/Trees/Lep_RootsTop");
			BottomRootTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/AncientSwampBiome/Trees/Lep_RootsBottom");
			StemTexture = ModContent.Request<Texture2D>(Texture);
			
			//Get the tile, color and offsets
			Tile tile = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);
			
			Vector2 TopRootOffSet = new Vector2(10, 0);
			Vector2 BottomRootOffSet = new Vector2(44, 0);
			Vector2 treeOffset = new Vector2(96, 144);
			
			//Frames sizes
			int frameSize = 16;
			int frameOff = 0;
			int frameSizeY = 16;
			
			Vector2 pos = TileGlobal.TileCustomPosition(i, j);
			
			//Time to draw the actual tree
			spriteBatch.Draw(StemTexture.Value, pos, new Rectangle(tile.TileFrameX + frameOff, tile.TileFrameY, frameSize, frameSizeY), new Color(col.R, col.G, col.B, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			
			//Draw the roots
			if(Framing.GetTileSafely(i, j).TileFrameX == 0)
			{
				Main.spriteBatch.Draw(TopRootTexture.Value, pos, new Rectangle(0, 0, 54, 34), new Color(col.R, col.G, col.B, 255), 0f, TopRootOffSet, 1f, SpriteEffects.None, 0f);
			}
			if(Framing.GetTileSafely(i, j).TileFrameX == 252)
			{
				Main.spriteBatch.Draw(TopRootTexture.Value, pos, new Rectangle(0, 18, 54, 34), new Color(col.R, col.G, col.B, 255), 0f, TopRootOffSet, 1f, SpriteEffects.None, 0f);
			}
			if(Framing.GetTileSafely(i, j).TileFrameX == 36)
			{
				Main.spriteBatch.Draw(BottomRootTexture.Value, pos, new Rectangle(0, 0, 124, 90), new Color(col.R, col.G, col.B, 255), 0f, BottomRootOffSet, 1f, SpriteEffects.None, 0f);
			}
			
			//Draw the top
			if(Framing.GetTileSafely(i, j).TileFrameX == 72)
			{
				Main.spriteBatch.Draw(TopTexture.Value, pos, new Rectangle(0, 0, 222, 158), new Color(col.R, col.G, col.B, 255), 0f, treeOffset, 1f, SpriteEffects.None, 0f);
			}
			
			return false;
		}
	}
}