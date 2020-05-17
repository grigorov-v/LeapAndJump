using UnityEngine;
using UnityEngine.SceneManagement;

using Grigorov.LeapAndJump.UI;

namespace Grigorov.LoadingManagement {
    public static class LoadingHelper {
        static SceneHandler _targetSceneHandler = new SceneHandler();

        public static void StartLoadingScene(string sceneName) {
            if ( !LoadingUI.Instance ) {
                LoadingUI.CreateNewObject();
            }

            LoadingUI.Instance.Show();
            _targetSceneHandler.LoadSceneAsync(sceneName)
                .SetLoadingAction(progress => LoadingUI.Instance.UpdateBar(progress))
                .SetLoadedAction(scene => LoadingUI.Instance.Hide());
        }

        public static void LoadMainMenu() {
            StartLoadingScene("MainMenu");
        }

        public static void RestartCurrentScene() {
            var sceneName = SceneManager.GetActiveScene().name;
            StartLoadingScene(sceneName);
        }

        public static void LoadLevel() {
            StartLoadingScene("World_1");
        }
    }
}