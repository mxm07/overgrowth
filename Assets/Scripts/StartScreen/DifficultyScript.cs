using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DifficultyScript : MonoBehaviour {
    Dropdown d;

    void Start() {
        d = GetComponent<Dropdown>();
        d.value = PlayerPrefs.GetInt( "Difficulty" );
    }
    public void updateDifficulty( int diff ) {
        PlayerPrefs.SetInt( "Difficulty", diff );
    }

}
