using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdsController : MonoBehaviour {
	
	public BezierSpline path;
	public float duration;

	public List<GameObject> birdList;
	public Vector3[] positionList;

	private bool isHit = false;

	private float elapsedTime = 0f;
	private float elapsedTimeSingleBird = 0f;
	private float spreadingTime = 1.5f;
	private float maxDistanceSpread = 5f;

	private int victimIndex;
	private Quaternion[] fromRotationList;
	private Quaternion[] targetRotationList;
	private Vector3[] fromPositionList;
	private Vector3[] middlePositionList;
	private Vector3[] targetPositionList;
	private float[] distanceSpreadList;

	// Use this for initialization
	void Start () {
		// save birds position
		positionList = new Vector3[birdList.Count];
		for (int i = 0; i < birdList.Count; i++) {
			positionList [i] = birdList [i].transform.localPosition;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
			transform.LookAt(position + path.GetDirection(progress));
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			Hit(birdList[Random.Range(0,birdList.Count - 1)]);
		}

		if (isHit) {
			elapsedTimeSingleBird += Time.deltaTime;

			// end
			if (elapsedTimeSingleBird >= spreadingTime) {
				// init all
				isHit = false;
				elapsedTimeSingleBird = 0f;
				for (int i = 0; i < birdList.Count; i++) {
					birdList[i].transform.localRotation = fromRotationList[i];
				}
			} else {
				UpdateBird ();
			}
		}
	}

	public void Hit(GameObject victimBird) {
		if (!isHit) {
			bool isMember = false;
			// check is victim in the list
			for (int i = 0; i < birdList.Count; i++) {
				if (birdList[i].Equals(victimBird)) {
					isMember = true;
					victimIndex = i;
					break;
				}
			}
			
			if (isMember) {
				isHit = true;

				// delete one bird
				GameObject victimObject = birdList[victimIndex];
				birdList.RemoveAt(victimIndex);

				SoundFXCtrl.instance.PlaySound(2,1);

				if (birdList.Count == 0) { // LOSE Condition
					Debug.Log("YOU LOSEEEEEEEEEEEEEEEEEEE!!!!!!!!!!!!!!!!");
					Application.LoadLevel("loseScene");
				}

				// go anywhere
				fromRotationList = new Quaternion[birdList.Count];
				targetRotationList = new Quaternion[birdList.Count];
				fromPositionList = new Vector3[birdList.Count];
				middlePositionList = new Vector3[birdList.Count];
				targetPositionList = new Vector3[birdList.Count];
				distanceSpreadList = new float[birdList.Count];

				int limitRotation = 45;

				for (int i = 0; i < birdList.Count; i++) {
					// rotation
					fromRotationList[i] = birdList[i].transform.localRotation;
					targetRotationList[i] = Quaternion.Euler (birdList[i].transform.localRotation.eulerAngles + new Vector3(Random.Range(-limitRotation,limitRotation), Random.Range(-limitRotation, limitRotation), Random.Range(-limitRotation, limitRotation)));
					// position
					fromPositionList[i] = birdList[i].transform.localPosition;
					targetPositionList[i] = positionList[i];

					float distanceSpread = Random.Range(1f, maxDistanceSpread);
					distanceSpreadList[i] = distanceSpread;

					middlePositionList[i] = ((fromPositionList[i] + targetPositionList[i]) / 2f) + new Vector3(Random.Range(-distanceSpread, distanceSpread),Random.Range(-distanceSpread, distanceSpread),Random.Range(-distanceSpread, distanceSpread));
				}
			} else {
				Debug.LogError(victimBird.name + " is not member of birdList");
			}				
		}
	}

	private void UpdateBird() {
		for (int i = 0; i < birdList.Count; i++) {
			Quaternion rotation;

			if (elapsedTimeSingleBird <= spreadingTime / 2f) {
				if (elapsedTimeSingleBird <= spreadingTime / 4f) {
					float time = elapsedTimeSingleBird / (spreadingTime / 4f);
					rotation = Quaternion.Slerp(fromRotationList[i], targetRotationList[i], time);
				} else {
					float time = ((elapsedTimeSingleBird - (spreadingTime / 4f)) / (spreadingTime / 4f));
					rotation = Quaternion.Slerp(targetRotationList[i], fromRotationList[i], time);
				}
				birdList [i].transform.localRotation = rotation;

				float _time = elapsedTimeSingleBird / (spreadingTime / 2f);
				birdList [i].transform.localPosition = Vector3.Lerp(fromPositionList[i], middlePositionList[i], _time); 
			} else {
				if (elapsedTimeSingleBird <= spreadingTime * 3f / 4f) {
					float time = ((elapsedTimeSingleBird - (spreadingTime / 2f)) / (spreadingTime / 4f));
					Quaternion tempTargetRotation = Quaternion.Euler(fromRotationList[i].eulerAngles - (targetRotationList[i].eulerAngles - fromRotationList[i].eulerAngles));
					rotation = Quaternion.Slerp(fromRotationList[i], tempTargetRotation, time);
				} else {
					float time = ((elapsedTimeSingleBird - (spreadingTime * 3f / 4f)) / (spreadingTime / 4f));
					Quaternion tempTargetRotation = Quaternion.Euler(fromRotationList[i].eulerAngles - (targetRotationList[i].eulerAngles - fromRotationList[i].eulerAngles));
					rotation = Quaternion.Slerp(tempTargetRotation, fromRotationList[i], time);
				}
				birdList [i].transform.localRotation = rotation;

				float _time = ((elapsedTimeSingleBird - (spreadingTime / 2f)) / (spreadingTime / 2f));
				birdList [i].transform.localPosition = Vector3.Lerp(middlePositionList[i], targetPositionList[i], _time); 
			}
		}
	}
}
