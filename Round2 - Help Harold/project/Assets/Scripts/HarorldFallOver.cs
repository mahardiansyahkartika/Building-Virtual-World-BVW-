using UnityEngine;
using System.Collections;

public class HarorldFallOver : MonoBehaviour 
{

	private bool oneChance = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if ( (other.gameObject.tag != "Kite") && (KiteBehaviour.WinFlag == 0))
		{
			if (oneChance) {
				oneChance = false;

				Debug.Log ("He Fell Over");
				StartCoroutine(LoadOnFall());

				audio.Play();
			}
		}

	}

	IEnumerator LoadOnFall() 
	{
		yield return new WaitForSeconds(3.0f);
		Application.LoadLevel (Application.loadedLevel);
		
	}
}
