using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotifyTextScript : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = transform.gameObject.GetComponent<Text>();
	}
	
    IEnumerator Fade() {
        float t = 0f;

        while( t < 1f ) {
            t += Time.deltaTime;
            text.color = Color.Lerp( new Color( 0, 1, 0, 1 ), new Color( 0, 1, 0, 0 ), t );
            yield return null;
        }
    }


    public void setColor( int c ) {
        if( c < 1 ) {
            text.color = Color.red;
        } else {
            text.color = Color.green;
        }
    }
    public void setAlpha( float a ) {
        Color col = text.color;
        col.a = a;
    }
    public void setValue( string s ) {
        text.text = s;
    }
    public void showText() {
        setAlpha( 0.5f );
        
        StartCoroutine( Fade() );
    }

	// Update is called once per frame
	void Update () {
	
	}
}
