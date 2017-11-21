using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BestTimeScript : MonoBehaviour {

    Text text;
	// Use this for initialization
	void Start () {
        text = transform.gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = GameObject.Find( "MainBlock" ).GetComponent<MainScript>().bestTime.ToString() + "s";
	}
}
