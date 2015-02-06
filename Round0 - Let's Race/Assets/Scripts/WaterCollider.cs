using UnityEngine;
using System.Collections;

public class WaterCollider : MonoBehaviour {

	public AudioClip waterSplashSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider) {
		audio.PlayOneShot (waterSplashSound);
	}
}
