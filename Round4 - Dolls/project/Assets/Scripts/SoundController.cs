using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
	public AudioClip[] ac;
	private AudioSource audioSource;
	private float recordtime;
	private float interve;

	enum soundName{
		whereiseveryone,
		someonedown,
		screaming,
		doorSound
	};

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		recordtime = 0f;
		interve = 1f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySound(int n) {
		if(recordtime <= Time.time + interve) {
			recordtime = Time.time;
			audioSource.clip = ac[n];
			audioSource.Play();
		}
	}
}
