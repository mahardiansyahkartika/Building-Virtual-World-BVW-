using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
		// mouse look rotation
		float rotationX = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * 3f;
		float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 1f;
		transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
		
		// keyboard movement
		Vector3 moveVector = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;

		transform.Translate(moveVector * 10 * Time.deltaTime, Space.World);
		Vector3 position = transform.position;
	}
}
