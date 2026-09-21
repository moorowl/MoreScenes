using HarmonyLib;
using PugMod;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace MoreScenes {
	[HarmonyPatch]
	public class Main : IMod {
		public const string Version = "1.4";
		public const string InternalName = "MoreScenes";
		public const string DisplayName = "Scenes+";
		
		public void EarlyInit() {
			Debug.Log($"[{DisplayName}]: Mod version: {Version}");
		}

		public void Init() { }

		public void Shutdown() { }

		public void ModObjectLoaded(Object obj) { }

		public void Update() { }

		[HarmonyPatch(typeof(ECSManager), "Init")]
		[HarmonyPrefix]
		private static void ECSManager_Init(ECSManager __instance) {
			InjectScenesInDungeonDataBlock.InjectAllScenes(Manager.ecs.serverAuthoringPrefab);
		}
	}
}