using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartScreenScript : MonoBehaviour {
	public GameObject block;

	List<GameObject> ring1 = new List<GameObject>();
	List<GameObject> ring2 = new List<GameObject>();
	List<GameObject> ring3 = new List<GameObject>();

	// Use this for initialization
	void Start () {
		StartCoroutine ( StartAnim () );
	}

	Vector3 numDir( int num ) {
		return ( num == 1 ? new Vector3( 1, 0, 0 ) : ( num == 2 ? new Vector3( 0, 0, -1 ) : ( num == 3 ? new Vector3( -1, 0, 0 ) : new Vector3( 0, 0, 1 ) ) ) );
	}
	bool blockDir( GameObject obj, int dir ) {
		Vector3 pos = obj.transform.position + numDir ( dir );
		return Physics.CheckSphere ( pos , 0.1f );
		//RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.1f, Vector3.forward, 0.1f);
		//return hits == null;
	}
	void generateNext( GameObject o ) {
		for( int i = 1; i < 5; i++ ) {
			if( !blockDir ( o, i )) {
				GameObject obj = (GameObject) Instantiate( block, o.transform.position + numDir ( i ), Quaternion.identity );
				ring1.Add ( obj );
			}
		}
	}


	IEnumerator StartAnim() {
		GameObject obj = (GameObject) Instantiate ( block, Vector3.zero, Quaternion.identity );
		ring1.Add ( obj );
		int i = 0;

		while( i < 300 ) {
			i++;


			List<GameObject> temp1 = new List<GameObject>(ring1);
			ring1.Clear ();

			List<GameObject> temp2 = new List<GameObject>(ring2);
			ring2.Clear ();
			ring2 = temp1;

			if( ring3.Count > 0 ) {
				foreach( GameObject o in ring3 ) {
					Destroy( o );
				}
			}
			ring3.Clear ();
			ring3 = temp2;

			foreach( GameObject o in temp1 ) {
				Vector3 pos = o.transform.position;
				if( pos.x >= 20 || pos.x <= -20 || pos.z >= 20 || pos.z <= -20 ) {
					Destroy ( o );
				} else {
					generateNext( o );
				}
			}

			yield return new WaitForSeconds(0.01f);
		}
	}
   
}
