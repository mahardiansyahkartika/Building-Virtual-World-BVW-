using UnityEngine;
using System.Collections;

public class PianoKeys : MonoBehaviour {

	public enum DirectionType {IDLE, PRESS, RELEASE};
	public bool isBlack = false;

	private DirectionType directionType = DirectionType.IDLE;

	private bool isHitting = false;

	private float maxRotation;

	private bool isAnimating = false;
	private float elapsedTimeAnimation = 0f;
	private float timeAnimation = 0.2f;
	private Quaternion firstRotation, targetRotation;

	// Use this for initialization
	void Start () {
		maxRotation = isBlack ? 8 : 4;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAnimating) {
			elapsedTimeAnimation += Time.deltaTime;

			transform.localRotation = Quaternion.Slerp(firstRotation, targetRotation, elapsedTimeAnimation / timeAnimation);

			// onComplete
			if (elapsedTimeAnimation >= timeAnimation) {
				if (directionType == DirectionType.PRESS) {
					audio.Play ();
					Init();
					Release();
				} else if (directionType == DirectionType.RELEASE) {
					isHitting = false;
					Init();
				}
			}
		}
	}

	void Init() {
		// init
		directionType = DirectionType.IDLE;
		isAnimating = false;
		elapsedTimeAnimation = 0f;
	}

	void OnTriggerEnter(Collider other) {
		if (!isHitting && other.gameObject.tag == "Hand") {
			Hit();
		}
	}

	void Hit() {
		if (!isAnimating && directionType == DirectionType.IDLE) {
			isAnimating = true;
			firstRotation = transform.localRotation;
			targetRotation = Quaternion.Euler (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z + maxRotation);
			directionType = DirectionType.PRESS;

			isHitting = true;
		}
	}

	void Release() {
		if (!isAnimating && directionType == DirectionType.IDLE) {
			isAnimating = true;
			firstRotation = transform.localRotation;
			targetRotation = Quaternion.Euler (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z - maxRotation);
			directionType = DirectionType.RELEASE;
		}
	}
}
