using UnityEngine;
using System.Collections;

public class MoveBeast : MonoBehaviour {
	
	GameObject bar;

	// Use this for initialization
	void Start () {
		int totalPath = GameObject.FindGameObjectWithTag ("PathSource").transform.childCount;
		Vector3[] path = iTweenPath.GetPath ("path" + Mathf.Round (Random.Range (1f, (float)totalPath)));

		this.gameObject.transform.localPosition = path [0];

		bar = gameObject.transform.GetChild (1).gameObject;

		iTween.MoveTo (gameObject, iTween.Hash(
			"path", path,
			"speed", Random.Range(0.5f, 1.5f),
			"orienttopath", true,
			"axis", "y",
			"easetype", iTween.EaseType.linear
		));
	}
	
	// Update is called once per frame
	void Update () {
		bar.transform.rotation = Quaternion.Euler (Vector3.zero);
		//rigidbody2D.transform.Translate (Vector2.right * Time.deltaTime * 7.5f);
	}

	public void Hit(float damage) {

	}
}
