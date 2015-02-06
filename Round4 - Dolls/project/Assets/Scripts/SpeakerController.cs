using UnityEngine;
using System.Collections;

public class SpeakerController : MonoBehaviour {
	public GameObject partySound;
	public GameObject someOneDownHere;
	private SoundController sc;
	private float recordtime;
	private float interve;
	private bool stopMusic;
	// Use this for initialization
	void Start () {
		interve = 1f;
		stopMusic = false;
		sc = GameObject.Find("SoundSets").GetComponent<SoundController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c) {
		if(c.tag == "Player" && !stopMusic) {
			toggleSound();
			stopMusic = true;
		}
	}

	void toggleSound() {
		if(recordtime <= Time.time - interve) {
			recordtime = Time.time;
			//sc.PlaySound(4);
			someOneDownHere.audio.Play();
			if(partySound.GetComponent<AudioSource>().isPlaying) {
				partySound.audio.Stop();
			}else {
				partySound.audio.Play();
			}
		}
	}
}