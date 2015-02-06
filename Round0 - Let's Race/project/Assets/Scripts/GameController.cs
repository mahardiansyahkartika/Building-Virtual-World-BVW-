using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	GameObject player;
	
	public Transform[] startPos;

	public GUIText countdownText;

	float totalCountDown = 3f;
	bool countdownFlag = true;

	int currentCountDown = 3;

	// global variable
	public static bool isFreeze = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (countdownFlag) {
			StartCountDown ();
		}
	}

	void StartCountDown() {
		if (isFreeze) {
			totalCountDown -= Time.deltaTime;
			int countDown = Mathf.CeilToInt(totalCountDown);
			countdownText.text = countDown.ToString();

			// play sound
			if (currentCountDown > countDown) {
				currentCountDown = countDown;
				Camera.main.gameObject.SendMessage("PlayCountDown" + (3-currentCountDown));
			}

			if (totalCountDown <= 0) {
				totalCountDown = 0;
				isFreeze = false;
				countdownFlag = false;
				countdownText.enabled = false;
			}
		}
	}

	void SpawnPlayerCar() {
		// instantiate
		player = Instantiate (Resources.Load ("Prefabs/car")) as GameObject;
		// set position
		player.transform.position = new Vector3(startPos[0].position.x, -0.4f, startPos[0].position.z);
	}
}
