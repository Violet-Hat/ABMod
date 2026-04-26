using System;
using Terraria;
using Terraria.ModLoader;

using ABMod.Common.Hooks;

namespace ABMod.Common.Visuals
{
    public class TreeWindSway : ILoadable
    {
        private static double treeWindCounter;

        //Update once and add it to the post update everything events
        public void Load(Mod mod)
        {
            Update();
            GeneralSystemHooks.PostUpdateEverythingEvent += Update;
        }

        //Check TileDrawing to know where this stuff comes from
        public static void Update()
        {
            //This game doesn't make things easier
            if (!Main.dedServ)
            {
                double baseWind = Math.Abs(Main.WindForVisuals);
                baseWind = Utils.GetLerpValue(0.08f, 1.2f, (float)baseWind, clamped: true);
                treeWindCounter = 1.0 / 240.0 + 1.0 / 240.0 * baseWind * 2.0;
            }
        }

        //Get
        public static double GetTreeWindCounter()
        {
            return treeWindCounter;
        }

        //Interface shenanigans
        public void Unload() { }
    }
}