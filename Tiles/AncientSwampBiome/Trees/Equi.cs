using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ABMod.Common;

namespace ABMod.Tiles.AncientSwampBiome.Trees
{
    public class Equi : ModTile
    {
        //This is a modified version of the custom tree code
        //Texture
        private Asset<Texture2D> StemTexture;

        public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileSolid[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileBlockLight[Type] = false;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(79, 126, 38), name);
			DustType = DustID.Grass;
			HitSound = SoundID.Dig;
		}

        //Update tile because it was placed or the neighbor got nuked
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
            Tile tileSafely = Framing.GetTileSafely(i, j + 1);
            
            if (!tileSafely.HasTile)
            {
                WorldGen.KillTile(i, j);
            }

            if (tileSafely.TileType != ModContent.TileType<PrehistoricMoss>() && tileSafely.TileType != ModContent.TileType<Equi>())
            {
                WorldGen.KillTile(i, j);
            }

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
		
		//Let it grow (used for initial worldgen placement)
		public static bool Grow(int i, int j, int minSize, int maxSize, bool saplingExists = false)
        {
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
				short XFrame = (short)((WorldGen.genRand.Next(4) * 18) + 18);

                WorldGen.PlaceTile(i, j, ModContent.TileType<Equi>(), true);
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

            for(int numSegments = 1; numSegments < height; numSegments++)
			{
				//Place the tree segment with a random X frame
				short XFrame = (short)((WorldGen.genRand.Next(10) * 18) + 90);
				WorldGen.PlaceTile(i, j - numSegments, ModContent.TileType<Equi>(), true);
				
				//Middle segments
				if (numSegments < height - 1)
                {
                    Framing.GetTileSafely(i, j - numSegments).TileFrameX = XFrame;
                }

                //If it's the last segment, make it the top of the tree
				if(numSegments == height - 1)
                {
                    XFrame = (short)((WorldGen.genRand.Next(5) * 18) + 270);
                    Framing.GetTileSafely(i, j - numSegments).TileFrameX = XFrame;
                }

                if(Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j - numSegments, 1, 1, TileChangeType.None);
				}
            }

            return true;
        }

        //Check the tree (bamboo)
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
			Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Vector2(i, j) * 16, ItemID.BambooBlock);
			Framing.GetTileSafely(i, j).HasTile = false;

            //If there's a remaining segment below
            bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<Equi>();
			int belowFrameX = Framing.GetTileSafely(i, j + 1).TileFrameX;
			
            if (down)
            {
                short XFrame = 0;

                if (belowFrameX > 72)
                {
                    XFrame = (short)((WorldGen.genRand.Next(5) * 18) + 270);
                    Framing.GetTileSafely(i, j + 1).TileFrameX = XFrame;
                }
                else
                {
                    Framing.GetTileSafely(i, j + 1).TileFrameX = XFrame;
                }
            }
        }

        //Check if a bamboo tile can be placed above the existing one
        public static bool CanItGrow(int x, int y)
        {
            if (!WorldGen.InWorld(x, y - 1))
            {
                return false;
            }

            if (Framing.GetTileSafely(x, y - 1).HasTile)
            {
                return false;
            }

            if (CountBamboo(x, y) >= 25)
            {
                return false;
            }

            return true;
        }

        //Count how many bamboo tiles exist
        public static int CountBamboo(int x, int y)
        {
            int count = 0;

            for (int j = 0; j < 25; j++)
            {
                if (Framing.GetTileSafely(x, y + j).TileType == (ushort)ModContent.TileType<Equi>())
                {
                    count++;
                }
            }

            return count;
        }

        //Used for the growth of the bamboo
        public override void RandomUpdate(int i, int j)
        {
            if (CanItGrow(i, j))
            {
                if (WorldGen.genRand.NextBool(10))
                {
                    short XFrame;

                    Tile tile = Main.tile[i, j];
                    Tile tileAbove = Main.tile[i, j - 1];

                    tileAbove.TileType = (ushort)ModContent.TileType<Equi>();
                    tileAbove.HasTile = true;
                    tileAbove.CopyPaintAndCoating(tile);

                    //Frame updates
                    XFrame = (short)((WorldGen.genRand.Next(5) * 18) + 270);
                    Framing.GetTileSafely(i, j - 1).TileFrameX = XFrame;

                    if (Framing.GetTileSafely(i, j).TileFrameX == 0)
                    {
                        XFrame = (short)((WorldGen.genRand.Next(4) * 18) + 18);
                        Framing.GetTileSafely(i, j).TileFrameX = XFrame;
                    }
                    else
                    {
                        XFrame = (short)((WorldGen.genRand.Next(10) * 18) + 90);
                        Framing.GetTileSafely(i, j).TileFrameX = XFrame;
                    }

                    //Netmode comedy
                    if (Main.netMode == NetmodeID.Server)
                    {
						NetMessage.SendTileSquare(-1, i, j - 1);
					}
                }
            }
        }

        //Draw the tree
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            StemTexture = ModContent.Request<Texture2D>(Texture);

            //Get the tile, color and offsets
			Tile tile = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);

            Vector2 pos = TileGlobal.TileCustomPosition(i, j);
            Vector2 stemOffset = new Vector2(8, 0);

            //Time to draw the actual tree
            int frame = tile.TileFrameX / 18;
			spriteBatch.Draw(StemTexture.Value, pos, new Rectangle(frame * 34, tile.TileFrameY, 32, 16), col, 0f, stemOffset, 1f, SpriteEffects.None, 0f);

            return false;
        }
    }
}