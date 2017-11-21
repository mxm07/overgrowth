using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicScript : MonoBehaviour {
    Toggle m;

    void Start() {
        m = GetComponent<Toggle>();
        m.isOn = (PlayerPrefs.GetInt( "Music" ) == 1);
    }


    public void updateMusic(bool on) {
        PlayerPrefs.SetInt( "Music", on ? 1 : 0 );
    }
}
