using UnityEngine;
using System.Collections;

public class AnimationEffect : MonoBehaviour {

	public enum AnimationType {Rotate, RotateRange, Translate, RotateTranslate, Scale};

	public AnimationType animationType;
	public float speedRotation, speedTranslation, timeScale;
	public float range, maxScale;
	public Vector3 deltaPosition;

	private Quaternion initRotation;
	private Vector3 initPosition;
	private Vector3 initScale;
	private int directionRotation = 1;
	private int directionTranslation = 1;
	private int directionZoom = 1;
	private float totalDeltaRotation = 0;
	private float totalDeltaTranslation = 0;
	private float totalTimeScale = 0;

	// Use this for initialization
	void Start () {
		initRotation = transform.rotation;
		initPosition = transform.localPosition;
		initScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (animationType == AnimationType.Rotate) {
			transform.Rotate (0, 0, Time.deltaTime * speedRotation);
		} 

		if (animationType == AnimationType.Scale) {
			totalTimeScale += Time.deltaTime;
			float delta = totalTimeScale * (maxScale - 1f) + 1f;

			transform.localScale = initScale * delta;

			if (totalTimeScale >= timeScale) {
				totalTimeScale = 0;
			}
		}

		if (animationType == AnimationType.RotateRange || animationType == AnimationType.RotateTranslate) {
			float delta = Time.deltaTime * speedRotation * directionRotation;
			totalDeltaRotation += Mathf.Abs (delta);

			if (totalDeltaRotation >= range) {
				delta = range - totalDeltaRotation;
				totalDeltaRotation = 0;
				directionRotation *= -1;
			}

			transform.Rotate (0, 0, delta);
		}

		if (animationType == AnimationType.Translate || animationType == AnimationType.RotateTranslate) {
			totalDeltaTranslation += Time.deltaTime * speedTranslation;

			if (directionTranslation == 1) {
				transform.localPosition = Vector3.Slerp(initPosition, initPosition + deltaPosition, totalDeltaTranslation);
			} else if (directionTranslation == -1) {
				transform.localPosition = Vector3.Slerp(initPosition + deltaPosition, initPosition, totalDeltaTranslation);
			}

			if (totalDeltaTranslation >= 1) {
				directionTranslation *= -1;
				totalDeltaTranslation = 0;
			}
		}
	}
}
