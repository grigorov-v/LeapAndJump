using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffAutoPlay : MonoBehaviour
{
    Button _button;

    bool _curAutoPlay = false;

    private void Awake() {
        _button = GetComponent<Button>();
       
        _button.onClick.AddListener(() => {
            _curAutoPlay = !_curAutoPlay;
            FindObjectOfType<PlayerControl>().AutoPlay = _curAutoPlay;
        });
    }
}
