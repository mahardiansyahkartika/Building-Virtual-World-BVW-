using UnityEngine;
using System.Collections;

public class MenuCameraController : MonoBehaviour {
	
	public BezierSpline path;
	public float duration;

	private float elapsedTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		
		// end 
		if (elapsedTime >= duration) {
			elapsedTime = duration;
		} else {
			float progress = elapsedTime / duration;
			
			// position
			Vector3 position = path.GetPoint(progress);
			transform.localPosition = position;
			// rotation
			//transform.LookAt(position + path.GetDirection(progress));
		}
	}
}
