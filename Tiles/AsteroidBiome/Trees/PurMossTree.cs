using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using ABMod.Tiles.AsteroidBiome;
using ABMod.Tiles.AsteroidBiome.Moss;
using ABMod.Common;

namespace ABMod.Tiles.AsteroidBiome.Trees
{
	public class PurMossTree : ModTile
	{
		/*
		Y = 0
		X frame 0 = Bottom tree segment
		X frame 18, 36, 54 = Normal tree segment
		X frame 72 = Top tree segment
		X frame 90, 108, 126 = Cut tree segment
		
		Y = 18
		X frame 0 = Bottom tree segment
		X frame 18, 36, 54 = Normal tree segment
		X frame 72 = Top tree segment
		X frame 90, 108, 126 = Cut tree segment
		*/
		
		public Asset<Texture2D> StemTexture;
		public Asset<Texture2D> TopTexture;
		public Asset<Texture2D> LightTexture;
		
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
			HitSound = SoundID.Dig;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(90, 31, 163), name);
			DustType = DustID.DemonTorch;
		}
		
		//Let it glow only on the top
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(Framing.GetTileSafely(i, j).TileFrameX == 72)
			{
				r = 0.50f;
				g = 0.50f;
				b = 0.75f;
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
				WorldGen.PlaceTile(i, j, ModContent.TileType<PurMossTree>(), true);
				
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
				WorldGen.PlaceTile(i, j - numSegments, ModContent.TileType<PurMossTree>(), true);
                Framing.GetTileSafely(i, j - numSegments).TileFrameX = (short)((WorldGen.genRand.Next(2) + 1) * 18);
				
				//If it's the middle segments change the Y frame
				if(numSegments > 4 && numSegments < height - 1)
				{
					Framing.GetTileSafely(i, j - numSegments).TileFrameY = 18;
					
					if(numSegments == 5)
					{
						Framing.GetTileSafely(i, j - numSegments).TileFrameX = 0;
					}
				}
				
				//If it's the last segment, make it the top of the tree
				if(numSegments == height - 1)
				{
					Framing.GetTileSafely(i, j - numSegments).TileFrameX = 72;
				}
				
				//If it's the one above the root segment, make it a regular trunk segment
				if(Framing.GetTileSafely(i, j - numSegments + 1).TileType != ModContent.TileType<AsteroidMossPur>() && Framing.GetTileSafely(i, j - numSegments).TileFrameX == 0 && Framing.GetTileSafely(i, j - numSegments).TileFrameY == 0)
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
			//int NewItem = Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), (new Vector2(i, j) * 16), ModContent.ItemType<Yes>());
			Framing.GetTileSafely(i, j).HasTile = false;
			
			bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<PurMossTree>();
			
			if (up)
				WorldGen.KillTile(i, j - 1);
			
			//If there's a remaining segment below, turn it into a cut segment
			int belowFrame = Framing.GetTileSafely(i, j + 1).TileFrameX;
			if(belowFrame < 90)
			{
				if(belowFrame == 0)
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 90;
				}
				else if(belowFrame == 18)
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 108;
				}
				else if(belowFrame == 36)
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 126;
				}
				else
				{
					Framing.GetTileSafely(i, j + 1).TileFrameX = 144;
				}
			}
		}
		
		//Draw the tree
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			//Get the textures
			TopTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/AsteroidBiome/Trees/PurMossOrb");
			LightTexture ??= ModContent.Request<Texture2D>("ABMod/Tiles/AsteroidBiome/Trees/PurMossOrb_Light");
			StemTexture = ModContent.Request<Texture2D>(Texture);
			
			//Get the tile, color and offsets
			Tile tile = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);
			
			float xOff = (float)Math.Sin((j * 19) * 0.04f) * 1.2f;

            if (xOff == 1 && (j / 4f) == 0)
            {
                xOff = 0;
            }
			
			Vector2 pos = TileGlobal.TileCustomPosition(i, j);
			
			Vector2 stemOffset = new Vector2((xOff * 2) + 2, 2);
			Vector2 treeOffset = new Vector2((xOff * 2) + 32, 64);
			
			//Time to draw the actual tree
			int frame = tile.TileFrameX / 18;
			int frameY = tile.TileFrameY / 18;
			
			spriteBatch.Draw(StemTexture.Value, pos, new Rectangle(frame * 22, frameY * 22, 20, 20), col, 0f, stemOffset, 1f, SpriteEffects.None, 0f);
			
			if(Framing.GetTileSafely(i, j).TileFrameX == 72)
			{
				spriteBatch.Draw(TopTexture.Value, pos, new Rectangle(0, 0, 82, 82), col, 0f, treeOffset, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(LightTexture.Value, pos, new Rectangle(0, 0, 82, 82), Color.White * 0.75f, 0f, treeOffset, 1f, SpriteEffects.None, 0f);
			}
			
			return false;
		}
	}
}