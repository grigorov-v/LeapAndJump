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
    }
}