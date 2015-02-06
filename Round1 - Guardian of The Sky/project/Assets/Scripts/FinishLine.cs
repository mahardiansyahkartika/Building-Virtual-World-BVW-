using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up, Time.deltaTime * 50f);
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.name == "BirdGroup") {
			// you win
			Application.LoadLevel ("winScene");
		}
	}
}
