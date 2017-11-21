using UnityEngine;
using System.Collections;

public class PlayScript : MonoBehaviour {
    public void nextLevel() {
        Application.LoadLevel( "scene" );
    }

    public void quit() {
        Application.Quit();
    }
}
