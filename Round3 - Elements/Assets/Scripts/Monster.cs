using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.MoveTo (this.gameObject, 
		               iTween.Hash(
							"path", iTweenPath.GetPath("path1"), 
							"time", 5f,
		               		"easetype", iTween.EaseType.linear
							)
		               );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "Attack")
			Destroy(gameObject);
	}
}
