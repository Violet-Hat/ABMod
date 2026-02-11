using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;

using ABMod.Tiles.AncientSwampBiome;
using ABMod.Tiles.AncientSwampBiome.Ambient;
using ABMod.Tiles.AncientSwampBiome.Trees;

namespace ABMod.Generation
{
	public class VegetationGen : ModSystem
    {
        //Biome positions
        public static int SwampStartX = 0;
        public static int SwampEndX = 0;
        public static int SwampStartY = 0;
        public static int SwampEndY = 0;

        private void SwampVegetation(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Adding plant life in the anomaly";

            for(int x = SwampStartX; x <= SwampEndX; x++)
			{
				for(int y = SwampStartY; y <= SwampEndY; y++)
				{
					Tile tile = Main.tile[x, y];
					Tile tileRight = Main.tile[x + 1, y];

					//Prehistoric moss ambient
					if(tile.TileType == (ushort)ModContent.TileType<PrehistoricMoss>() && !WorldgenTools.TooMuchLiquid(x, y, 3))
					{
						//Tree
						if(tileRight.TileType == (ushort)ModContent.TileType<PrehistoricMoss>())
						{
							if (WorldGen.genRand.NextBool(8))
							{
								if (WorldgenTools.GrowTreeCheck(x, y, 6, 35, 1))
								{
									WorldGen.SlopeTile(x, y, 0);
                                    WorldGen.SlopeTile(x + 1, y, 0);
									Lep.Grow(x, y - 1, 25, 30, false);
								}
							}
						}

						if (WorldGen.genRand.NextBool(6))
						{
							if (WorldgenTools.GrowSkinnyCheck(x, y, 5, 25))
							{
								WorldGen.SlopeTile(x, y, 0);
								Skinny.Grow(x, y - 1, 15, 20, false);
							}
						}

						//Ambient decorations
						if (!WorldgenTools.IsTreeType(x, y - 1))
                        {
							if (WorldGen.genRand.NextBool(3))
							{
								switch (WorldGen.genRand.Next(3))
								{
									default: 
										WorldGen.PlaceObject(x, y - 1, ModContent.TileType<LargeAmbientPlant>(), true, WorldGen.genRand.Next(9));
										break;
									case 1:
										WorldGen.PlaceObject(x, y - 1, ModContent.TileType<LargePrehistoricMossRock>(), true, WorldGen.genRand.Next(3));
										break;
									case 2:
										WorldGen.PlaceObject(x, y - 1, ModContent.TileType<MediumAmbientPlant>(), true, WorldGen.genRand.Next(12));
										break;
								}
							}

							if (WorldGen.genRand.NextBool(4))
							{
								switch (WorldGen.genRand.Next(3))
								{
									default: 
										WorldGen.PlaceObject(x, y - 1, ModContent.TileType<SmallCycad>(), true);
										break;
									case 1:
										WorldGen.PlaceObject(x, y - 1, ModContent.TileType<MediumCycad>(), true);
										break;
									case 2:
										WorldGen.PlaceObject(x, y - 1, ModContent.TileType<LargeCycad>(), true);
										break;
								}
							}

							//Grass
							if (WorldGen.genRand.NextBool(3))
							{
								WorldGen.PlaceObject(x, y - 1, ModContent.TileType<SwampGrass>(), true, WorldGen.genRand.Next(28));
							}
                        }
					}

					//Aquatic prehistoric moss ambience
					if(tile.TileType == (ushort)ModContent.TileType<PrehistoricMoss>() && WorldgenTools.TooMuchLiquid(x, y, 3))
					{
						if (WorldGen.genRand.NextBool(3))
						{
							if (WorldgenTools.GrowSkinnyCheck(x, y, 5, 30))
							{
								WorldGen.SlopeTile(x, y, 0);
								Equi.Grow(x, y - 1, 10, 25, false);
							}
						}
					}

					//Ancient dirt ambient
					if(tile.TileType == (ushort)ModContent.TileType<AncientDirt>())
                    {
						if (WorldGen.genRand.NextBool(3))
						{
							WorldGen.PlaceObject(x, y - 1, ModContent.TileType<SmallAncientDirtRock>(), true, WorldGen.genRand.Next(3));
						}
						
						if (WorldGen.genRand.NextBool(3) && !WorldgenTools.TooMuchLiquid(x, y))
						{
							WorldGen.PlaceObject(x, y - 1, ModContent.TileType<Cooksonia>(), true, WorldGen.genRand.Next(6));
						}
                    }

					//Ancient stone ambient
					if(tile.TileType == (ushort)ModContent.TileType<AncientStone>() && !WorldgenTools.IsSloped(x, y))
                    {
                        //Ambient decorations
						if (WorldGen.genRand.NextBool(5))
                        {
                            switch (WorldGen.genRand.Next(2))
                            {
								default: 
									WorldGen.PlaceObject(x, y - 1, ModContent.TileType<LargeAncientStoneRock>(), true, WorldGen.genRand.Next(3));
									break;
                                case 1:
									WorldGen.PlaceObject(x, y - 1, ModContent.TileType<MediumAncientStoneRock>(), true, WorldGen.genRand.Next(3));
									break;
                            }
                        }

						if (WorldGen.genRand.NextBool(3))
                        {
                            WorldGen.PlaceObject(x, y - 1, (ushort)ModContent.TileType<SmallAncientStoneRock>(), true, WorldGen.genRand.Next(3));
                        }
                    }
                }
			}
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			//Add the biome in the worldgen task
			int BiomesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
			if (BiomesIndex != -1)
			{
				tasks.Insert(BiomesIndex + 1, new PassLegacy("Ancient Swamp", SwampVegetation));
			}
		}
    }
}