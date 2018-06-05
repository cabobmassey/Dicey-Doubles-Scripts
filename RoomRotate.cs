using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRotate : MonoBehaviour {

	public float rotTime = .2f;

	public bool isRotating = false;

	DiceMovement diceScript;

	public float roundToX;
	public float roundToY;
	public float roundToZ;

	// Use this for initialization
	void Start () {
		
		//diceScript = FindObjectOfType<DiceMovement> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator RotateToWall(Vector3 direction, Vector3 pivotOffset){
        Debug.Log("rotating "+gameObject.name);
		isRotating = true;

		Vector3 rotAxis = Vector3.Cross (Vector3.up, direction);
		Vector3 pivot = pivotOffset + direction * 0.5f;

		Quaternion startRotation = transform.rotation;
		Quaternion endRotation = Quaternion.AngleAxis (90, rotAxis) * startRotation;

		Vector3 endPosition = transform.position;

		float rotSpeed = 90 / rotTime;
		float t = 0;

		while (t < rotTime) {

			t += Time.deltaTime;
			transform.RotateAround (pivot, rotAxis, rotSpeed * Time.deltaTime);
			yield return null;
		}
        
        if(roundToX !=0) {
            endPosition.x = Mathf.Round(transform.position.x / roundToX) * roundToX;
        } else {
            endPosition.x = transform.position.x;
        }
        if (roundToY != 0) {
            endPosition.y = Mathf.Round(transform.position.y / roundToY) * roundToY;
        } else {
            endPosition.y = transform.position.y;
        }
        if (roundToZ != 0) {
            endPosition.z = Mathf.Round(transform.position.z / roundToZ) * roundToZ;
        } else {
            endPosition.z = transform.position.z;
        }
        /*
        if (roundToX != 0 && roundToY != 0 && roundToZ != 0) {
            endPosition.x = Mathf.Round(transform.position.x / roundToX) * roundToX;
            endPosition.z = Mathf.Round(transform.position.z / roundToZ) * roundToZ;
            endPosition.y = Mathf.Round(transform.position.y / roundToY) * roundToY;
        } else {
            Debug.Log("roundTo = 0");
            endPosition.x = transform.position.x;
            endPosition.z = transform.position.z;
            endPosition.y = transform.position.y;
        }
        */
		transform.rotation = endRotation;
		transform.position = endPosition;

		isRotating = false;

	}
		
}
