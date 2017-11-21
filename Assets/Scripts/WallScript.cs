using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
    public Material rotten;

	// Use this for initialization
	void Start () {
        float[] diff = GameObject.Find( "MainBlock" ).GetComponent<MainScript>().diffSettings[ PlayerPrefs.GetInt( "Difficulty" ) ];

        float rand = Random.Range( diff[2], diff[3] );

        Invoke( "Rot", rand - 3f );
        Invoke( "Destruct", rand  );
	}

    void Rot() {
        GetComponent<Renderer>().material = rotten;
    }
    void Destruct() {
        Destroy( transform.gameObject );
    }
}
