using UnityEngine;
using System.Collections;

public class BasementLightControl : MonoBehaviour {
	Light[] lights;
	public int blinkTimes = 5;
	private bool isblinking;
	public bool playBuzzSound;
	public float interve;
	// Use this for initialization
	void Start () {
		lights = gameObject.GetComponentsInChildren<Light>();
		interve = 4f;
		//StartCoroutine( BlinkLight(lights[0]));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if(!isblinking) {
			StartCoroutine(SelectBlink());
			isblinking = true;
		}
	}

	IEnumerator SelectBlink() {
		int chose = Random.Range(0, lights.Length - 4);
		for(int i = 0; i < chose; i++) {
			int ran = Random.Range(0, lights.Length);
			StartCoroutine(BlinkLight(lights[ran]));
		}
		yield return new WaitForSeconds(interve);
		isblinking = false;

	}

	IEnumerator BlinkLight(Light l) {
		int soundrand = Random.Range(0,2);
		AudioSource lighas = l.gameObject.AddComponent<AudioSource>();
		lighas.clip = Resources.Load("Sound/Light/lb" + soundrand) as AudioClip;

		for(int i =0; i < blinkTimes; i++) {
			float n = Random.Range(0.1f,0.6f);
			l.enabled = false;
			yield return new WaitForSeconds(n);
			l.enabled = true;
			if(playBuzzSound) {
				lighas.Play();
			}
			yield return new WaitForSeconds(n);
		}
	}
}
