using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {

	public Vector3 distance;
	public Vector3 rotation;
	public bool isRotate = false;
	public float time = 1f;
	public float delay = 0f;
	public iTween.EaseType easeType = iTween.EaseType.linear;

	// Use this for initialization
	void Start () {
		StartCoroutine ("Animate", delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Animate(float delay) {
		yield return new WaitForSeconds(delay);

		if (isRotate) {
			iTween.RotateAdd(this.gameObject, iTween.Hash (
				"x", rotation.x,
				"y", rotation.y,
				"z", rotation.z,
				"time", time,
				"easetype", easeType,
				"looptype", iTween.LoopType.pingPong
			));
		} else {
			iTween.MoveBy (this.gameObject, iTween.Hash (
				"x", distance.x,
				"y", distance.y,
				"z", distance.z,
				"time", time,
				"easetype", easeType,
				"looptype", iTween.LoopType.pingPong
			));
		}
	}
}
