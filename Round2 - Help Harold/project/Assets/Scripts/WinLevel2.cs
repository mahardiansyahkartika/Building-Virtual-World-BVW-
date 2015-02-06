using UnityEngine;
using System.Collections;

public class WinLevel2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)

	{
		if(other.gameObject.tag == "Win")
		{
			StartCoroutine(LoadOnWin());
		}
	}

	IEnumerator LoadOnWin()
	{
		yield return new WaitForSeconds (3.0f);
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
