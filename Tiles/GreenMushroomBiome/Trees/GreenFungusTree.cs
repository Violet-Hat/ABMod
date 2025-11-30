using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using ABMod.Tiles.GreenMushroomBiome;
using ABMod.Common;

namespace ABMod.Tiles.GreenMushroomBiome.Trees
{
	public class GreenFungusTree : ModTile
	{
		/*
		X frame 0 = Root segment
		X frame 18 = Normal tree segment
		X frame 36 = Top segment
		X frame 54 = Fungus segment
		X frame 72 = Branches segment
		X frame 90 = Cut segment
		X frame 108 = Cut segment with root
		*/

		//Textures
		private Asset<Texture2D> TopTexture;
		private Asset<Texture2D> BranchTexture;
        private Asset<Texture2D> FungusTexture;
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
			AddMapEntry(new Color(10, 49, 49), name);
			DustType = DustID.GreenTorch;
			HitSound = SoundID.Dig;
		}
		
		//Let it glow only on the top
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(Framing.GetTileSafely(i, j).TileFrameX == 36)
			{
				r = 0.15f;
				g = 0.35f;
				b = 0.30f;
			}
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
				WorldGen.PlaceTile(i, j, ModContent.TileType<GreenFungusTree>(), true);
				Framing.GetTileSafely(i, j).TileFrameY = (short)(WorldGen.genRand.Next(6) * 18);
				
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j, 1, 1, TileChangeType.None);
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
				WorldGen.PlaceTile(i, j - numSegments, ModContent.TileType<GreenFungusTree>(), true);
                Framing.GetTileSafely(i, j - numSegments).TileFrameY = (short)(WorldGen.genRand.Next(6) * 18);
				
				//If it's the middle segments randomize between three X frames
				if(numSegments > 1 && numSegments < height - 1)
				{
					if(Main.rand.NextBool(6))
					{
						//Branch segments
						Framing.GetTileSafely(i, j - numSegments).TileFrameX = 72;
					}
					else
					{
						if(Main.rand.NextBool(4))
						{
							//Fungus segments
							Framing.GetTileSafely(i, j - numSegments).TileFrameX = 54;
						}
						else
						{
							//Regular trunk segment
							Framing.GetTileSafely(i, j - numSegments).TileFrameX = 18;
						}
					}
				}
				
				//If it's the last segment, make it the top of the tree
				if(numSegments == height - 1)
				{
					Framing.GetTileSafely(i, j - numSegments).TileFrameX = 36;
				}
				
				//If it's the one above the root segment, make it a regular trunk segment
				if(Framing.GetTileSafely(i, j - numSegments + 1).TileType != ModContent.TileType<MushroomPasture>() && Framing.GetTileSafely(i, j - numSegments).TileFrameX == 0)
				{
					Framing.GetTileSafely(i, j - numSegments).TileFrameX = 18;
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
			int NewItem = Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), (new Vector2(i, j) * 16), ModContent.ItemType<MushroomItem>());
			Framing.GetTileSafely(i, j).HasTile = false;
			
			bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<GreenFungusTree>();
			
			if (up)
				WorldGen.KillTile(i, j - 1);
			
			//If there's a remaining segment below, turn it into a cut segment
			int belowFrame = Framing.GetTileSafely(i, j + 1).TileFrameX;
			
			if(belowFrame < 90)
			{
				if(belowFrame == 0)
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 108;
				}
				else
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 90;
				}
			}
		}
		
		//Draw the tree
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			//Get the textures
			TopTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/GreenMushroomBiome/Trees/GMTree_Top");
			RootTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/GreenMushroomBiome/Trees/GMTree_Root");
			FungusTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/GreenMushroomBiome/Trees/GMTree_Fungus");
			BranchTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/GreenMushroomBiome/Trees/GMTree_Branches");
			StemTexture = ModContent.Request<Texture2D>(Texture);
			
			//Get the tile, color and offsets
			Tile tile = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);
			
			Vector2 branchOffSetL = new Vector2(18, 0);
			Vector2 branchOffSetR = new Vector2(-16, 0);
			Vector2 fungusOffSet = new Vector2(2, 0);
			Vector2 rootOffSet = new Vector2(6, -10);
			Vector2 treeOffset = new Vector2(26, 50);
			
			//Frames sizes
			int frameSize = 16;
			int frameOff = 0;
			int frameSizeY = 16;
			
			Vector2 pos = TileGlobal.TileCustomPosition(i, j);
			
			//Time to draw the actual tree
			spriteBatch.Draw(StemTexture.Value, pos, new Rectangle(tile.TileFrameX + frameOff, tile.TileFrameY, frameSize, frameSizeY), col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			
			int frame = tile.TileFrameY / 18;
			
			//Draw branches
			if(Framing.GetTileSafely(i, j).TileFrameX == 72)
			{
				//Left
				if(Framing.GetTileSafely(i, j).TileFrameY == 0 || Framing.GetTileSafely(i, j).TileFrameY == 18 || Framing.GetTileSafely(i, j).TileFrameY == 36)
				{
					Main.spriteBatch.Draw(BranchTexture.Value, pos, new Rectangle(0, frame * 18, 18, 16), new Color(col.R, col.G, col.B, 255), 0f, branchOffSetL, 1f, SpriteEffects.None, 0f);
				}
				
				//Right
				if(Framing.GetTileSafely(i, j).TileFrameY == 54 || Framing.GetTileSafely(i, j).TileFrameY == 72 || Framing.GetTileSafely(i, j).TileFrameY == 90)
				{
					Main.spriteBatch.Draw(BranchTexture.Value, pos, new Rectangle(0, frame * 18, 18, 16), new Color(col.R, col.G, col.B, 255), 0f, branchOffSetR, 1f, SpriteEffects.None, 0f);
				}
			}
			
			//Draw fungus
			if(Framing.GetTileSafely(i, j).TileFrameX == 54)
			{
				//Left
				if(Framing.GetTileSafely(i, j).TileFrameY == 0 || Framing.GetTileSafely(i, j).TileFrameY == 18 || Framing.GetTileSafely(i, j).TileFrameY == 36)
				{
					Main.spriteBatch.Draw(FungusTexture.Value, pos, new Rectangle(0, frame * 18, 20, 16), new Color(col.R, col.G, col.B, 255), 0f, fungusOffSet, 1f, SpriteEffects.None, 0f);
				}
				
				//Right
				if(Framing.GetTileSafely(i, j).TileFrameY == 54 || Framing.GetTileSafely(i, j).TileFrameY == 72 || Framing.GetTileSafely(i, j).TileFrameY == 90)
				{
					Main.spriteBatch.Draw(FungusTexture.Value, pos, new Rectangle(0, frame * 18, 20, 16), new Color(col.R, col.G, col.B, 255), 0f, fungusOffSet, 1f, SpriteEffects.None, 0f);
				}
			}
			
			//Draw roots
			if(Framing.GetTileSafely(i, j).TileFrameX == 0 || Framing.GetTileSafely(i, j).TileFrameX == 108)
			{
				//First variant
				if(Framing.GetTileSafely(i, j).TileFrameY == 0 || Framing.GetTileSafely(i, j).TileFrameY == 18 || Framing.GetTileSafely(i, j).TileFrameY == 36)
				{
					Main.spriteBatch.Draw(RootTexture.Value, pos, new Rectangle(0, 0, 28, 10), new Color(col.R, col.G, col.B, 255), 0f, rootOffSet, 1f, SpriteEffects.None, 0f);
				}
				
				//Second variant
				if(Framing.GetTileSafely(i, j).TileFrameY == 54 || Framing.GetTileSafely(i, j).TileFrameY == 72 || Framing.GetTileSafely(i, j).TileFrameY == 90)
				{
					Main.spriteBatch.Draw(RootTexture.Value, pos, new Rectangle(0, 12, 28, 10), new Color(col.R, col.G, col.B, 255), 0f, rootOffSet, 1f, SpriteEffects.None, 0f);
				}
			}
			
			//Draw the top
			if(Framing.GetTileSafely(i, j).TileFrameX == 36)
			{
				Main.spriteBatch.Draw(TopTexture.Value, pos, new Rectangle(0, 0, 68, 66), new Color(col.R, col.G, col.B, 255), 0f, treeOffset, 1f, SpriteEffects.None, 0f);
			}
			
			return false;
		}
	}
}