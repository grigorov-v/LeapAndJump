using UnityEngine;
using UnityEngine.SceneManagement;

using Grigorov.LeapAndJump.UI;

namespace Grigorov.SceneManagement {
    public static class LoadSceneHelper {
        static SceneHandler _targetSceneHandler = new SceneHandler();

        public static void StartLoadingScene(string sceneName) {
            if ( !LoadingUI.Instance ) {
                LoadingUI.CreateNewObject();
            }

            LoadingUI.Instance.Show(0);
            _targetSceneHandler.LoadSceneAsync(sceneName)
                .SetLoadedAction(scene => LoadingUI.Instance.Hide(1))
                .SetLoadingAction(progress => LoadingUI.Instance.UpdateBar(progress));
        }
    }
}