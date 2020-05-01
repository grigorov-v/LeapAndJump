using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class VersionLabel : MonoBehaviour {
    void Awake() {
        GetComponent<Text>().text = "ver." + Version.GetStringVersion();
    }
}
