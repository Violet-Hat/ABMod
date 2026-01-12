using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ABMod
{
	public class ABMod : Mod
	{
		internal static ABMod Instance;
		
		//For NPC management in case of subworlds
		internal Mod subworldLibrary = null;
		
		internal static ABMod mod;
		
		public ABMod()
		{
			mod = this;
		}
		
		public override void Load()
		{
			Instance = this;
			
			ModLoader.TryGetMod("SubworldLibrary", out subworldLibrary);
		}
		
		public override void Unload()
		{
			subworldLibrary = null;
			
			mod = null;
		}
	}
}
