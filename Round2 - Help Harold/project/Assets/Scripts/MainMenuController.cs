using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	bool oneChance = true;

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.tag == "SmallLine" && oneChance) {
			oneChance = false;

			audio.Play();

			Destroy(other.gameObject.transform.parent.gameObject.rigidbody2D);

			//Application.LoadLevel (Application.loadedLevel + 1);
			StartCoroutine(GoToNextScene());
		}
	}
	
	IEnumerator GoToNextScene() 
	{
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
