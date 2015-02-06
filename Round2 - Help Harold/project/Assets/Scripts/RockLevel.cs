using UnityEngine;
using System.Collections;

public class RockLevel : MonoBehaviour {

	private bool oneChance = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] rockObject = GameObject.FindGameObjectsWithTag ("DrawnObject");

		if (rockObject.Length <= 0 && oneChance) {
			oneChance = false;
			audio.Play();
		}
	}
}
