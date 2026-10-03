using System;
using ABMod.Common.Tiles;
using ABMod.Common.Visuals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ABMod.Common.Bases
{
    public class CustomTree : ModTile
    {
        //Overrides
        public virtual int WoodType => TileID.WoodBlock;
        public virtual int ValidGroundType => TileID.Grass;

        public virtual int TreeDustType => DustID.Grass;
        public virtual Color TreeColor => new(141, 107, 75);

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
			AddMapEntry(TreeColor, name);
			DustType = TreeDustType;
			HitSound = SoundID.Dig;
		}

        //Update tile because it was placed or the neighbor got nuked
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            /*
            Tile tile = Framing.GetTileSafely(i, j);
            short originalFrameX = tile.TileFrameX;
            short originalFrameY = tile.TileFrameY;

            //Check if we are a brach
            if (tile.TileFrameX == 216) //left
            {
                Tile right = Framing.GetTileSafely(i + 1, j);

                //If the right tile doesn't exist, the type isn't correct or the frame isn't valid, kill the branch
                if (!right.HasTile || right.TileType != Type || right.TileFrameX != 162 || right.TileFrameX != 180)
                {
                    WorldGen.KillTile(i, j);
                }
            }
            else if (tile.TileFrameX == 234) //Right
            {
                Tile left = Framing.GetTileSafely(i - 1, j);

                //If the left tile doesn't exist, the type isn't correct or the frame isn't valid, kill the branch
                if (!left.HasTile || left.TileType != Type || left.TileFrameX != 162 || left.TileFrameX != 198)
                {
                    WorldGen.KillTile(i, j);
                }
            }
            //If we are not a branch, proceed to check for regular trunk segments
            else
            {
                Tile up = Framing.GetTileSafely(i, j - 1);
                Tile down = Framing.GetTileSafely(i, j + 1);

                bool isCutFrame = tile.TileFrameY > 36;

                if (tile.TileFrameX == 126 && tile.TileFrameX == 144)
                {
                    isCutFrame = false;
                }

                //If there isn't a valid tile or ground, kill the tile
                if (down.TileType != ValidGroundType || down.TileType != Type)
                {
                    WorldGen.KillTile(i, j);
                }
                //Else change the frame to a cut frame if it's not the top of the tree or a cut segment
                else if (up.TileType != Type && (tile.TileFrameX != 0 || !isCutFrame))
                {
                    //Regular cut frames
                    if (tile.TileFrameX != 126 && tile.TileFrameX != 144)
                    {
                        tile.TileFrameY += 54;
                    }
                    else
                    {
                        short frameX = (short)((WorldGen.genRand.Next(2) * 18) + 252);
                        short frameY = (short)((WorldGen.genRand.Next(3) * 18) + 54);

                        tile.TileFrameX = frameX;
                        tile.TileFrameY = frameY;
                    }
                }
            }

            if (tile.TileFrameX != originalFrameX || tile.TileFrameY != originalFrameY)
            {
                WorldGen.TileFrame(i - 1, j);
                WorldGen.TileFrame(i + 1, j);
                WorldGen.TileFrame(i, j - 1);
                WorldGen.TileFrame(i, j + 1);
            }
            */

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

        //Place side tile
        public static void PlaceSideTile(int i, int j, int frameX, short frameY)
        {
            Tile sideTile = Framing.GetTileSafely(i, j);

            //Kill any plants
            if (sideTile.HasTile && sideTile.TileType == TileID.Plants)
            {
                WorldGen.KillTile(i, j);
            }

            WorldGen.PlaceTile(i, j, ModContent.TileType<CustomTree>(), true);
            sideTile.TileFrameX = (short)frameX;
            sideTile.TileFrameY = frameY;
        }

        //Let it grow
		public static bool Grow(int i, int j, int minSize, int maxSize, bool saplingExists = false)
        {
            //Check if there's a sapling and destroy it
			if(saplingExists)
			{
				WorldGen.KillTile(i, j, noItem: true);
				WorldGen.KillTile(i, j - 1, noItem: true);
				
				if(Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendTileSquare(-1, i, j - 1, 1, 2, TileChangeType.None);
				}
			}

            //Get a random height for it
			int height = WorldGen.genRand.Next(minSize, maxSize + 1);

			for(int k = 1; k < height; k++)
			{
				//If there´s a tile blocking it or it's not on the world, make it shorter
				if(SolidTile(i, j - k) || !WorldGen.InWorld(i, j - k))
				{
					height = k - 1;
					break;
				}
			}

            //Make sure the tile is valid
			if(!Framing.GetTileSafely(i, j).HasTile)
			{
                //Place the base of the tree
                WorldGen.PlaceTile(i, j, ModContent.TileType<CustomTree>(), true);

                //Acquire the tile
                Tile tile = Framing.GetTileSafely(i, j);

                //For the Net shenanigans
                bool placedLeftRoot = false;
                bool placedRightRoot = false;

                //Y frame
                short frameY = (short)(WorldGen.genRand.Next(3) * 18);

                //If the random hits the 33%, try placing roots
                if (WorldGen.genRand.NextBool(3))
                {
                    //Call random to choose between bidirectional, left or right
                    int rand = WorldGen.genRand.Next(3);

                    switch(rand)
                    {
                        case 0: //Bidirectional
                            tile.TileFrameX = 18;
                            tile.TileFrameY = frameY;

                            PlaceSideTile(i - 1, j, 72, frameY);
                            PlaceSideTile(i + 1, j, 90, frameY);

                            placedLeftRoot = true;
                            placedRightRoot = true;
                            break;
                            
                        case 1: //Left
                            tile.TileFrameX = 36;
                            tile.TileFrameY = frameY;

                            PlaceSideTile(i - 1, j, 72, frameY);
                            placedLeftRoot = true;
                            break;
                            
                        case 2: //Right
                            tile.TileFrameX = 54;
                            tile.TileFrameY = frameY;
                            
                            PlaceSideTile(i + 1, j, 90, frameY);
                            placedRightRoot = true;
                            break;
                    }
                }
                else
                {
                    tile.TileFrameX = 108;
                    tile.TileFrameY = frameY;
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
            bool branchCooldown = false;

            for(int numSegments = 1; numSegments < height; numSegments++)
            {
                //Place tile
                WorldGen.PlaceTile(i, j - numSegments, ModContent.TileType<CustomTree>(), true);

                //Acquire tile
                Tile tile = Framing.GetTileSafely(i, j - numSegments);

                //For the Net shenanigans
                bool placedLeftBranch = false;
                bool placedRightBranch = false;

                //Y frame
                short frameY = (short)(WorldGen.genRand.Next(3) * 18);

                //Regular segments
                if (numSegments < height - 1)
                {
                    if (!branchCooldown && WorldGen.genRand.NextBool(3)) //Branch segment
                    {
                        //50% chance for it being a large branch
                        int offset = WorldGen.genRand.NextBool() ? 36 : 0;

                        //Call random to choose between bidirectional, left or right
                        int rand = WorldGen.genRand.Next(3);

                        switch(rand)
                        {
                            case 0: //Bidirectional
                                tile.TileFrameX = 162;
                                tile.TileFrameY = frameY;

                                PlaceSideTile(i - 1, j - numSegments, 216 + offset, frameY);
                                PlaceSideTile(i + 1, j - numSegments, 234 + offset, frameY);

                                placedLeftBranch = true;
                                placedRightBranch = true;
                                break;
                                
                            case 1: //Left
                                tile.TileFrameX = 180;
                                tile.TileFrameY = frameY;

                                PlaceSideTile(i - 1, j - numSegments, 216 + offset, frameY);
                                placedLeftBranch = true;
                                break;
                                
                            case 2: //Right
                                tile.TileFrameX = 198;
                                tile.TileFrameY = frameY;
                                
                                PlaceSideTile(i + 1, j - numSegments, 234 + offset, frameY);
                                placedRightBranch = true;
                                break;
                        }

                        branchCooldown = true;
                    }
                    else //Trunk segment
                    {
                        //Choose a random X frame
                        short frameX = (short)((WorldGen.genRand.Next(3) * 18) + 108);

                        //If it's one of the alts, randomize Y to choose between six frames instead of three
                        if (frameX == 126 || frameX == 144)
                        {
                            frameY = (short)(WorldGen.genRand.Next(6) * 18);
                        }

                        //Set the tile frames
                        tile.TileFrameX = frameX;
                        tile.TileFrameY = frameY;

                        branchCooldown = false;
                    }
                }

                //If it's the last segment, make it the top of the tree
				if(numSegments == height - 1)
				{
                    tile.TileFrameX = 0;
                    tile.TileFrameY = frameY;
				}

                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendTileSquare(-1, i, j - numSegments, 1, 1, TileChangeType.None);

                    if (placedLeftBranch)
                        NetMessage.SendTileSquare(-1, i - 1, j - numSegments, 1, 1, TileChangeType.None);
                    
                    if (placedRightBranch)
                        NetMessage.SendTileSquare(-1, i + 1, j - numSegments, 1, 1, TileChangeType.None);
                }
            }

            return true;
        }

        //Check the tree
		private void CheckEntireTree(ref int i, ref int j)
		{
			while(Framing.GetTileSafely(i, j).TileType == Type)
			{
				j--;
			}
			
			j++;
		}

        //Kill the tile
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient || !fail)
                return;

            /*
            //In case of failure
			if(fail && effectOnly && !noItem)
			{
				(int x, int y) = (i, j);
				CheckEntireTree(ref x, ref y);
			}
			
			if(fail)
                return;
            
            //Acquire the tile
            Tile tile = Framing.GetTileSafely(i, j);
            
            //Drop item
			Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Vector2(i, j) * 16, ItemType);
			tile.HasTile = false;

            //Kill the tiles above and sides
            bool up = Framing.GetTileSafely(i, j - 1).TileType == TileType;
            bool isBase = (tile.TileFrameX > 0 && tile.TileFrameX < 72) || (tile.TileFrameX > 144 && tile.TileFrameX < 216);

            if (up)
            {
                WorldGen.KillTile(i, j - 1);
            }
            if (isBase)
            {
                bool left = Framing.GetTileSafely(i - 1, j).TileType == TileType;
                bool right = Framing.GetTileSafely(i + 1, j).TileType == TileType;

                if (left)
                {
                    WorldGen.KillTile(i - 1, j);
                }
                if (right)
                {
                    WorldGen.KillTile(i + 1, j);
                }
            }

            //If the tile below is part of the tree, change the frame to a cut frame
            Tile below = Framing.GetTileSafely(i, j - 1);

            if (below.TileType == TileType)
            {
                //Regular cut frames
                if (below.TileFrameX != 126 && below.TileFrameX != 144)
                {
                    below.TileFrameY += 54;
                }
                else
                {
                    short frameX = (short)((WorldGen.genRand.Next(2) * 18) + 252);
                    short frameY = (short)((WorldGen.genRand.Next(3) * 18) + 54);

                    below.TileFrameX = frameX;
                    below.TileFrameY = frameY;
                }
            }
            */
        }

        //Draw the tree
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            //Get the textures
            TreeTex1 ??= ModContent.Request<Texture2D>(Texture + "_Branches");
            TreeTex2 ??= ModContent.Request<Texture2D>(Texture + "_Tops");
            TreeTrunkTex = ModContent.Request<Texture2D>(Texture);

            //Get the tile on this position, color and offsets
            Tile tile = Framing.GetTileSafely(i, j);
			Color col = Lighting.GetColor(i, j);

            Vector2 pos = TileGlobal.TileCustomPosition(i, j);
            Vector2 trunkOffset = new(2, 0);
            
            //Time to draw the actual tree
            int frameX = tile.TileFrameX / 18;
            int frameY = tile.TileFrameY / 18;

			spriteBatch.Draw(
                TreeTrunkTex.Value,
                pos,
                new Rectangle(frameX * 22, frameY * 22, 20, 20),
                new Color(col.R, col.G, col.B, 255),
                0f,
                trunkOffset,
                1f,
                SpriteEffects.None,
                0f
            );

            //Draw top
            if (tile.TileFrameX == 0)
            {
                Vector2 topOffset = new(32, 64);

                //Tree wind sway
				Vector2 treeTopPos = pos;
				float sway = Main.instance.TilesRenderer.GetWindCycle(i, j, TreeSwayHelper.TreeWindCounter);
				treeTopPos.Y += Math.Abs(sway) * 2;

                Main.spriteBatch.Draw(
                    TreeTex2.Value,
                    treeTopPos,
                    new Rectangle(frameY * 82, 0, 80, 80),
                    new Color(col.R, col.G, col.B, 255),
                    sway * 0.04f,
                    topOffset,
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }

            //Draw large branches
            if (tile.TileFrameX > 234 && tile.TileFrameY < 54)
            {
                int x = tile.TileFrameX == 252 ? 0 : 42;
                Vector2 branchOffset = tile.TileFrameX == 252 ? new(24, 12) : new(0, 12);

                Main.spriteBatch.Draw(
                    TreeTex1.Value,
                    pos,
                    new Rectangle(x, frameY * 42, 40, 40),
                    new Color(col.R, col.G, col.B, 255),
                    0f,
                    branchOffset,
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }

            return false;
        }
    }
}