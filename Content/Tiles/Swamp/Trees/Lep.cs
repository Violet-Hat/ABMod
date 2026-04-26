using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ABMod.Common.Tiles;
using ABMod.Common.Visuals;

namespace ABMod.Content.Tiles.Swamp.Trees
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
			if (saplingExists)
			{
				WorldGen.KillTile(i, j, false, false, true);
				WorldGen.KillTile(i, j - 1, false, false, true);
				WorldGen.KillTile(i + 1, j, false, false, true);
				WorldGen.KillTile(i + 1, j - 1, false, false, true);
				
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j, 2, 2, TileChangeType.None);
				}
			}

            //Get a random height for it
			int height = WorldGen.genRand.Next(minSize, maxSize);

			for (int k = 1; k < height; k++)
			{
				//If there's a tile blocking it or it's not on the world, make it shorter
				if (SolidTile(i, j - k) || !WorldGen.InWorld(i, j - k))
				{
					height = k - 2;
					break;
				}
			}

            //If it's too short, don't let it grow
			if (height < minSize)
			{
				return false;
			}

            //Make sure the tile is valid
			if ((SolidTopTile(i, j + 1) || SolidTile(i, j + 1)) && !Framing.GetTileSafely(i, j).HasTile)
			{
				if((SolidTopTile(i + 1, j + 1) || SolidTile(i + 1, j + 1)) && !Framing.GetTileSafely(i, j).HasTile)
				{
                    //Place the base of the tree and adjust the frames
					WorldGen.PlaceTile(i, j, ModContent.TileType<Lep>(), true);
					WorldGen.PlaceTile(i + 1, j, ModContent.TileType<Lep>(), true);
                    Framing.GetTileSafely(i, j).TileFrameX = 18;
					Framing.GetTileSafely(i + 1, j).TileFrameX = 90;
				
					if (Main.netMode != NetmodeID.SinglePlayer)
					{
						NetMessage.SendTileSquare(-1, i, j, 1, 1, TileChangeType.None);
						NetMessage.SendTileSquare(-1, i + 1, j, 1, 1, TileChangeType.None);
					}
				}
			}
			else
			{
				return false;
			}

            //Time to put the tile frames
			for (int numSegments = 1; numSegments < height; numSegments++)
            {
                //Place the tree segment with a random X frame
				short YFrame = (short)((WorldGen.genRand.Next(6) * 18) + 18);
				
				WorldGen.PlaceTile(i, j - numSegments, ModContent.TileType<Lep>(), true);
                Framing.GetTileSafely(i, j - numSegments).TileFrameY = YFrame;
				
				WorldGen.PlaceTile(i + 1, j - numSegments, ModContent.TileType<Lep>(), true);
                Framing.GetTileSafely(i + 1, j - numSegments).TileFrameY = YFrame;
				
				//If it's the first segment, put the frame X to 0 & 72 and frame Y to 0 on both
				if (numSegments == 1)
                {
                    Framing.GetTileSafely(i, j - numSegments).TileFrameX = 0;
					Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 72;

                    Framing.GetTileSafely(i, j - numSegments).TileFrameY = 0;
					Framing.GetTileSafely(i + 1, j - numSegments).TileFrameY = 0;
                }
                //If it's the middle segments randomize between three types of Y frames
				if (numSegments > 1 && numSegments < height - 1)
                {
                    if (Main.rand.NextBool(4))
					{
						//Irregular trunk right
						Framing.GetTileSafely(i, j - numSegments).TileFrameX = 18;
						Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 90;
					}
					else if (Main.rand.NextBool(4))
					{
						
						//Irregular trunk left
						Framing.GetTileSafely(i, j - numSegments).TileFrameX = 36;
						Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 108;
                    }
					else
					{
						//Regular trunk
						Framing.GetTileSafely(i, j - numSegments).TileFrameX = 0;
						Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 72;
					}
                }

                //If it's the last segment, make it the top of the tree with X equals 36 & 108 and frame Y equals 0 on both
				if (numSegments == height - 1)
				{
					Framing.GetTileSafely(i, j - numSegments).TileFrameX = 36;
					Framing.GetTileSafely(i + 1, j - numSegments).TileFrameX = 108;

                    Framing.GetTileSafely(i, j - numSegments).TileFrameY = 0;
					Framing.GetTileSafely(i + 1, j - numSegments).TileFrameY = 0;
				}
				
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j - numSegments, 1, 1, TileChangeType.None);
					NetMessage.SendTileSquare(-1, i + 1, j - numSegments, 1, 1, TileChangeType.None);
				}
            }

            return true;
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
			Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Vector2(i, j) * 16, ModContent.ItemType<ScaleWoodItem>());
			Framing.GetTileSafely(i, j).HasTile = false;

            //Check segments
			bool left = Framing.GetTileSafely(i - 1, j).TileType == ModContent.TileType<Lep>();
			bool right = Framing.GetTileSafely(i + 1, j).TileType == ModContent.TileType<Lep>();
			bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<Lep>();
			bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<Lep>();
			bool floorCheck = Framing.GetTileSafely(i, j + 2).TileType != ModContent.TileType<Lep>();
			
			if (left)
				WorldGen.KillTile(i - 1, j);
			if (right)
				WorldGen.KillTile(i + 1, j);
			if (up)
				WorldGen.KillTile(i, j - 1);
			if (down && floorCheck)
            {
                WorldGen.KillTile(i, j + 1);
				down = false;
            }
			
			//If there's a remaining segment below, turn it into a cut segment
			if (down)
			{
				int belowFrameX = Framing.GetTileSafely(i, j + 1).TileFrameX;
				int belowFrameY = Framing.GetTileSafely(i, j + 1).TileFrameY;
				
				if(belowFrameX != 54 || belowFrameX != 126)
				{
                    short l = 54;
                    short r = 126;

                    Tile tile = Framing.GetTileSafely(i, j + 1);

					if(belowFrameY == 0)
					{
                        tile.TileFrameX = (belowFrameX == 0) ? l : r;
					}
					else
					{
						short FrameY = (short)((WorldGen.genRand.Next(3) * 18) + 18);

						tile.TileFrameX = (belowFrameX < 54) ? l : r;
						Framing.GetTileSafely(i, j + 1).TileFrameY = FrameY;
					}
				}
			}
		}

        //Draw the tree
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			//Get the textures
			TopTexture ??= ModContent.Request<Texture2D>(Texture + "_Top");
			TopRootTexture ??= ModContent.Request<Texture2D>(Texture + "_RootsTop");
			BottomRootTexture ??= ModContent.Request<Texture2D>(Texture + "_RootsBottom");
			StemTexture = ModContent.Request<Texture2D>(Texture);
			
			//Get the tile, color and offsets
			Tile tile = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);
			
			Vector2 pos = TileGlobal.TileCustomPosition(i, j);
			Vector2 TopRootOffSet = new Vector2(10, 0);
			Vector2 BottomRootOffSet = new Vector2(44, 0);
			Vector2 treeOffset = new Vector2(96, 114);

            //Time to draw the actual tree
			spriteBatch.Draw(StemTexture.Value, pos, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(col.R, col.G, col.B, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			bool topRoot = tile.TileFrameY == 0 && tile.TileFrameX == 0;
			bool cutTopRoot = tile.TileFrameY == 0 && tile.TileFrameX == 54;
			bool bottomRoot = tile.TileFrameY == 0 && tile.TileFrameX == 18;
			bool top = tile.TileFrameY == 0 && tile.TileFrameX == 36;
			
			//Draw the roots
			if(topRoot)
			{
				Main.spriteBatch.Draw(TopRootTexture.Value, pos, new Rectangle(0, 0, 54, 34), new Color(col.R, col.G, col.B, 255), 0f, TopRootOffSet, 1f, SpriteEffects.None, 0f);
			}
			if(cutTopRoot)
			{
				Main.spriteBatch.Draw(TopRootTexture.Value, pos, new Rectangle(0, 18, 54, 34), new Color(col.R, col.G, col.B, 255), 0f, TopRootOffSet, 1f, SpriteEffects.None, 0f);
			}
			if(bottomRoot)
			{
				Main.spriteBatch.Draw(BottomRootTexture.Value, pos, new Rectangle(0, 0, 124, 90), new Color(col.R, col.G, col.B, 255), 0f, BottomRootOffSet, 1f, SpriteEffects.None, 0f);
			}
			
			//Draw the top
			if(top)
			{
				//Tree wind sway, yaaay
				Vector2 treeTopPos = pos;
				float sway = Main.instance.TilesRenderer.GetWindCycle(i, j, TreeWindSway.GetTreeWindCounter());
				treeTopPos.Y += Math.Abs(sway) * 2;

				Main.spriteBatch.Draw(TopTexture.Value, treeTopPos, new Rectangle(0, 0, 222, 128), new Color(col.R, col.G, col.B, 255), sway * 0.04f, treeOffset, 1f, SpriteEffects.None, 0f);
			}

            return false;
        }
    }
}