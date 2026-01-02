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
using ABMod.Items;
using ABMod.Tiles.AncientSwampBiome;

namespace ABMod.Tiles.AncientSwampBiome.Trees
{
	public class Skinny : ModTile
	{
		//Textures
		private Asset<Texture2D> TopTexture;
        private Asset<Texture2D> RootTexture;
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
			AddMapEntry(new Color(96, 81, 45), name);
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
				short XFrame = (short)(WorldGen.genRand.Next(3) * 18);

                WorldGen.PlaceTile(i, j, ModContent.TileType<Skinny>(), true);
                Framing.GetTileSafely(i, j).TileFrameX = XFrame;
                
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendTileSquare(-1, i, j, 1, 1, TileChangeType.None);
                }
			}
			else
			{
				return false;
			}
			
			//Time to put the tile frames
            int barkFrames = (int)(height / 2.5f);

			for(int numSegments = 1; numSegments < height; numSegments++)
			{
				//Place the tree segment with a random X frame
				short XFrame = (short)((WorldGen.genRand.Next(3) * 18) + 54);
				WorldGen.PlaceTile(i, j - numSegments, ModContent.TileType<Skinny>(), true);
				
				//If it's the middle segments change the X and Y frames to fit the situation
				if (numSegments < height - 1)
				{
					if (numSegments != barkFrames)
                    {
                        if (numSegments < barkFrames)
                        {
                            if (Main.rand.NextBool(4))
                            {
                                //Irregular trunk
                                XFrame = (short)((WorldGen.genRand.Next(2) * 18) + 108);
                                Framing.GetTileSafely(i, j - numSegments).TileFrameX = XFrame;
                            }
                            else
                            {
                                Framing.GetTileSafely(i, j - numSegments).TileFrameX = XFrame;
                            }
                        }
                        else
                        {
                            Framing.GetTileSafely(i, j - numSegments).TileFrameY = 18;
                            Framing.GetTileSafely(i, j - numSegments).TileFrameX = XFrame;
                        }
                    }
                    else
                    {
                        XFrame = (short)(WorldGen.genRand.Next(3) * 18);
                        Framing.GetTileSafely(i, j - numSegments).TileFrameY = 18;
                        Framing.GetTileSafely(i, j - numSegments).TileFrameX = XFrame;
                    }
				}
				
				//If it's the last segment, make it the top of the tree
				if(numSegments == height - 1)
				{
                    XFrame = (short)((WorldGen.genRand.Next(3) * 18) + 108);
					Framing.GetTileSafely(i, j - numSegments).TileFrameY = 18;
                    Framing.GetTileSafely(i, j - numSegments).TileFrameX = XFrame;
				}
				
				if(Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j - numSegments, 1, 1, TileChangeType.None);
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
			Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Vector2(i, j) * 16, ModContent.ItemType<ScaleWoodItem>());
			Framing.GetTileSafely(i, j).HasTile = false;

			bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<Skinny>();
            bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<Skinny>();
			
			if (up)
				WorldGen.KillTile(i, j - 1);
            if (down && Framing.GetTileSafely(i, j + 1).TileFrameY == 18)
            {
                WorldGen.KillTile(i, j + 1);
                down = false;
            }
			
			//If there's a remaining segment below, turn it into a cut segment
			int belowFrameX = Framing.GetTileSafely(i, j + 1).TileFrameX;
			int belowFrameY = Framing.GetTileSafely(i, j + 1).TileFrameY;
			
            if (down)
            {
                if (belowFrameY < 36)
                {
                    if (belowFrameX < 108)
                    {
                        Framing.GetTileSafely(i, j + 1).TileFrameY = 36;
                    }
                    else
                    {
                        short XFrame = (short)((WorldGen.genRand.Next(3) * 18) + 54);
                        Framing.GetTileSafely(i, j + 1).TileFrameY = 36;
                        Framing.GetTileSafely(i, j + 1).TileFrameX = XFrame;
                    }
                }
            }
		}
		
		//Draw the tree
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			//Get the textures
			TopTexture ??= ModContent.Request<Texture2D>(Texture + "_Top");
			RootTexture ??= ModContent.Request<Texture2D>(Texture + "_Root");
			StemTexture = ModContent.Request<Texture2D>(Texture);
			
			//Get the tile, color and offsets
			Tile tile = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);

            Vector2 pos = TileGlobal.TileCustomPosition(i, j);
            Vector2 rootOffset = new Vector2(0, -16);
			Vector2 topOffset = new Vector2(16, 0);

            //Time to draw the actual tree
			spriteBatch.Draw(StemTexture.Value, pos, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(col.R, col.G, col.B, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            int frame = tile.TileFrameX / 18;

            //Draw the roots
			if((Framing.GetTileSafely(i, j).TileFrameY == 0 || Framing.GetTileSafely(i, j).TileFrameY == 36) && Framing.GetTileSafely(i, j).TileFrameX < 54)
			{
				Main.spriteBatch.Draw(RootTexture.Value, pos, new Rectangle(frame * 18, 0, 16, 2), new Color(col.R, col.G, col.B, 255), 0f, rootOffset, 1f, SpriteEffects.None, 0f);
			}

            //Draw the tops
			if(Framing.GetTileSafely(i, j).TileFrameY == 18)
			{
				Main.spriteBatch.Draw(TopTexture.Value, pos, new Rectangle(frame * 50, 0, 48, 16), new Color(col.R, col.G, col.B, 255), 0f, topOffset, 1f, SpriteEffects.None, 0f);
			}

            return false;
		}
	}
}