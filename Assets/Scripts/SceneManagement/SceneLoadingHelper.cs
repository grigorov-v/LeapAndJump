using UnityEngine.SceneManagement;
using Grigorov.SceneManagement.UI;

namespace Grigorov.SceneManagement {
    public static class SceneLoadingHelper {
        static SceneHandler _targetSceneHandler = new SceneHandler();

        public static void StartLoadingScene(string sceneName, LoadingUI loadingUI) {
            loadingUI.Show();
            _targetSceneHandler.LoadSceneAsync(sceneName)
                .SetLoadingAction(progress => loadingUI.UpdateBar(progress))
                .SetLoadedAction(scene => loadingUI.Hide());
        }
    }
}