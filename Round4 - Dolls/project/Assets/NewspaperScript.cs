using UnityEngine;
using System.Collections;

public class NewspaperScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void thump()
	{
		Debug.Log ("THUMP");
		audio.Play ();
	}
}
