using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {

	public GameObject harold;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D( Collision2D other)
	{
		if(other.gameObject.tag == "Win")
		{
			if (harold != null) {
				harold.audio.Play ();
			}

			StartCoroutine(LoadOnWin());
		}
	}

	IEnumerator LoadOnWin() 
	{
		yield return new WaitForSeconds(GameHUB.LoadGap);
		Application.LoadLevel (Application.loadedLevel + 1);
		
	}
}
