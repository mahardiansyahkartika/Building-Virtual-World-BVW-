using UnityEngine;
using System.Collections;

public class RigController : MonoBehaviour {
	FirstPersonCharacter fpc;
	public bool isBasement;
	AudioSource basementSound;
	// Use this for initialization
	void Start () {
		fpc = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonCharacter>();
		basementSound = GameObject.Find("BasementSound").GetComponent<AudioSource>();
		if(gameObject.name == "Basement") {
			isBasement = true;
			//
		}else {
			isBasement = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider c) {
		if(c.tag == "Player") {
			print(isBasement);
			if(isBasement) {
				fpc.SendMessage("InBasement");
				basementSound.transform.position = fpc.transform.position;
				basementSound.transform.parent = fpc.transform;
				basementSound.Play();
			}else {
				fpc.SendMessage("OnRig");
			}
		}
	}

	void OnTriggerExit() {
		fpc.SendMessage("BackToNormalFloor");
	}
}