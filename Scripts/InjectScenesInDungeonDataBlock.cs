using System.Collections.Generic;
using Pug.UnityExtensions;
using PugWorldGen;
using UnityEngine;

namespace MoreScenes {
	public class InjectScenesInDungeonDataBlock : ScriptableDataBlock {
		public string dungeonName;
		public int sceneGroupIndex;
		public DataBlockRef<CustomSceneDataBlock>[] scenesToInject;
		
		public static void InjectAllScenes(GameObject serverAuthoring) {
			foreach (var dataBlock in ScriptableData.GetDataBlocks<InjectScenesInDungeonDataBlock>())
				InjectScenes(serverAuthoring, dataBlock);
		}

		private static void InjectScenes(GameObject serverAuthoring, InjectScenesInDungeonDataBlock dataBlock) {
			var dungeonCustomSceneAuthorings = FindDungeonCustomSceneAuthoringsWithName(serverAuthoring, dataBlock.dungeonName);

			foreach (var dungeonCustomSceneAuthoring in dungeonCustomSceneAuthorings) {
				if (dungeonCustomSceneAuthoring.groups.IsValidIndex(dataBlock.sceneGroupIndex)) {
					foreach (var sceneRef in dataBlock.scenesToInject) {
						dungeonCustomSceneAuthoring.groups[dataBlock.sceneGroupIndex].scenes.Add(sceneRef);
						Debug.Log($"[{Main.DisplayName}]: Injected {sceneRef.address} into {dataBlock.dungeonName} (group {dataBlock.sceneGroupIndex})");
					}
				}
			}
		}
		
		private static IEnumerable<DungeonCustomScenesAuthoring> FindDungeonCustomSceneAuthoringsWithName(GameObject serverAuthoring, string internalName) {
			var dungeonSpawnsTable = serverAuthoring.GetComponentInChildren<DungeonSpawnTable>();
			var pugWorldGenAuthorings = serverAuthoring.GetComponentsInChildren<PugWorldGenAuthoring>();

			foreach (var group in dungeonSpawnsTable.biomes) {
				foreach (var entry in group.spawnEntries) {
					if (!entry.prefab.TryGetComponent<DungeonAuthoring>(out var dungeonAuthoring) || dungeonAuthoring.name != internalName)
						continue;

					if (!entry.prefab.TryGetComponent<DungeonCustomScenesAuthoring>(out var dungeonCustomScenesAuthoring))
						continue;

					yield return dungeonCustomScenesAuthoring;
				}
			}

			foreach (var pugWorldGenAuthoring in pugWorldGenAuthorings) {
				if (!pugWorldGenAuthoring.prefab.TryGetComponent<DungeonAuthoring>(out var dungeonAuthoring) || dungeonAuthoring.name != internalName)
					continue;

				if (!pugWorldGenAuthoring.prefab.TryGetComponent<DungeonCustomScenesAuthoring>(out var dungeonCustomScenesAuthoring))
					continue;

				yield return dungeonCustomScenesAuthoring;
			}
		}
	}
}