using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainScript : MonoBehaviour {
	public GameObject block;
	public GameObject wall;
	public GameObject crate;

	public GameObject wallText;
	public GameObject timeText;

    public GameObject gameOverPanel;
    public GameObject pausedPanel;
    public GameObject notifyText;
    public GameObject growthText;

   
    public int wallNum;
	public int time = 0;

	List<GameObject> outer = new List<GameObject>();
	List<GameObject> bigouter = new List<GameObject>();
	List<GameObject> walls = new List<GameObject>();
	Vector3 last;

	bool stuckCheck = false;
	bool stuck = false;
	bool stop = false;
    bool paused = false;

    int amtNew = 0;
    float growthTime = 0;

    public int bestTime;
    public int bestWalls;

    int difficulty = 0;
    //Crate spawn time, block gen divide time, wall disappear lower bound, wall disappear upper, wilt time, starting wall amt, setup time, starting crate amt
    public float[][] diffSettings = {
        new float[] { 4f, 3f, 35f, 45f, 13f, 200f, 7f, 10f },
        new float[] { 7f, 4f, 25f, 35f, 9f, 150f, 5f, 5f },
        new float[] { 11f, 5f, 15f, 25f, 5f, 100f, 3f, 3f }
    };




    // Use this for initialization
    void Start () {
		outer.Add ( transform.gameObject );

        bestTime = PlayerPrefs.GetInt( "best_time" );
        bestWalls = PlayerPrefs.GetInt( "best_walls" );

        transform.gameObject.tag = "Weed";

        difficulty = PlayerPrefs.GetInt( "Difficulty" );
        wallNum = (int) diffSettings[ difficulty ][5];

        for(int i = 0; i < (int) diffSettings[difficulty][7]; i++) {
            genCrate();
        }

        if( PlayerPrefs.GetInt( "Music" ) == 1 )
            GetComponents<AudioSource>()[1].Play();



        growthTime = diffSettings[difficulty][6];
        Invoke( "step", diffSettings[difficulty][6] );
        Invoke( "wallcount", 0.5f );
        Invoke( "timecount", 1f );
        Invoke( "cratecount", diffSettings[difficulty][0] );
    }

	void Update() {
        if(!stop) {
            if(Input.GetButton( "Fire1" )) {
                if(!stuck && wallNum > 0) {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

                    if(Physics.Raycast( ray, out hit, 1000 )) {
                        if(last != new Vector3( Mathf.Round( hit.point.x ), 0, Mathf.Round( hit.point.z ) )) {
                            last = new Vector3( Mathf.Round( hit.point.x ), 0, Mathf.Round( hit.point.z ) );

                            if(last.x < 20 && last.x > -20 && last.z < 20 && last.z > -20 && hit.point.y < 0.5) {
                                GameObject obj = (GameObject) Instantiate ( wall, last, Quaternion.identity );
                                obj.tag = "Wall";
                                walls.Add( obj );

                                wallNum--;
                            }
                        }
                    }
                }
            } else if( Input.GetButton( "Fire2" ) ) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

                if( Physics.Raycast( ray, out hit, 1000 ) ) {
                    if( hit.transform.gameObject.tag == "Wall" )
                        Destroy( hit.transform.gameObject );
                }
            }
            
        }
        
        if( Input.GetKeyDown( KeyCode.P ) ) {
            paused = !paused;

            if( paused ) {
                GetComponents<AudioSource>()[1].Pause();
                pausedPanel.SetActive( true );
                Time.timeScale = 0;
            } else {
                GetComponents<AudioSource>()[1].UnPause();
                pausedPanel.SetActive( false );
                Time.timeScale = 1;
            }
        }
		wallText.GetComponent<Text>().text = wallNum.ToString ();
		timeText.GetComponent<Text>().text = time.ToString () + "s";
        growthText.GetComponent<Text>().text = growthTime.ToString( "F2" ) + "s";
    }




	void wallcount() {
		if( !stop && !stuck ) {
			wallNum += 5;

            notifyText.GetComponent<NotifyTextScript>().setColor( 1 );
            notifyText.GetComponent<NotifyTextScript>().setValue( "+5 Walls" );
            notifyText.GetComponent<NotifyTextScript>().showText();

            if( wallNum > bestWalls ) {
                bestWalls = wallNum;
                PlayerPrefs.SetInt( "best_walls", bestWalls );
            }
		}

        Invoke( "wallcount", 2f );
    }
	void timecount() {
		if( !stop && !stuck ) {
			time++;

            if( time > bestTime ) {
                bestTime = time;
                PlayerPrefs.SetInt( "best_time", bestTime );
            }
		}

        Invoke( "timecount", 1f );
    }
	void cratecount() {
		if( !stop ) {
			genCrate();
		}

        Invoke( "cratecount", diffSettings[difficulty][0] );
    }








    void genCrate() {
		Vector3 pos = new Vector3( (int) UnityEngine.Random.Range( -19.0f, 19.0f ), 0, (int) UnityEngine.Random.Range( -19.0f, 19.0f ) );

        if( !Physics.CheckSphere( pos, 0.1f ) ) {
            GameObject obj = (GameObject) Instantiate( crate, pos, Quaternion.identity );
            obj.tag = "Crate";
        }
	}

	void step() {
		if( !stop ) {
			List<GameObject> temp = new List<GameObject>( stuck ? bigouter : outer );
            
			outer.Clear ();

			//Checks if the weeds are stuck (have no more moves)
			stuckCheck = true;

            amtNew = 0;

			//Loop through the list of outer weed blocks and check if they can move (or are out of bounds)
			foreach( GameObject o in temp ) {
                //Take care of wilted blocks
                if( o == null )
                    continue;

				Vector3 pos = o.transform.position;
				if( pos.x > 19 || pos.x < -19 || pos.z > 19 || pos.z < -19 ) {
                    //Destroy ( o );
                    o.GetComponent<Renderer>().material = new Material( Shader.Find( "Diffuse" ) );
                    o.GetComponent<Renderer>().material.color = Color.red;

                    gameOverPanel.SetActive( true );

                    stop = true;

                    GetComponents<AudioSource>()[1].Stop();
					return;
				} else
					generateNext( o );
			}



			if( stuckCheck ) {
                stuck = true;
				if( walls.Count > 0 ) {
					GameObject t = walls[ walls.Count - 1 ];
					walls.RemoveAt ( walls.Count - 1 );
					Destroy ( t );
				}
			}


            growthTime = ( stuck ? 0.01f : (float) Math.Pow( amtNew / 1.5f, 0.5 ) / diffSettings[difficulty][1] );

            Invoke ( "step", growthTime );
		}
	}



	Vector3 numDir( int num ) {
		return ( num == 1 ? new Vector3( 1, 0, 0 ) : ( num == 2 ? new Vector3( 0, 0, -1 ) : ( num == 3 ? new Vector3( -1, 0, 0 ) : new Vector3( 0, 0, 1 ) ) ) );
	}
	bool blockDir( GameObject obj, int dir ) {
		Collider[] hitColliders = Physics.OverlapSphere( obj.transform.position + numDir ( dir ),  0.1f );

		if( hitColliders.Length != 0 & !stuck ) {
            GameObject o = hitColliders[0].gameObject;

			if( o.tag == "Wall" ) {
				bigouter.Add ( obj );
			} else if( o.tag == "Crate") {
                Destroy( o );
                wallNum += 30;

                
                GetComponent<AudioSource>().Play();
                

                notifyText.GetComponent<NotifyTextScript>().setColor( 1 );
                notifyText.GetComponent<NotifyTextScript>().setValue( "+30 Walls" );
                notifyText.GetComponent<NotifyTextScript>().showText();

                return true;
            }
		}

		return hitColliders.Length != 0; 
	}

	void generateNext( GameObject o ) {
        if( o.tag != "Weed" )
            return;

		for( int i = 1; i < 5; i++ ) {
			if( !blockDir ( o, i ) ) {
				stuckCheck = false;
                stuck = false;
                amtNew++; //Speed up weed movement if less are generated

				GameObject obj = (GameObject) Instantiate( block, o.transform.position + numDir ( i ), Quaternion.identity );

				outer.Add ( obj );
			}
		}
	}
}
