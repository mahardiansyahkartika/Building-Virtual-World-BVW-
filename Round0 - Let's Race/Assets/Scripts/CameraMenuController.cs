using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CameraMenuController : MonoBehaviour {

	public AudioClip engineStart;

	public GameObject player;

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
	}

	// Update is called once per frame
	void Update () {
		transform.LookAt (player.transform);
	}

	void PlayEngineStart() {
		audio.PlayOneShot (engineStart);
	}
}
