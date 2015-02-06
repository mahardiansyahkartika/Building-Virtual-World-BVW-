using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public GameObject soundfx;
	public bool mute;

	// Use this for initialization
	void Start () {
		if(Application.loadedLevel == 1) {
			//PlaySound ("Bgm_new",0.2f, true);
			StartCoroutine(PlayWithDely(4f, "Bgm_new"));
		}else if(Application.loadedLevel == 0) {
			PlaySound ("new_select_bgm",1f, true);
		}
		//PlaySound ("dig",1f	, true);
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (CleanUp());
	}

	public GameObject PlaySound(string s, float vl = 1f, bool lop = false) { //value vloume
		if (mute) {
			vl = 0;
		}
		GameObject soundOutput = Instantiate (soundfx) as GameObject;
		soundOutput.transform.parent = GameObject.Find ("SoundSets").transform;
		AudioSource audioS = GetAS (soundOutput);
		audioS.clip = Load(s);
		audioS.volume = vl;
		audioS.loop = lop;
		GetAS (soundOutput).Play ();
		return soundOutput;
	}

	AudioClip Load(string s) {
		string path = "SoundTrack/" + s;
		if (!Resources.Load (path)) {
			string path_new = "SoundTrack/" + s + "/" + s + Random.Range (0, 6);
			return Resources.Load (path_new) as AudioClip;
		} else {
			return Resources.Load ("SoundTrack/" + s) as AudioClip;
		}
	}

	AudioSource GetAS(GameObject go) {
		return go.GetComponent<AudioSource> ();
	}

	IEnumerator CleanUp() {
		yield return new WaitForSeconds (1f);
		foreach(Transform tf in GameObject.Find("SoundSets").transform) {
			if(!tf.gameObject.audio.isPlaying) {
				Destroy(tf.gameObject,1f);
			}
		}
	}

	IEnumerator PlayWithDely(float delay_time, string name) {
		yield return new WaitForSeconds(delay_time);
		if (name == "Bgm_new") {
						PlaySound (name, 0.8f, true);
				} else {
						PlaySound (name, 1f, false);
				}
	}
}
