using UnityEngine;
using System.Collections;

public class ResultController : MonoBehaviour {

	public GameObject winObject;
	public GameObject loseObject;
	public GameObject blackObject;

	protected float totalTime = 2f;
	protected float targetAlpha = 0.7f;

	protected float elapsedTime = 0f;
	protected bool isAnimating = false;

	protected bool isWinLose = false;
	
	public AudioClip winAudio, loseAudio;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isAnimating) {
			elapsedTime += Time.deltaTime;

			if (elapsedTime >= totalTime) {
				elapsedTime = totalTime;

				isAnimating = false;
			}

			// set alpha
			float alpha = (elapsedTime / totalTime) * targetAlpha;

			blackObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,alpha);
		}
	}

	public void Win() {
		if (!isWinLose) {
			isAnimating = true;
			elapsedTime = 0;
			isWinLose = true;

			Invoke("PlayWinSound", totalTime);
			Invoke("GoToCreditScene", totalTime + 3f);

			iTween.MoveTo (winObject, iTween.Hash (
				"y", 0,
				"time", totalTime
				));
		}
	}

	public void Lose() {
		if (!isWinLose) {
			isAnimating = true;
			elapsedTime = 0;
			isWinLose = true;

			Invoke("PlayLoseSound", totalTime);
			Invoke("GoToCreditScene", totalTime + 3f);

			iTween.MoveTo (loseObject, iTween.Hash (
				"y", 0,
				"time", totalTime
				));
		}
	}

	public void GoToCreditScene() {
		Application.LoadLevel ("Credits");
	}

	public void PlayLoseSound() {
		audio.clip = loseAudio;
		audio.Play ();
	}

	public void PlayWinSound() {
		audio.clip = winAudio;
		audio.Play ();
	}
}
