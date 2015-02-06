using UnityEngine;
using System.Collections;

public class MessagingController : MonoBehaviour {

	bool played = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider c) {
		if(c.tag == "Player") {
			if(!played) {
				played = true;

				GameObject.FindGameObjectWithTag ("Phone").SendMessage ("Show",1);
			}
		}
	}
}
