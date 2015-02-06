using UnityEngine;
using System.Collections;

public class WinLevel1 : MonoBehaviour {

	private int WheelCount=0;
	private bool oneChance = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Wheel")
		{
			WheelCount++;
		}

		if(WheelCount>1)
		{
			if (oneChance) {
				oneChance = false;

				audio.Play();
				StartCoroutine(LoadOnWin());
			}
		}
	}


	IEnumerator LoadOnWin()
	{
		yield return new WaitForSeconds (4.0f);
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
