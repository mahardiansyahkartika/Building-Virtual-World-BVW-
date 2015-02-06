using UnityEngine;
using System.Collections;
using Leap;

public class LeapController : MonoBehaviour {

	protected const float GIZMO_SCALE = 5.0f;

	Controller leapController;

	public Vector3 leftHandPosition, rightHandPosition;

	// Convert Position
	Vector2 rangeXScreen = new Vector2 (-10f, 10f);
	Vector2 rangeYScreen = new Vector2 (-5f, 5f);
	Vector2 rangeXLeapMotion = new Vector2 (-300f, 300f);
	Vector2 rangeYLeapMotion = new Vector2 (70f, 350f);

	// Recording parameters.
	public bool enableRecordPlayback = false;
	public TextAsset recordingAsset;
	public float recorderSpeed = 1.0f;
	public bool recorderLoop = true;

	LeapRecorder recorder_ = new LeapRecorder();

	// smoothing attribute
	private float kFilteringFactor = 0.1f;

	void OnDrawGizmos() {
		// Draws the little Leap Motion Controller in the Editor view.
		Gizmos.matrix = Matrix4x4.Scale(GIZMO_SCALE * Vector3.one);
		Gizmos.DrawIcon(transform.position, "leap_motion.png");
	}

	// Use this for initialization
	void Start () {
		UnityEngine.Screen.showCursor = false;

		leapController = new Controller ();

		if (leapController == null) {
			Debug.LogWarning(
				"Cannot connect to controller. Make sure you have Leap Motion v2.0+ installed");
		}
		
		if (enableRecordPlayback && recordingAsset != null)
			recorder_.Load(recordingAsset);		
	}
	
	// Update is called once per frame
	void Update () {
		if (leapController == null) {
			Debug.Log("leapController null");
			return;
		}

		// update leap motion
		UpdateRecorder ();
		Frame frame = GetFrame();

		// get hand
		HandList hands = frame.Hands;
		
		Hand leftHand = GetLeftHand (hands);
		Hand rightHand = GetRightHand (hands);

		// left hand
		if (leftHand != null) {
			// smoothing algorithm
			leftHandPosition = (ConvertLeadToScreenPosition (leftHand.PalmPosition) * kFilteringFactor) + (leftHandPosition * (1.0f - kFilteringFactor));
		}
		// right hand
		if (rightHand != null) {
			// smoothing algorithm
			rightHandPosition = (ConvertLeadToScreenPosition (rightHand.PalmPosition) * kFilteringFactor) + (rightHandPosition * (1.0f - kFilteringFactor));
		}
	}

	protected Vector3 ConvertLeadToScreenPosition(Vector leapPos) {
		float posX = -(((rangeXLeapMotion.y - leapPos.x) * (rangeXScreen.y - rangeXScreen.x) / (rangeXLeapMotion.y - rangeXLeapMotion.x)) - rangeXScreen.y);
		float posY = -(((rangeYLeapMotion.y - leapPos.y) * (rangeYScreen.y - rangeYScreen.x) / (rangeYLeapMotion.y - rangeYLeapMotion.x)) - rangeYScreen.y);

		return new Vector3 (posX, posY, 0f);
	}

	protected Hand GetLeftHand(HandList handList) {
		foreach (Hand hand in handList) {
			if (hand.IsLeft) {
				return hand;
			}
		}

		return null;
	}

	protected Hand GetRightHand(HandList handList) {
		foreach (Hand hand in handList) {
			if (hand.IsRight) {
				return hand;
			}
		}
		
		return null;
	}

	protected Frame GetFrame() {
		if (enableRecordPlayback && recorder_.state == RecorderState.Playing)
			return recorder_.GetCurrentFrame();
		
		return leapController.Frame();
	}

	protected void UpdateRecorder() {
		if (!enableRecordPlayback)
			return;
		
		recorder_.speed = recorderSpeed;
		recorder_.loop = recorderLoop;
		
		if (recorder_.state == RecorderState.Recording) {
			recorder_.AddFrame(leapController.Frame());
		}
		else {
			recorder_.NextFrame();
		}
	}
}
