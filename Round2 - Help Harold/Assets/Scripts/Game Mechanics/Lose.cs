using UnityEngine;
using System.Collections;

public class Lose : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}

	void OnTriggerEnter2D( Collider2D other)
	{
		if(other.gameObject.tag=="Lose")
		{
			Debug.Log ("You Lose");
			StartCoroutine(LoadOnLoss());
		}
	}

	IEnumerator LoadOnLoss() 
	{
		yield return new WaitForSeconds(GameHUB.LoadGap);
		Application.LoadLevel (Application.loadedLevel);
		
	}
}

