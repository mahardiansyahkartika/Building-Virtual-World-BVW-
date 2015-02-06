using UnityEngine;
using System.Collections;

public class SoundFXCtrl : MonoBehaviour {
	public AudioClip[] soundFX;
	public static SoundFXCtrl instance;
	
	private AudioSource audioSource;
	
	// Use this for initialization
	void Awake () {
		instance = this;
		audioSource = audio;
	}
	
	public float PlaySound(int index, float volumn){
		audioSource.PlayOneShot (soundFX[index], volumn);
		return soundFX [index].length;
	}
}