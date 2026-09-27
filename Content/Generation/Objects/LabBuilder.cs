using Microsoft.Xna.Framework;
using Terraria;

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
            string middle = "LabStruct_A";
            string leftSide;
            string rightSide;

            //Middle segment variety
            int middlePadding = WorldGen.genRand.Next(6);
            for(int i = -middlePadding; i <= middlePadding; i++)
            {
                Vector2 middleOrigin = new(origin.X + i, origin.Y - 14);
                StructureHelper.API.Generator.GenerateStructure(path + middle + fileType, middleOrigin.ToPoint16(), ABMod.Instance);
            }

            //Main room
            if (!HasLeftRoom)
            {
                leftSide = "LabStruct_B";
                Vector2 leftOrigin = new(origin.X - 19 - middlePadding, origin.Y - 14);
                StructureHelper.API.Generator.GenerateStructure(path + leftSide + fileType, leftOrigin.ToPoint16(), ABMod.Instance);
            }
            if (!HasRightRoom)
            {
                rightSide = "LabStruct_C";
                Vector2 rightOrigin = new(origin.X + 1 + middlePadding, origin.Y - 14);
                StructureHelper.API.Generator.GenerateStructure(path + rightSide + fileType, rightOrigin.ToPoint16(), ABMod.Instance);
            }

            //Extra rooms
            if (HasLeftRoom)
            {
                leftSide = "LabStruct_D";
                Vector2 leftOrigin = new(origin.X - 37 - middlePadding, origin.Y - 14);
                StructureHelper.API.Generator.GenerateStructure(path + leftSide + fileType, leftOrigin.ToPoint16(), ABMod.Instance);
            }
            if (HasRightRoom)
            {
                rightSide = "LabStruct_E";
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
                        topLeftSide = "LabStruct_P";
                        topRightSide = "LabStruct_K";

                        topLeftOrigin = new(origin.X - 28 - topMiddlePadding, origin.Y - 26);
                        topRightOrigin = new(origin.X + 1 + topMiddlePadding, origin.Y - 26);
                    }
                    else
                    {
                        topLeftSide = "LabStruct_J";
                        topRightSide = "LabStruct_Q";

                        topLeftOrigin = new(origin.X - 19 - topMiddlePadding, origin.Y - 26);
                        topRightOrigin = new(origin.X + 1 + topMiddlePadding, origin.Y - 26);
                    }
                }
                else
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        topLeftSide = "LabStruct_F";
                        topRightSide = "LabStruct_S";

                        topLeftOrigin = new(origin.X - 14 - topMiddlePadding, origin.Y - 26);
                        topRightOrigin = new(origin.X + 1 + topMiddlePadding, origin.Y - 26);
                    }
                    else
                    {
                        topLeftSide = "LabStruct_R";
                        topRightSide = "LabStruct_G";

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
                        bottomLeftSide = "LabStruct_N";
                        bottomRightSide = "LabStruct_M";

                        bottomLeftOrigin = new(origin.X - 28 - bottomMiddlePadding, origin.Y - 2);
                        bottomRightOrigin = new(origin.X + 1 + bottomMiddlePadding, origin.Y - 2);
                    }
                    else
                    {
                        bottomLeftSide = "LabStruct_L";
                        bottomRightSide = "LabStruct_O";

                        bottomLeftOrigin = new(origin.X - 19 - bottomMiddlePadding, origin.Y - 2);
                        bottomRightOrigin = new(origin.X + 1 + bottomMiddlePadding, origin.Y - 2);
                    }
                }
                else
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        bottomLeftSide = "LabStruct_H";
                        bottomRightSide = "LabStruct_S";

                        bottomLeftOrigin = new(origin.X - 14 - bottomMiddlePadding, origin.Y - 2);
                        bottomRightOrigin = new(origin.X + 1 + bottomMiddlePadding, origin.Y - 2);
                    }
                    else
                    {
                        bottomLeftSide = "LabStruct_R";
                        bottomRightSide = "LabStruct_I";

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