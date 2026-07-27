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
    public abstract class LabBuilder(bool hasLeftRoom, bool hasRightRoom, bool hasBottomRoom)
    {
        public bool HasLeftRoom { get; set; } = hasLeftRoom;
        public bool HasRightRoom { get; set; } = hasRightRoom;
        public bool HasBottomRoom { get; set; } = hasBottomRoom;

        public abstract bool Place(Point origin, int width, int height);

        public static bool IsValidRectangleSpot(Point origin, int width, int height)
        {
            for (int x = origin.X - width; x <= origin.X + width; x++)
            {
                for (int y = origin.Y - height; y <= origin.Y; y++)
                {
                    if (IsBiomeTile.IsTempleTile(x, y))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool IsValidDomeSpot(Point origin, int radius)
        {
            for (int x = origin.X - radius; x <= origin.X + radius; x++)
            {
                for (int y = origin.Y - radius; y <= origin.Y; y++)
                {
                    if (IsBiomeTile.IsTempleTile(x, y))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void PlaceRectangle(Point origin, int tileType, int wallType, int width, int height)
        {
            //Inner outline
            ShapeData rectangle = new();
            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(
            [
                new Actions.Blank().Output(rectangle)
            ]));

            //Clear tiles
            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(
            [
                new Actions.ClearTile(), new Actions.SetLiquid(0, 0)
            ]));

            //Place tiles
            WorldUtils.Gen(origin, new ModShapes.InnerOutline(rectangle, true), Actions.Chain(
            [
                new Actions.PlaceTile((ushort)tileType)
            ]));

            //Walls
            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(
            [
                new Actions.ClearWall(), new Actions.PlaceWall((ushort)wallType)
            ]));

            //Clear walls on edges
            WorldUtils.Gen(origin, new ModShapes.InnerOutline(rectangle, true), Actions.Chain(
            [
                new Actions.ClearWall()
            ]));
        }

        public static void PlaceDome(Point origin, int tileType, int wallType, int radius)
        {
            //Inner outline
            ShapeData halfCircle = new();
            WorldUtils.Gen(origin, new Shapes.HalfCircle(radius), Actions.Chain(
            [
                new Actions.Blank().Output(halfCircle)
            ]));

            //Clear tiles
            WorldUtils.Gen(origin, new Shapes.HalfCircle(radius), Actions.Chain(
            [
                new Actions.ClearTile(), new Actions.SetLiquid(0, 0)
            ]));

            //Place tiles
            WorldUtils.Gen(origin, new ModShapes.InnerOutline(halfCircle, true), Actions.Chain(
            [
                new Actions.PlaceTile((ushort)tileType)
            ]));

            //Walls
            WorldUtils.Gen(origin, new Shapes.HalfCircle(radius), Actions.Chain(
            [
                new Actions.ClearWall(), new Actions.PlaceWall((ushort)wallType)
            ]));

            //Clear walls on edges
            WorldUtils.Gen(origin, new ModShapes.InnerOutline(halfCircle, true), Actions.Chain(
            [
                new Actions.ClearWall()
            ]));
        }

        public static void PlaceLeftStaircase(Point origin, int tileType, int wallType, int width, int height, bool direction)
        {
            //True is up, false is down
            int directionMultiplier = direction ? -1 : 1;

            //Inner outline
            ShapeData rectangle = new();
            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(
            [
                new Actions.Blank().Output(rectangle)
            ]));
            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(
            [
                new Modifiers.Offset(-width, 5 * directionMultiplier), new Actions.Blank().Output(rectangle)
            ]));

            for (int i = 0; i <= 5; i++)
            {
                int offsetX = -i;
                int offsetY = i * directionMultiplier;

                WorldUtils.Gen(origin, new Shapes.Rectangle(height, height), Actions.Chain(
                [
                    new Modifiers.Offset(offsetX, offsetY), new Actions.Blank().Output(rectangle)
                ]));
            }

            //Clear tiles
            WorldUtils.Gen(origin, new ModShapes.All(rectangle), Actions.Chain(
            [
                new Actions.ClearTile(), new Actions.SetLiquid(0, 0)
            ]));

            //Place tiles
            WorldUtils.Gen(origin, new ModShapes.InnerOutline(rectangle, true), Actions.Chain(
            [
                new Actions.PlaceTile((ushort)tileType)
            ]));

            //Walls
            WorldUtils.Gen(origin, new ModShapes.All(rectangle), Actions.Chain(
            [
                new Actions.ClearWall(), new Actions.PlaceWall((ushort)wallType)
            ]));

            //Clear walls on edges
            WorldUtils.Gen(origin, new ModShapes.InnerOutline(rectangle, true), Actions.Chain(
            [
                new Actions.ClearWall()
            ]));
        }

        public static void PlaceRightStaircase(Point origin, int tileType, int wallType, int width, int height, bool direction)
        {
            //True is up, false is down
            int directionMultiplier = direction ? -1 : 1;

            //Inner outline
            ShapeData rectangle = new();
            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(
            [
                new Modifiers.Offset(-width, 0), new Actions.Blank().Output(rectangle)
            ]));
            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(
            [
                new Modifiers.Offset(0, 5 * directionMultiplier), new Actions.Blank().Output(rectangle)
            ]));

            int stairOffsetX = -height / 2;

            for (int i = 0; i <= 5; i++)
            {
                int offsetX = stairOffsetX + i;
                int offsetY = i * directionMultiplier;

                WorldUtils.Gen(origin, new Shapes.Rectangle(height, height), Actions.Chain(
                [
                    new Modifiers.Offset(offsetX, offsetY), new Actions.Blank().Output(rectangle)
                ]));
            }

            //Clear tiles
            WorldUtils.Gen(origin, new ModShapes.All(rectangle), Actions.Chain(
            [
                new Actions.ClearTile(), new Actions.SetLiquid(0, 0)
            ]));

            //Place tiles
            WorldUtils.Gen(origin, new ModShapes.InnerOutline(rectangle, true), Actions.Chain(
            [
                new Actions.PlaceTile((ushort)tileType)
            ]));

            //Walls
            WorldUtils.Gen(origin, new ModShapes.All(rectangle), Actions.Chain(
            [
                new Actions.ClearWall(), new Actions.PlaceWall((ushort)wallType)
            ]));

            //Clear walls on edges
            WorldUtils.Gen(origin, new ModShapes.InnerOutline(rectangle, true), Actions.Chain(
            [
                new Actions.ClearWall()
            ]));
        }
    }

    public class RectangleLabBuilder(bool hasLeftRoom, bool hasRightRoom, bool hasTopRoom, bool hasBottomRoom) : LabBuilder(hasLeftRoom, hasRightRoom, hasBottomRoom)
    {
        bool HasTopRoom { get; set; } = hasTopRoom;

        public override bool Place(Point origin, int width, int height)
        {
            Point trueOrigin = new(origin.X - width, origin.Y - height);
            int trueWidth = (width * 2) + 1;
            int trueHeight = height + 1;

            //Main room
            bool check = WorldGen.genRand.NextBool(5);
            if (!HasLeftRoom && !HasRightRoom && !HasTopRoom && !HasBottomRoom && check)
            {
                trueWidth = (int)(trueWidth * 0.6f);

                //Check specific for stair style
                trueOrigin = new(origin.X, origin.Y - height);
                Point originCheck = new(origin.X, origin.Y + height);

                if (!IsValidRectangleSpot(originCheck, width * 2, height * 2))
                {
                    return false;
                }

                //Place
                if (WorldGen.genRand.NextBool())
                {
                    PlaceRightStaircase(trueOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, trueWidth, trueHeight, WorldGen.genRand.NextBool());
                }
                else
                {
                    PlaceLeftStaircase(trueOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, trueWidth, trueHeight, WorldGen.genRand.NextBool());
                }
            }
            else
            {
                //Check for regular style
                if (!IsValidRectangleSpot(origin, width, height))
                {
                    return false;
                }

                PlaceRectangle(trueOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, trueWidth, trueHeight);
            }

            //Extra rooms
            int padding = width + height + 5;

            if (HasLeftRoom)
            {
                Point leftOrigin = new(origin.X - padding, origin.Y);

                if (!IsValidDomeSpot(leftOrigin, height))
                {
                    return false;
                }

                //Place
                PlaceDome(leftOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, height);
            }
            if (HasRightRoom)
            {
                Point rightOrigin = new(origin.X + padding, origin.Y);

                if (!IsValidDomeSpot(rightOrigin, height))
                {
                    return false;
                }

                //Place
                PlaceDome(rightOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, height);
            }

            trueWidth = (int)(trueWidth * 0.6f);
            padding = trueHeight + 5;

            if (HasTopRoom)
            {
                Point topOrigin = new(origin.X, trueOrigin.Y - padding);

                //Check specific for stair style
                Point originCheck = new(origin.X, topOrigin.Y + height);

                if (!IsValidRectangleSpot(originCheck, width * 2, height * 2))
                {
                    return false;
                }

                //Place
                if (WorldGen.genRand.NextBool())
                {
                    PlaceLeftStaircase(topOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, trueWidth, trueHeight, true);
                }
                else
                {
                    PlaceLeftStaircase(topOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, trueWidth, trueHeight, true);
                }
            }
            if (HasBottomRoom)
            {
                Point bottomOrigin = new(origin.X, trueOrigin.Y + padding);

                //Check specific for stair style
                Point originCheck = new(origin.X, bottomOrigin.Y + height);

                if (!IsValidRectangleSpot(originCheck, width * 2, height * 2))
                {
                    return false;
                }

                //Place
                if (WorldGen.genRand.NextBool())
                {
                    PlaceLeftStaircase(bottomOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, trueWidth, trueHeight, false);
                }
                else
                {
                    PlaceLeftStaircase(bottomOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, trueWidth, trueHeight, false);
                }
            }

            return true;
        }
    }

    public class DomeLabBuilder(bool hasLeftRoom, bool hasRightRoom, bool hasBottomRoom) : LabBuilder(hasLeftRoom, hasRightRoom, hasBottomRoom)
    {
        public override bool Place(Point origin, int width, int height)
        {
            if (!IsValidDomeSpot(origin, height))
            {
                return false;
            }
            
            //Place
            PlaceDome(origin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, height);

            return true;
        }
    }
}