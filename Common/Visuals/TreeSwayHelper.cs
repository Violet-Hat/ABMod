using System;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader;

namespace ABMod.Common.Visuals
{
    //Only used for tree wind shenanigans for now
    public class TreeSwayHelper : ModSystem
    {
        public static event Action OnTileDrawingUpdate;
        public static double TreeWindCounter { get; private set; }

        public override void Load()
        {
            On_TileDrawing.Update += Update;
        }

        //Check the TileDrawing class, Update()
        private static void Update(On_TileDrawing.orig_Update orig, TileDrawing self)
        {
            orig(self);

            if (!Main.dedServ)
            {
                double num = Math.Abs(Main.WindForVisuals);
                Utils.GetLerpValue(0.08f, 1.2f, (float)num, clamped: true);
                TreeWindCounter += 1.0 / 240.0 + 1.0 / 240.0 * num * 2.0;

                OnTileDrawingUpdate?.Invoke();
            }
        }
    }
}