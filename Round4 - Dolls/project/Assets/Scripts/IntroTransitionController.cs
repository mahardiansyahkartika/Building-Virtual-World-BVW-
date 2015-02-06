using UnityEngine;
using System.Collections;

public class IntroTransitionController : MonoBehaviour {

	private bool isBlackFading = false;
	private bool isTitleFading = false;
	private float totalTimeBlackFading = 1f;
	private float totalTimeTitleFading = 4f;
	private float elapsedTimeBlackFading = 0f;
	private float elapsedTimeTitleFading = 0f;
	private float firstAlpha, targetAlpha;
	public AudioClip clip;
	
	GameObject child;

	// Use this for initialization
	void Start () {
		child = gameObject.transform.GetChild(0).gameObject;	

		// set color to transparent
		renderer.material.SetColor ("_Color", new Color (0, 0, 0, 0));
		child.renderer.material.SetColor ("_Color", new Color (1, 1, 1, 0));
	}
	
	// Update is called once per frame
	void Update () {
		if (isBlackFading) {
			elapsedTimeBlackFading += Time.deltaTime;

			if (elapsedTimeBlackFading >= totalTimeBlackFading) {
				isBlackFading = false;
				elapsedTimeBlackFading = totalTimeBlackFading;

				// sound
				GameObject.Find("SoundObject").SendMessage("PlayClip",clip);
				
				// fade in title
				StartCoroutine("ShowTitle", 0f);
				
				// fade out title
				StartCoroutine("HideTitle", clip.length - (totalTimeTitleFading + 0.7f));
			}

			Color tintColor = renderer.material.GetColor("_Color");
			renderer.material.SetColor("_Color", new Color(tintColor.r, tintColor.g, tintColor.b, elapsedTimeBlackFading / totalTimeBlackFading));
		}

		if (isTitleFading) {
			elapsedTimeTitleFading += Time.deltaTime;

			if (elapsedTimeTitleFading >= totalTimeTitleFading) {
				isTitleFading = false;
				elapsedTimeTitleFading = totalTimeTitleFading;

				if (targetAlpha == 0) {
					Application.LoadLevel(Application.loadedLevel + 1);
				}
			}
			Color prevColor = child.renderer.material.GetColor("_Color");
			child.renderer.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, firstAlpha + ((targetAlpha - firstAlpha) * (elapsedTimeTitleFading / totalTimeTitleFading))));
		}
	}

	public void StartTransition() {
		// fade in
		isBlackFading = true;
		elapsedTimeBlackFading = 0f;
	}

	IEnumerator ShowTitle(float delay) {
		yield return new WaitForSeconds (delay);
		// fade in
		isTitleFading = true;
		firstAlpha = 0f;
		targetAlpha = 1f;
		elapsedTimeTitleFading = 0f;
	}

	IEnumerator HideTitle(float delay) {
		yield return new WaitForSeconds (delay);
		// fade out
		isTitleFading = true;
		firstAlpha = 1f;
		targetAlpha = 0f;
		elapsedTimeTitleFading = 0f;
	}
}
