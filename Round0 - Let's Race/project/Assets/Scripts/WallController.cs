using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

	public AudioClip crashAudio;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.name == "car") {
			audio.volume = collision.relativeVelocity.magnitude / 32f;
			audio.PlayOneShot(crashAudio);
		}
	}
}
