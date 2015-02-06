using UnityEngine;
using System.Collections;

public class HaroldWagonAI : MonoBehaviour {

	public AudioClip winSFX, loseSFX;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.tag=="Win")
		{
			audio.PlayOneShot(winSFX);
			StartCoroutine( LoadOnWin() );
		}

		if(other.gameObject.tag=="Lose")
		{
			audio.PlayOneShot(loseSFX);
			StartCoroutine( LoadOnLoss() );
		}
	}

	IEnumerator LoadOnWin()
	{
		yield return new WaitForSeconds (2.0f);
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	IEnumerator LoadOnLoss()
	{
		yield return new WaitForSeconds (2.0f);
		Application.LoadLevel (Application.loadedLevel);
	}
}
