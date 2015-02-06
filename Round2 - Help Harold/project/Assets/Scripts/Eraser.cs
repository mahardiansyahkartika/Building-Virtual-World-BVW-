using UnityEngine;
using System.Collections;

public class Eraser : MonoBehaviour {

	public bool isReadyErasing = false;
	public bool isAnimatingFirst = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D (Collider2D other) {
		if (isReadyErasing) {
			if (other.tag == "SmallLine") {
				audio.Play();
				other.gameObject.transform.parent.gameObject.SendMessage ("DeleteSmallLine", other);
			}
		}
	}
	
	void OnTriggerExit2D (Collider2D other) {

	}

	void OnTriggerStay2D (Collider2D other) {

	}
}
