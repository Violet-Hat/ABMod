using System;
using Terraria.ModLoader;

namespace ABMod.Common.Hooks
{
    //Currently only used to make the tree wind sway functional
    public class GeneralSystemHooks : ModSystem
    {
        //The event(s) that will be invoked after the network got updated
        public static event Action PostUpdateEverythingEvent;

        public override void PostUpdateEverything()
        {
            PostUpdateEverythingEvent?.Invoke();
        }
    }
}