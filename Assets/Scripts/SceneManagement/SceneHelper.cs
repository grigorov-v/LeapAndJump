using System.Collections.Generic;

namespace Grigorov.SceneManagement {
    public static class SceneHelper {
        static Dictionary<string, SceneHandler> _sceneHandlers = new Dictionary<string, SceneHandler>();

        public static SceneHandler LoadSceneAsync(string sceneName) {
            return GetSceneHandler(sceneName).LoadSceneAsync();
        }

        public static SceneHandler LoadScene(string sceneName) {
            return GetSceneHandler(sceneName).LoadScene();
        }

        static SceneHandler GetSceneHandler(string sceneName) {
            if ( !_sceneHandlers.ContainsKey(sceneName) ) {
                _sceneHandlers[sceneName] = new SceneHandler(sceneName);
            }

            return _sceneHandlers[sceneName];
        }
    }
}