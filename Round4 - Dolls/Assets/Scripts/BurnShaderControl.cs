using UnityEngine;
using System.Collections;

public class BurnShaderControl : MonoBehaviour {
	float f = 0;
	// Use this for initialization
	void Start () {
		renderer.material.SetFloat("_SliceAmount", f);
	}
	
	// Update is called once per frame
	void Update () {
		if(f < 1) {
			f += 0.0001f;
			renderer.material.SetFloat("_SliceAmount", f);
		}
		//renderer.material.SetFloat("_SliceAmount", 0.1f);
		//renderer.material.SetFloat("_SliceAmount", 0.5);
	}
}
