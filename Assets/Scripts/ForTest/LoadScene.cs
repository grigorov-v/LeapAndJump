using UnityEngine;
using UnityEngine.UI;

using Grigorov.SceneManagement;

public class LoadScene : MonoBehaviour {
    public void Load(string name) {
        SceneHelper.LoadSceneAsync(name)
            .SetLoadingAction((progress) => Debug.Log(progress))
            .SetLoadedAction((scene, mode) => Debug.Log("Loaded " + scene.name))
            .SetUnloadedAction((scene) => Debug.Log("Unloaded " + scene.name));
    }
}