using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.IO;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;

using ABMod.Common.Tiles;

namespace ABMod.Content.Generation.Objects
{
    public class LabBuilder(bool hasTopRoom, bool hasLeftRoom, bool hasRightRoom, bool hasBottomRoom)
    {
        bool HasTopRoom { get; set; } = hasTopRoom;
        bool HasLeftRoom { get; set; } = hasLeftRoom;
        bool HasRightRoom { get; set; } = hasRightRoom;
        bool HasBottomRoom { get; set; } = hasBottomRoom;

        static readonly string path = "Content/Generation/Structures/Swamp/";
        static readonly string fileType = ".shstruct";

        public bool Place(Point origin)
        {
            string middle = "LabStruct_1_Middle";
            string leftSide;
            string rightSide;

            //Middle segment variety
            int middlePadding = WorldGen.genRand.Next(6);
            for(int i = -middlePadding; i <= middlePadding; i++)
            {
                Vector2 middleOrigin = new(origin.X + i, origin.Y - 14);
                StructureHelper.API.Generator.GenerateStructure(path + middle + fileType, middleOrigin.ToPoint16(), ABMod.Instance);
            }

            //Main room, 33% of it being a two sided stair style room if alone
            bool check = WorldGen.genRand.NextBool(3);
            if (!HasLeftRoom && !HasRightRoom && !HasTopRoom && !HasBottomRoom && check)
            {
                if (WorldGen.genRand.NextBool())
                {
                    leftSide = "LabStruct_16_UpStairLeft";
                    rightSide = "LabStruct_15_DownStairRight";
                }
                else
                {
                    leftSide = "LabStruct_14_DownStairLeft";
                    rightSide = "LabStruct_17_UpStairRight";
                }

                Vector2 stairLeftOrigin = new(origin.X - 28 - middlePadding, origin.Y - 14);
                Vector2 stairRightOrigin = new(origin.X + 1 + middlePadding, origin.Y - 14);

                StructureHelper.API.Generator.GenerateStructure(path + leftSide + fileType, stairLeftOrigin.ToPoint16(), ABMod.Instance);
                StructureHelper.API.Generator.GenerateStructure(path + rightSide + fileType, stairRightOrigin.ToPoint16(), ABMod.Instance);
            }
            else
            {
                if (!HasLeftRoom)
                {
                    leftSide = "LabStruct_2_Left";
                    Vector2 leftOrigin = new(origin.X - 14 - middlePadding, origin.Y - 14);
                    StructureHelper.API.Generator.GenerateStructure(path + leftSide + fileType, leftOrigin.ToPoint16(), ABMod.Instance);
                }
                if (!HasRightRoom)
                {
                    rightSide = "LabStruct_3_Right";
                    Vector2 rightOrigin = new(origin.X + 1 + middlePadding, origin.Y - 14);
                    StructureHelper.API.Generator.GenerateStructure(path + rightSide + fileType, rightOrigin.ToPoint16(), ABMod.Instance);
                }
            }

            //Extra rooms
            if (HasLeftRoom)
            {
                leftSide = "LabStruct_4_LeftDome";
                Vector2 leftOrigin = new(origin.X - 32 - middlePadding, origin.Y - 14);
                StructureHelper.API.Generator.GenerateStructure(path + leftSide + fileType, leftOrigin.ToPoint16(), ABMod.Instance);
            }
            if (HasRightRoom)
            {
                rightSide = "LabStruct_5_RightDome";
                Vector2 rightOrigin = new(origin.X + 1 + middlePadding, origin.Y - 14);
                StructureHelper.API.Generator.GenerateStructure(path + rightSide + fileType, rightOrigin.ToPoint16(), ABMod.Instance);
            }
            if (HasTopRoom)
            {
                int topMiddlePadding = WorldGen.genRand.Next(6);

                string topLeftSide;
                string topRightSide;

                Vector2 topMiddleOrigin;
                Vector2 topLeftOrigin;
                Vector2 topRightOrigin;

                //33% of the top room being a special room
                if (WorldGen.genRand.NextBool(3))
                {
                    //Left or right
                    if (WorldGen.genRand.NextBool())
                    {
                        topLeftSide = "LabStruct_16_UpStairLeft";
                        topRightSide = "LabStruct_11_UpRightLarge";

                        topLeftOrigin = new(origin.X - 28 - topMiddlePadding, origin.Y - 26);
                        topRightOrigin = new(origin.X + 1 + topMiddlePadding, origin.Y - 26);
                    }
                    else
                    {
                        topLeftSide = "LabStruct_10_UpLeftLarge";
                        topRightSide = "LabStruct_17_UpStairRight";

                        topLeftOrigin = new(origin.X - 19 - topMiddlePadding, origin.Y - 26);
                        topRightOrigin = new(origin.X + 1 + topMiddlePadding, origin.Y - 26);
                    }
                }
                else
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        topLeftSide = "LabStruct_6_UpLeftSmall";
                        topRightSide = "LabStruct_3_Right";

                        topLeftOrigin = new(origin.X - 14 - topMiddlePadding, origin.Y - 26);
                        topRightOrigin = new(origin.X + 1 + topMiddlePadding, origin.Y - 26);
                    }
                    else
                    {
                        topLeftSide = "LabStruct_2_Left";
                        topRightSide = "LabStruct_7_UpRightSmall";

                        topLeftOrigin = new(origin.X - 14 - topMiddlePadding, origin.Y - 26);
                        topRightOrigin = new(origin.X + 1 + topMiddlePadding, origin.Y - 26);
                    }
                }

                for(int i = -topMiddlePadding; i <= topMiddlePadding; i++)
                {
                    topMiddleOrigin = new(origin.X + i, origin.Y - 26);
                    StructureHelper.API.Generator.GenerateStructure(path + middle + fileType, topMiddleOrigin.ToPoint16(), ABMod.Instance);
                }

                StructureHelper.API.Generator.GenerateStructure(path + topLeftSide + fileType, topLeftOrigin.ToPoint16(), ABMod.Instance);
                StructureHelper.API.Generator.GenerateStructure(path + topRightSide + fileType, topRightOrigin.ToPoint16(), ABMod.Instance);
            }
            if (HasBottomRoom)
            {
                int bottomMiddlePadding = WorldGen.genRand.Next(6);

                string bottomLeftSide;
                string bottomRightSide;

                Vector2 bottomMiddleOrigin;
                Vector2 bottomLeftOrigin;
                Vector2 bottomRightOrigin;

                //33% of the top room being a special room
                if (WorldGen.genRand.NextBool(3))
                {
                    //Left or right
                    if (WorldGen.genRand.NextBool())
                    {
                        bottomLeftSide = "LabStruct_14_DownStairLeft";
                        bottomRightSide = "LabStruct_13_DownRightLarge";

                        bottomLeftOrigin = new(origin.X - 28 - bottomMiddlePadding, origin.Y - 2);
                        bottomRightOrigin = new(origin.X + 1 + bottomMiddlePadding, origin.Y - 2);
                    }
                    else
                    {
                        bottomLeftSide = "LabStruct_12_DownLeftLarge";
                        bottomRightSide = "LabStruct_15_DownStairRight";

                        bottomLeftOrigin = new(origin.X - 19 - bottomMiddlePadding, origin.Y - 2);
                        bottomRightOrigin = new(origin.X + 1 + bottomMiddlePadding, origin.Y - 2);
                    }
                }
                else
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        bottomLeftSide = "LabStruct_8_DownLeftSmall";
                        bottomRightSide = "LabStruct_3_Right";

                        bottomLeftOrigin = new(origin.X - 14 - bottomMiddlePadding, origin.Y - 2);
                        bottomRightOrigin = new(origin.X + 1 + bottomMiddlePadding, origin.Y - 2);
                    }
                    else
                    {
                        bottomLeftSide = "LabStruct_2_Left";
                        bottomRightSide = "LabStruct_9_DownRightSmall";

                        bottomLeftOrigin = new(origin.X - 14 - bottomMiddlePadding, origin.Y - 2);
                        bottomRightOrigin = new(origin.X + 1 + bottomMiddlePadding, origin.Y - 2);
                    }
                }

                for(int i = -bottomMiddlePadding; i <= bottomMiddlePadding; i++)
                {
                    bottomMiddleOrigin = new(origin.X + i, origin.Y - 2);
                    StructureHelper.API.Generator.GenerateStructure(path + middle + fileType, bottomMiddleOrigin.ToPoint16(), ABMod.Instance);
                }

                StructureHelper.API.Generator.GenerateStructure(path + bottomLeftSide + fileType, bottomLeftOrigin.ToPoint16(), ABMod.Instance);
                StructureHelper.API.Generator.GenerateStructure(path + bottomRightSide + fileType, bottomRightOrigin.ToPoint16(), ABMod.Instance);
            }

            return true;
        }
    }
}