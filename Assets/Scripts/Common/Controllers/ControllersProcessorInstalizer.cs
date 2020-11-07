using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grigorov.Controllers {
	public static class ControllersProcessorInstalizer {
		const string ControllersProcessorName = "[ControllersProcessor]";

		[RuntimeInitializeOnLoadMethod]
		static void OnRuntimeInitializeOnLoad() {
			TryCreateControllersProcessor();
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			TryCreateControllersProcessor();
		}

		static void TryCreateControllersProcessor() {
			if ( MonoBehaviour.FindObjectOfType<ControllersProcessor>() ) {
				return;
			}
			new GameObject(ControllersProcessorName).AddComponent<ControllersProcessor>();
		}
	}
}