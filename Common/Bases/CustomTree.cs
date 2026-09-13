using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ABMod.Common.Bases
{
    public class CustomTree : ModTile
    {
        //Textures
		protected Asset<Texture2D> TreeTex1; //Use it as desired, in this case the branches
        protected Asset<Texture2D> TreeTex2; //Use it as desired, in this case the tops
        protected Asset<Texture2D> TreeTrunkTex; //Trunk

        public override void SetStaticDefaults()
		{
			//This makes the tile a tree trunk
			TileID.Sets.IsATreeTrunk[Type] = true;
			Main.tileAxe[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = false;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(141, 107, 75), name);
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

        //Check if the tile is solid
		public static bool SolidTile(int i, int j) 
        {
            return Framing.GetTileSafely(i, j).HasTile &&
            Main.tileSolid[Framing.GetTileSafely(i, j).TileType];
        }
		
		public static bool SolidTopTile(int i, int j) 
        {
            return Framing.GetTileSafely(i, j).HasTile &&
            (Main.tileSolidTop[Framing.GetTileSafely(i, j).TileType] || Main.tileSolid[Framing.GetTileSafely(i, j).TileType]);
        }

        //Place roots
        public static void PlaceBidirectionalRoot(Tile tile, Tile left, Tile right, int i, int j, short frameY)
        {
            tile.TileFrameX = 18;
            tile.TileFrameY = frameY;
            
            WorldGen.PlaceTile(i - 1, j, ModContent.TileType<CustomTree>(), true);
            left.TileFrameX = 72;
            left.TileFrameY = frameY;
            
            WorldGen.PlaceTile(i + 1, j, ModContent.TileType<CustomTree>(), true);
            right.TileFrameX = 90;
            right.TileFrameY = frameY;
        }

        public static void PlaceLeftRoot(Tile tile, Tile left, int i, int j, short frameY)
        {
            tile.TileFrameX = 36;
            tile.TileFrameY = frameY;
            
            WorldGen.PlaceTile(i - 1, j, ModContent.TileType<CustomTree>(), true);
            left.TileFrameX = 72;
            left.TileFrameY = frameY;
        }

        public static void PlaceRightRoot(Tile tile, Tile right, int i, int j, short frameY)
        {
            tile.TileFrameX = 54;
            tile.TileFrameY = frameY;

            WorldGen.PlaceTile(i + 1, j, ModContent.TileType<CustomTree>(), true);
            right.TileFrameX = 90;
            right.TileFrameY = frameY;
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
					NetMessage.SendTileSquare(-1, i, j - 1, 1, 2, TileChangeType.None);
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
                //Place the base of the tree
                WorldGen.PlaceTile(i, j, ModContent.TileType<CustomTree>(), true);

                //Acquire the tiles on center, left and right
                Tile tile = Framing.GetTileSafely(i, j);
                Tile left = Framing.GetTileSafely(i - 1, j);
                Tile right = Framing.GetTileSafely(i + 1, j);

                //For the Net shenanigans
                bool placedLeftRoot = false;
                bool placedRightRoot = false;

                //Root checks. If there's no tile and the tile below is solid, it is valid
                bool canPlaceLeftRoot = !left.HasTile && SolidTile(i - 1, j + 1);
                bool canPlaceRightRoot = !right.HasTile && SolidTile(i + 1, j + 1);

                //If there's roots can be placed and the random hits the 33%, try placing roots
                if ((canPlaceLeftRoot || canPlaceRightRoot) && WorldGen.genRand.NextBool(3))
                {
                    //Y frame
                    short frameY = (short)(WorldGen.genRand.Next(3) * 18);

                    //Bidirectional check
                    bool canBeBidirectional = canPlaceLeftRoot && canPlaceLeftRoot;

                    //If it can be bidirectional, call random to choose between bidirectional, left or right
                    if (canBeBidirectional)
                    {
                        int rand = WorldGen.genRand.Next(3);
                        switch(rand)
                        {
                            case 0: //Bidirectional
                                PlaceBidirectionalRoot(tile, left, right, i, j, frameY);
                                placedLeftRoot = true;
                                placedRightRoot = true;
                                break;
                            
                            case 1: //Left
                                PlaceLeftRoot(tile, left, i, j, frameY);
                                placedLeftRoot = true;
                                break;
                            
                            case 2: //Right
                                PlaceRightRoot(tile, right, i, j, frameY);
                                placedRightRoot = true;
                                break;
                        }
                    }
                    else if (canPlaceLeftRoot) //Just place left
                    {
                        PlaceLeftRoot(tile, left, i, j, frameY);
                        placedLeftRoot = true;
                    }
                    else //Just place right
                    {
                        PlaceRightRoot(tile, right, i, j, frameY);
                        placedRightRoot = true;
                    }
                }
                
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendTileSquare(-1, i, j, 1, 1, TileChangeType.None);

                    if (placedLeftRoot)
                        NetMessage.SendTileSquare(-1, i - 1, j, 1, 1, TileChangeType.None);
                    
                    if (placedRightRoot)
                        NetMessage.SendTileSquare(-1, i + 1, j, 1, 1, TileChangeType.None);
                }
			}
			else
			{
				return false;
			}

            //Time to put the tile frames
            for(int numSegments = 1; numSegments < height; numSegments++)
            {
                //Place tile
                WorldGen.PlaceTile(i, j - numSegments, ModContent.TileType<CustomTree>(), true);

                Tile tile = Framing.GetTileSafely(i, j - numSegments);
                Tile left = Framing.GetTileSafely(i - 1, j - numSegments);
                Tile right = Framing.GetTileSafely(i + 1, j - numSegments);

                bool canPlaceBranch = !left.HasTile && !right.HasTile;

                if (canPlaceBranch && WorldGen.genRand.NextBool(3)) //Branch segment
                {
                    
                }
                else //Trunk segment
                {
                    
                }

                if(Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendTileSquare(-1, i, j - numSegments, 1, 1, TileChangeType.None);
            }

            return true;
        }
    }
}