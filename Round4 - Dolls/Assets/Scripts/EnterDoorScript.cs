using UnityEngine;
using System.Collections;

public class EnterDoorScript : MonoBehaviour {
	bool played;
	GameObject sc;

	// Use this for initialization
	void Start () {
		sc = GameObject.Find("LindseyVoice");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c) {
		if(c.tag == "Player") {
			if(!played) {
				AudioClip sfx = Resources.Load("Sound/First Floor/where is everyone") as AudioClip;
				sc.SendMessage("PlaySound", sfx);
				played = true;
			}
		}
	}
}
