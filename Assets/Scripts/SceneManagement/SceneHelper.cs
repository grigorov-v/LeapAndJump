namespace Grigorov.SceneManagement {
    public static class SceneHelper {
        public static SceneHandler LoadSceneAsync(string sceneName) {
            return new SceneHandler(sceneName).LoadSceneAsync();
        }

        public static SceneHandler LoadScene(string sceneName) {
            return new SceneHandler(sceneName).LoadScene();
        }
    }
}