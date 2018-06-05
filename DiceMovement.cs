using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DiceMovement : MonoBehaviour {

    public int DiceID;  //used to identify the die in the level uniquely. First Die should be 1, next die 2, etc. required for checkpoints to function.

	public float rollDuration = .2f;

	public float rayDist;

	public bool isRolling = false;

    public bool canMove = true;

	public bool playerOn = false;

    public bool posessed = false;

    public float DiceXOffset;
    public float DiceYOffset;
    public float DiceZOffset;

	public Vector3 dir;

	public LayerMask cullingMask;

	public RoomRotate rotateRoomScript;

	public JB_GameController myGameController;

	public Vector3 fwd = Vector3.forward;
	public Vector3 bkwd = Vector3.back;
	public Vector3 right = Vector3.right;
	public Vector3 left = Vector3.left;

	public ChangeView camAngleScript;
    public ParticleSystem SmokePuff;
    private GameObject SmokeParent;
    private ParticleSystem TempSmoke;

    //public LayerMask m_LayerMask;
    //bool m_Started;

    public Vector3 pivotOffset;

    Renderer myRenderer;
    Material myMaterial;
    Material OrigMaterial1;
    Material OrigMaterial2;
    Material OrigMaterial3;
    Material OrigMaterial4;
    Material OrigMaterial5;
    Material OrigMaterial6;
    //Material PosessedMaterial;
    //Material PosessedMaterial1;
    //Material PosessedMaterial2;
    //Material PosessedMaterial3;
    //Material PosessedMaterial4;
    //Material PosessedMaterial5;
    //Material PosessedMaterial6;

    PlayerMovement playerScript;

    // Use this for initialization
    void Start () {
        SmokeParent = new GameObject();
		try {
			myGameController = GameObject.Find("GameController").GetComponent<JB_GameController>();
		} catch {
			//Debug.Log("MyGameController cannot be loaded.");
		}
        playerScript = FindObjectOfType<PlayerMovement>();

        //rotateRoomScript = FindObjectOfType<RoomRotate> ();

        camAngleScript = FindObjectOfType<ChangeView> ();

        //pivotOffset = transform.position + Vector3.down * 0.5f;

        //m_Started = true;

        DiceXOffset = Mathf.Abs(transform.position.x - (int)transform.position.x);
        DiceYOffset = Mathf.Abs(transform.position.y - (int)transform.position.y);
        DiceZOffset = Mathf.Abs(transform.position.z - (int)transform.position.z);
        //Debug.Log(this.name+" OFFSET= " + DiceXOffset + ", " + DiceYOffset + ", " + DiceZOffset);
    }

	// Update is called once per frame
	void Update () {

		pivotOffset = transform.position + Vector3.down * 0.5f;

		ChangeDirection ();

		if (playerOn && canMove) {
			HandleMovement ();
		}

        if(!playerOn || playerScript != null) {
            if(!playerScript.gameObject.activeSelf) {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetButton("Fire1")) {
                    playerScript.gameObject.SetActive(true);
                    playerScript.UnPosess();
                    UnPosess();
                }
            }
            
        }
	}

	void ChangeDirection(){

		switch (camAngleScript.curCamAngle) {

		case 0:
			fwd = Vector3.forward;
			bkwd = Vector3.back;
			right = Vector3.right;
			left = Vector3.left;
			break;

		case 1:
			fwd = Vector3.right;
			bkwd = Vector3.left;
			right = Vector3.back;
			left = Vector3.forward;
			break;

		case 2:
			fwd = Vector3.back;
			bkwd = Vector3.forward;
			right = Vector3.left;
			left = Vector3.right;
			break;

		case 3:
			fwd = Vector3.left;
			bkwd = Vector3.right;
			right = Vector3.forward;
			left = Vector3.back;
			break;

		default:
			fwd = Vector3.forward;
			bkwd = Vector3.back;
			right = Vector3.right;
			left = Vector3.left;
			break;
			 
		}

	}

	public void HandleMovement(){

		dir = Vector3.zero;

		RaycastHit hit;

		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0) {
			if (!Physics.Raycast (transform.position, fwd, out hit, rayDist, cullingMask, QueryTriggerInteraction.Ignore)) {
				dir = fwd;
			} else {
				if (hit.collider.name == "Switch" && isRolling == false) {
					rotateRoomScript = hit.collider.gameObject.GetComponentInParent<RoomRotate> ();
					if (rotateRoomScript.isRotating == false) {
						dir = fwd;
						StartCoroutine (rotateRoomScript.RotateToWall (dir, pivotOffset));
					}
				} else if (hit.collider.tag == "Boundary"|| hit.collider.tag == "Dice" || hit.collider.name == "DiceBoundary") {
					dir = Vector3.zero;
				}
			}
		}

		if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0) {
			if (!Physics.Raycast (transform.position, left, out hit, rayDist, cullingMask, QueryTriggerInteraction.Ignore)) {
				dir = left;
			} else {
				if (hit.collider.name == "Switch" && isRolling == false) {
					rotateRoomScript = hit.collider.gameObject.GetComponentInParent<RoomRotate> ();
					if (rotateRoomScript.isRotating == false) {
						dir = left;
						StartCoroutine (rotateRoomScript.RotateToWall (dir, pivotOffset));
					}
				} else if (hit.collider.tag == "Boundary" || hit.collider.tag == "Dice") {
					dir = Vector3.zero;
				}
			}
		}

		if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0) {
			if (!Physics.Raycast (transform.position, bkwd, out hit, rayDist, cullingMask, QueryTriggerInteraction.Ignore)) {
				dir = bkwd;
			} else {
				if (hit.collider.name == "Switch" && isRolling == false) {
					rotateRoomScript = hit.collider.gameObject.GetComponentInParent<RoomRotate> ();
					if (rotateRoomScript.isRotating == false) {
						dir = bkwd;
						StartCoroutine (rotateRoomScript.RotateToWall (dir, pivotOffset));
					}
				} else if (hit.collider.tag == "Boundary"|| hit.collider.tag == "Dice") {
					dir = Vector3.zero;
				}
			}
		}

		if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0) {
			if (!Physics.Raycast (transform.position, right, out hit, rayDist, cullingMask, QueryTriggerInteraction.Ignore)) {
				dir = right;
			} else {
				if (hit.collider.name == "Switch" && isRolling == false) {
					rotateRoomScript = hit.collider.gameObject.GetComponentInParent<RoomRotate> ();
					if (rotateRoomScript.isRotating == false) {
						dir = right;
						StartCoroutine (rotateRoomScript.RotateToWall (dir, pivotOffset));
					}
				} else if (hit.collider.tag == "Boundary" || hit.collider.tag == "Dice") {
					dir = Vector3.zero;
				}
			}
		}

		if (dir != Vector3.zero && !isRolling) {
			StartCoroutine (DoRoll (dir));
		}

		//print (myGameController.GetMoves ());

	}

	public IEnumerator DoRoll(Vector3 direction){
		myGameController.UpdateMoves (1);
		isRolling = true;

		Vector3 rotAxis = Vector3.Cross (Vector3.up, direction);
		Vector3 pivot = (transform.position + Vector3.down * 0.5f) + direction * 0.5f;

		Quaternion startRotation = transform.rotation;
		Quaternion endRotation = Quaternion.AngleAxis (90, rotAxis)*startRotation;

		Vector3 endPosition = transform.position + direction;

		float rotSpeed = 90 / rollDuration;
		float t = 0;

		while (t < rollDuration) {

			t += Time.deltaTime;
			transform.RotateAround (pivot, rotAxis, rotSpeed * Time.deltaTime);
			yield return null;
		}
        //Debug.Log("endposition = " + endPosition);
        //snap to grid
        /*
        if (endPosition.x >= 0f) {
            endPosition.x = ((int)endPosition.x + DiceXOffset);
        } else {
            endPosition.x = ((int)(endPosition.x - 1f) + DiceXOffset);
        }
        */
        if (endPosition.y >= 0f) {
            endPosition.y = ((int)endPosition.y + DiceYOffset);
        }else {
            endPosition.y = ((int)(endPosition.y - 1f) + DiceYOffset);
        }
        /*
        if (endPosition.z >= 0f) {
            endPosition.z = ((int)endPosition.z + DiceZOffset);
        } else {
            endPosition.z = ((int)(endPosition.z - 1f) + DiceZOffset);
        }
        */
        //Debug.Log(this.name + " OFFSET= " + DiceXOffset + ", " + DiceYOffset + ", " + DiceZOffset);
        //Debug.Log("Adjusted endposition = " + endPosition);
		transform.rotation = endRotation;
		transform.position = endPosition;
		//handles sound effect for rolling dice
		myGameController.PlayDieRollSound();
        Vector3 smokePosition = gameObject.transform.position;
        smokePosition.y = smokePosition.y - .0f;
        Quaternion smokeRotation = new Quaternion(0f, 0f, 0f, 0f);
        SmokeParent.transform.position = smokePosition;
        SmokeParent.transform.rotation = smokeRotation;
        //ParticleSystem mySmoke = (ParticleSystem) Instantiate(SmokePuff, SmokeParent.transform);
        Instantiate(SmokePuff, SmokeParent.transform);
        //Destroy(mySmoke, GetComponent<ParticleSystem>().main.duration);
        isRolling = false;
	}

    public void SetCanMove(bool newStatus) {
        canMove = newStatus;
    }
    
    public bool getRolling() {
        return isRolling;
    }

    public float GetDiceXOffset() {
        return DiceXOffset;
    }
    public float GetDiceYOffset() {
        return DiceYOffset;
    }
    public float GetDiceZOffset() {
        return DiceZOffset;
    }

    public void Posess() {
        //Debug.Log("changing my color to yellow");
        //myRenderer = gameObject.GetComponent<Renderer>();
        //myRenderer.material.color = Color.yellow;
        gameObject.GetComponentInParent<Highlight>().PosessDieHighlight();
        posessed = true;
    }

    public void UnPosess() {
        //Debug.Log("Changing my color to red");
        //myRenderer = gameObject.GetComponent<Renderer>();
        //myRenderer.material.color = Color.red;
        gameObject.GetComponentInParent<Highlight>().unLockDieHighlight();
        playerOn = false;
        posessed = false;
    }

    public bool isControlled() {
        bool controlled = false;
        if(posessed || playerOn) {
            controlled = true;
        }else {
            controlled = false;
        }
        return controlled;
    }


 

    public void OnGUI(){
		//GUI.Label (new Rect (10, 100, 100, 20),"Moves: " + myGameController.GetMoves());

	}

    /*
    private void FixedUpdate() {
        mycollisions();
    }
    
    void mycollisions() {
        m_LayerMask = 1 << 8;
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        int icol = 0;
        while (icol < hitColliders.Length) {
            Debug.Log("Hit:" + hitColliders[icol].name + icol);
            icol++;
        }
        Debug.Log("length:"+ hitColliders.Length);
    }


    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
    */
}
