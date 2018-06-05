using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeView : MonoBehaviour {

	bool isTurning = false;

	float turnDuration = .5f;

	GameObject player;

	Vector3 startPos;
    //Quaternion startRot;

	public int curCamAngle = 0;
	int camAngleMax = 3;

	// Use this for initialization
	void Start () {

		startPos = transform.position;
        //startRot = transform.rotation;
		
	}
	
	// Update is called once per frame
	void Update () {

		FindPlayer ();

		if (Input.GetKeyDown (KeyCode.Q) && isTurning == false) {
			StartCoroutine (DoTurn (90));
			if (curCamAngle <= camAngleMax) {
				curCamAngle++;
			}

			if (curCamAngle > camAngleMax) {
				curCamAngle = 0;
			}
		}

		if (Input.GetKeyDown (KeyCode.E) && isTurning == false) {
			StartCoroutine (DoTurn (-90));
			if (curCamAngle >= 0) {
				curCamAngle--;
			}
			if (curCamAngle < 0) {
				curCamAngle = camAngleMax;
			}
		}
		
	}
		
	public IEnumerator DoTurn(float angle){

		isTurning = true;

		Vector3 rotAxis = Vector3.up;
		Vector3 pivot = player.transform.position;

		Quaternion startRotation = transform.rotation;
		Quaternion endRotation = Quaternion.AngleAxis (angle, rotAxis)*startRotation;

		float rotSpeed = angle / turnDuration;
		float t = 0;

		while (t < turnDuration) {

			t += Time.deltaTime;
			transform.RotateAround (pivot, rotAxis, rotSpeed * Time.deltaTime);
			yield return null;
		}

		transform.rotation = endRotation;

	
		isTurning = false;

	}

    public void resetView() {
        curCamAngle = 0;
    }

	void FindPlayer(){
		if (player == null) {
			try{
				player = GameObject.FindGameObjectWithTag("Player");
			}
			catch{
				transform.position = startPos;
                //transform.rotation = startRot;
			}
		}
	}
}
