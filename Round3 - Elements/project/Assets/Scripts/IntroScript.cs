using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {

	bool oneChance = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Hand") 
		{
			if (oneChance) {
				oneChance = false;

				StartCoroutine (GoToNextLevel());
			}
		}
	}

	IEnumerator GoToNextLevel() {
		//audio.Play ();

		yield return new WaitForSeconds (2f);

		Application.LoadLevel(1);		
	}
}
