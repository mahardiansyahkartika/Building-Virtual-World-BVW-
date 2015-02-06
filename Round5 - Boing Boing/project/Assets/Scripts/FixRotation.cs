using UnityEngine;
using System.Collections;

public class FixRotation : MonoBehaviour {
	float lockPos = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x,lockPos,lockPos);
	}
}
