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

        /*
        --List of X frames-- (Gotta update)
        Tree Top = 0;
        Tree Bottom (bidirectional) = 18;
        Tree Bottom (unidirectional)= 36;
        Tree Roots = 54;
        Tree Trunk = 72;
        Tree Trunk Alt 1 = 90;
        Tree Trunk Alt 2 = 108;
        Tree Branch Base (bidirectional)= 126;
        Tree Branch Base (unidirectional) = 144;
        Tree Branches Small = 162;
        Tree Branches Large = 180;
        */

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
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) => false;

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
                Tile tile = Framing.GetTileSafely(i, j);
                Tile left = Framing.GetTileSafely(i - 1, j);
                Tile right = Framing.GetTileSafely(i + 1, j);

                tile.TileType = (ushort)ModContent.TileType<CustomTree>();
     
                if((!left.HasTile || !right.HasTile) && WorldGen.genRand.NextBool(3))
                {
                    if (!left.HasTile && !right.HasTile)
                    {
                        if (WorldGen.genRand.NextBool()) // Bidirectional
                        {
                            tile.TileFrameX = 18;
                            tile.TileFrameY = (short)(WorldGen.genRand.Next(3) * 18);

                            /*Place roots
                            left.TileType = (ushort)ModContent.TileType<CustomTree>();
                            left.TileFrameX = 54;
                            left.TileFrameY = tile.TileFrameY;

                            right.TileType = (ushort)ModContent.TileType<CustomTree>();
                            right.TileFrameX = 54;
                            right.TileFrameY = tile.TileFrameY;
                            */
                        }
                        else //Unidirectional
                        {
                            if(WorldGen.genRand.NextBool()) //Left
                            {
                                tile.TileFrameX = 18;
                                tile.TileFrameY = (short)(WorldGen.genRand.Next(3) * 18);
                            }
                        }
                    }
                }
                else
                {
                    tile.TileFrameX = 72;
                    tile.TileFrameY = (short)(WorldGen.genRand.Next(3) * 18);
                }
                
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
            for(int numSegments = 1; numSegments < height; numSegments++)
            {
                //The first segment will be a regular trunk
                if (numSegments == 1)
                {
                    
                }
            }

            return true;
        }
    }
}