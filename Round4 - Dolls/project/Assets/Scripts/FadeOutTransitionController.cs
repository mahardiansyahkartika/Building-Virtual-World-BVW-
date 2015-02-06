using UnityEngine;
using System.Collections;

public class FadeOutTransitionController : MonoBehaviour {

	private float ambientVolume;

	private bool isFading = false;
	private float totalTimeFading = 2.5f;
	private float elapsedTimeFading = 0f;

	// Use this for initialization
	void Start () {
		Init ();
		StartFade ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isFading) {
			elapsedTimeFading += Time.deltaTime;

			if (elapsedTimeFading >= totalTimeFading) {
				isFading = false;
				elapsedTimeFading = totalTimeFading;
			}
			
			// fade out
			Color prevColor = renderer.material.GetColor("_Color");
			renderer.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 1 - (elapsedTimeFading / totalTimeFading)));
			// ambient fade in
			transform.parent.gameObject.audio.volume = (elapsedTimeFading / totalTimeFading) * ambientVolume;
		}
	}

	void Init() {
		// show the black screen
		renderer.material.SetColor ("_Color", new Color (0, 0, 0, 1));
		// mute the ambient
		GameObject parentGameObject = transform.parent.gameObject;
		ambientVolume = parentGameObject.audio.volume;
		parentGameObject.audio.volume = 0f;
	}

	void StartFade() {
		isFading = true;
		elapsedTimeFading = 0f;

		transform.parent.gameObject.audio.Play ();

		AudioClip sfx = Resources.Load("Sound/First Floor/fashionably late") as AudioClip;
		GameObject.Find("LindseyVoice").SendMessage("PlaySound", sfx);
	}
}
