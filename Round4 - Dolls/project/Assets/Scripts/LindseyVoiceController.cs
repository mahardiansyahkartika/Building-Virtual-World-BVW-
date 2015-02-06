using UnityEngine;
using System.Collections;

public class LindseyVoiceController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySound(AudioClip audioClip) {
		audio.clip = audioClip;
		audio.Play();
	}
}
