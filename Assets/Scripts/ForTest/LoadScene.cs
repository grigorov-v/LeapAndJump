using UnityEngine;
using UnityEngine.UI;

using Grigorov.LoadingManagement;

public class LoadScene : MonoBehaviour {
    public void Load(string name) {
        LoadingHelper.StartLoadingScene(name);
    }
}