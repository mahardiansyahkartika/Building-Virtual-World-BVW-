using UnityEngine;
using System.Collections;

public class FadeInScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (fadein ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator fadein()
	{
		yield return new WaitForSeconds (38.0f);
		Color col = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		for (float i=0; i<2.0f; i+=Time.deltaTime) 
		{
			col.a = i/2.0f;
			this.renderer.material.SetColor("_Color",col);
			yield return null;
		}
	}

}
