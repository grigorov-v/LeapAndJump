using UnityEngine;
using UnityEngine.UI;

using Grigorov.SceneManagement;

public class LoadScene : MonoBehaviour {
    public void Load(string name) {
        LoadSceneHelper.StartLoadingScene(name);
    }
}