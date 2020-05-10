using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Grigorov.SceneManagement;

public class TestLoadingBar : MonoBehaviour
{
    [SerializeField] Image _fill = null;

    void Start()
    {
        SceneHelper.LoadSceneAsync("MainMenu")
            .SetLoadingAction((progress) => _fill.fillAmount = progress);
    }
}
