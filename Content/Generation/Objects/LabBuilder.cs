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
        bool HasLeftRoom { get; set; } = hasLeftRoom;
        bool HasRightRoom { get; set; } = hasRightRoom;
        bool HasBottomRoom { get; set; } = hasBottomRoom;

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

        public static void PlaceStaircase(Point origin, int tileType, int wallType, int width, int height)
        {
            //Inner outline
            ShapeData rectangle = new();
            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(
            [
                new Modifiers.Offset(-width, 0), new Actions.Blank().Output(rectangle)
            ]));
            WorldUtils.Gen(origin, new Shapes.Rectangle(width, height), Actions.Chain(
            [
                new Modifiers.Offset(0, 5), new Actions.Blank().Output(rectangle)
            ]));

            int stairOffsetX = -height / 2;

            for (int i = 0; i <= 5; i++)
            {
                WorldUtils.Gen(origin, new Shapes.Rectangle(height, height), Actions.Chain(
                [
                    new Modifiers.Offset(stairOffsetX + i, i), new Actions.Blank().Output(rectangle)
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
    }

    public class RectangleLabBuilder(bool hasLeftRoom, bool hasRightRoom, bool hasTopRoom, bool hasBottomRoom) : LabBuilder(hasLeftRoom, hasRightRoom, hasBottomRoom)
    {
        bool HasTopRoom { get; set; } = hasTopRoom;

        public override bool Place(Point origin, int width, int height)
        {
            if (!IsValidRectangleSpot(origin, width, height))
            {
                return false;
            }

            //Place
            Point tOrigin = new(origin.X - width, origin.Y - height);
            int tWidth = (width * 2) + 1;
            int tHeight = height + 1;

            //PlaceRectangle(tOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, tWidth, tHeight);
            PlaceStaircase(tOrigin, TileID.EmeraldGemspark, WallID.EmeraldGemspark, tWidth, tHeight);

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