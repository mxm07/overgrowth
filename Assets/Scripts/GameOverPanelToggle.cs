using UnityEngine;
using System.Collections;

public class GameOverPanelToggle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.gameObject.SetActive( false );
    }

    public void OnClickRetry() {
        Time.timeScale = 1;
        Application.LoadLevel( "scene" );
    }
    public void OnClickMainMenu() {
        Time.timeScale = 1;
        Application.LoadLevel( "startscene" );
    }
}
