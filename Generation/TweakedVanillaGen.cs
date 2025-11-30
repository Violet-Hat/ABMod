using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using ReLogic.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ABMod.Generation
{
    public class TweakenVanillaGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            #region "Shimmer"

            //Re-locate the Aether to avoid the Ancient Swamps
            int shimmerIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shimmer"));
            tasks[shimmerIndex] = new PassLegacy("Shimmer", (progress, config) =>
            {
                int MinY = (int)(Main.rockLayer + 200);
                int MaxY = (int)(((Main.maxTilesY - 250) * 2) + Main.rockLayer) / 3;

                if (MaxY > Main.maxTilesY - 200)
                {
                    MaxY = Main.maxTilesY - 200;
                }
                if (MaxY <= MinY)
                {
                    MaxY = MinY + 50;
                }

                int AetherX = GenVars.dungeonSide < 0 ? Main.maxTilesX - 100 : 100;
                int AetherY = WorldGen.genRand.Next(MinY, MaxY);
                
                //anniversary lobotomy
				int AnniversaryMinY = (int)Main.worldSurface + 200;
				int AnniversaryMaxY = (int)(Main.rockLayer + Main.worldSurface + 200) / 2;

                if (AnniversaryMaxY <= AnniversaryMinY)
                {
                    AnniversaryMaxY = AnniversaryMinY + 50;
                }

                if (WorldGen.tenthAnniversaryWorldGen)
                {
                    AetherY = WorldGen.genRand.Next(AnniversaryMinY, AnniversaryMaxY);
                }
                
                //Fail-safe
                while (!WorldGen.ShimmerMakeBiome(AetherX, AetherY))
                {
                    AetherX = (GenVars.dungeonSide < 0) ? (int)(Main.maxTilesX * 0.95f) : (int)(Main.maxTilesX * 0.05f);
                    AetherY = WorldGen.genRand.Next(MinY, MaxY);
                }

                GenVars.shimmerPosition = new Vector2D(AetherX, AetherY);

                //This protects the shimmer from other structures
                int ProtectionSize = 200;
                GenVars.structures.AddProtectedStructure(new Rectangle(AetherX - ProtectionSize / 2, AetherY - ProtectionSize / 2, ProtectionSize, ProtectionSize));
            });

            #endregion
        }
    }
}