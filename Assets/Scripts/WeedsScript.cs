using UnityEngine;
using System.Collections;

public class WeedsScript : MonoBehaviour {
    public Material wiltedMaterial;
    float[] diff;

	// Use this for initialization
	void Start () {
        diff = GameObject.Find( "MainBlock" ).GetComponent<MainScript>().diffSettings[ PlayerPrefs.GetInt( "Difficulty" ) ];


        Invoke( "Wilt", diff[4] );
        transform.gameObject.tag = "Weed";
	}
	
    void Wilt() {
        transform.GetComponent<Renderer>().material = wiltedMaterial;
        transform.gameObject.tag = "Wilted";
        Invoke( "Destruct", diff[4] );
    }

    void Destruct() {
        Destroy( transform.gameObject );
    }
}
