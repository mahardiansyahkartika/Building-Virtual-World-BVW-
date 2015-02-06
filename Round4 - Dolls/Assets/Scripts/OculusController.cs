using UnityEngine;
using System.Collections;

public class OculusController : MonoBehaviour {

	public GameObject leftOculusCamera, rightOculusCamera;

	private bool isOculusActive = true;
	public bool isUsingMouse = false;

	Vector2 rotationRange = new Vector3 (130, 3600);
	Vector3 targetAngles, followAngles, followVelocity;
	Quaternion originalRotation;
	float rotationSpeed = 5;
	float dampingTime = 0.2f;

	GameObject OVRCameraController;

	// Use this for initialization
	void Start () {
		OVRCameraController = GameObject.Find ("OVRCameraController");

		if (isUsingMouse) {
			SetEnableOculus(false);
		}

		originalRotation = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (isOculusActive) {
			// update transform direction of player
			Vector3 forward = rightOculusCamera.transform.forward;
			forward.y = 0f;
			
			transform.forward = forward;
		}

		// change oculus with mouse
		if (isUsingMouse) {
			// we make initial calculations from the original local rotation
			transform.localRotation = originalRotation;

			float inputH = CrossPlatformInput.GetAxis("Mouse X");
			float inputV = CrossPlatformInput.GetAxis("Mouse Y");

			// wrap values to avoid springing quickly the wrong way from positive to negative
			if (targetAngles.y > 180) { targetAngles.y -= 360; followAngles.y -= 360; }
			if (targetAngles.x > 180) { targetAngles.x -= 360; followAngles.x-= 360; }
			if (targetAngles.y < -180) { targetAngles.y += 360; followAngles.y += 360; }
			if (targetAngles.x < -180) { targetAngles.x += 360; followAngles.x += 360; }

			targetAngles.y += inputH * rotationSpeed;
			targetAngles.x += inputV * rotationSpeed;
			
			// clamp values to allowed range
			targetAngles.y = Mathf.Clamp ( targetAngles.y, -rotationRange.y * 0.5f, rotationRange.y * 0.5f );
			targetAngles.x = Mathf.Clamp ( targetAngles.x, -rotationRange.x * 0.5f, rotationRange.x * 0.5f );

			// smoothly interpolate current values to target angles
			followAngles = Vector3.SmoothDamp( followAngles, targetAngles, ref followVelocity, dampingTime );
			
			// update the actual gameobject's rotation
			transform.localRotation = originalRotation * Quaternion.Euler(0, followAngles.y, 0);
			OVRCameraController.transform.localRotation = Quaternion.Euler(-followAngles.x, OVRCameraController.transform.localRotation.eulerAngles.y, OVRCameraController.transform.localRotation.eulerAngles.z);
		}
	}

	public void SetEnableOculus(bool value) {
		isOculusActive = value;

		// oculus
		leftOculusCamera.GetComponent<OVRCamera> ().isUpdateRotation = isOculusActive;
		rightOculusCamera.GetComponent<OVRCamera> ().isUpdateRotation = isOculusActive;
	}
}
