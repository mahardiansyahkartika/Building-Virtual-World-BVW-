using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	
	public GameObject wheelFL, wheelFR, wheelRL, wheelRR;

	public WheelCollider colliderWheelFL, colliderWheelFR, colliderWheelRL, colliderWheelRR;
	private WheelCollider[] colliderWheelList;

	// audio
	public AudioClip engineSound, skidSound, bombSound;

	// velocity
	GUIText speedometerText;
	// distance
	GUIText distanceText;
	float totalDistance = 0f;
	// time
	float initialTime = 60f;
	float totalTime;
	GUIText timerLeftText, timerRightText;
	// lap
	int lap = 0;
	GUIText lapText;

	float enginePower = 150.0f;
	float maxSteer = 40.0f;
	float power = 0f;
	float steer = 0f;

	float frontRightWheelRotation = 0;
	float frontLeftWheelRotation = 0;
	float backRightWheelRotation = 0;
	float backLeftWheelRotation = 0;

	Vector3 firstPosition;

	// animation timer text attributes
	bool isAnimationTimerText = false;
	float totalAnimationTime = 1f;
	float deltaScale = 1.8f;
	float elapsedAnimationTime = 0f;
	Vector3 initScale;

	// slip attribute
	Vector3 wheelPrevPos;

	/* MAIN FUNCTIONS */
	// Use this for initialization
	void Start () {
		wheelPrevPos = colliderWheelRR.transform.position;

		rigidbody.centerOfMass += new Vector3 (0f, -1.1f, 0.6f);
		
		colliderWheelList = new WheelCollider[]{colliderWheelFL, colliderWheelFR, colliderWheelRL, colliderWheelRR};

		// assign text GUI
		speedometerText = GameObject.Find ("speedometer_text").GetComponent<GUIText> ();
		distanceText = GameObject.Find ("distance_text").GetComponent<GUIText> ();
		timerLeftText = GameObject.Find ("timer_text_left").GetComponent<GUIText> ();
		timerRightText = GameObject.Find ("timer_text_right").GetComponent<GUIText> ();
		lapText = GameObject.Find ("lap_text").GetComponent<GUIText> ();

		// init
		firstPosition = transform.position;
		totalTime = initialTime;

		SetVisualTimer ();
	}
	
	// Update is called once per frame
	void Update() {
		float deltaTime = Time.deltaTime;

		if (!GameController.isFreeze) {
			power = Input.GetAxis ("Vertical") * enginePower * deltaTime * 250;
			steer = Input.GetAxis ("Horizontal") * maxSteer;
			
			// PHYSICS
			// set steering
			colliderWheelFL.steerAngle = steer;
			colliderWheelFR.steerAngle = steer;
			
			// brake handler
			if (Input.GetKey (KeyCode.Space)) { // brake
				Brake ();
			} else { // move
				Move ();
			}

			// distance
			CalculateDistance ();
			
			// timer
			CalculateTimer ();				
		}

		// tween timer text
		if (isAnimationTimerText) {
			AnimateTimerText (deltaTime);		
		}

		// VISUAL
		// velocity
		// 7.7 unity unit = 4.7 meter, 1 unity unit = 0.61 meter, 1 meters/sec = 3.6 km/hour
		int velocity = (int)(rigidbody.velocity.magnitude * 0.61 * 3.6); // kph
		speedometerText.text = velocity.ToString();

		PlayEngineSound (velocity);

		// rotation of the wheels
		frontLeftWheelRotation = Mathf.Repeat(frontLeftWheelRotation + deltaTime * colliderWheelFL.rpm * 360f / 60f, 360f);
		frontRightWheelRotation = Mathf.Repeat(frontRightWheelRotation + deltaTime * colliderWheelFR.rpm * 360f / 60f, 360f);
		backLeftWheelRotation = Mathf.Repeat(backLeftWheelRotation + deltaTime * colliderWheelRL.rpm * 360f / 60f, 360f);
		backRightWheelRotation = Mathf.Repeat(backRightWheelRotation + deltaTime * colliderWheelRR.rpm * 360f / 60f, 360f);
		
		// set the rotation of the wheels
		wheelFL.gameObject.transform.localRotation = Quaternion.Euler(frontLeftWheelRotation, steer, 0f);
		wheelFR.gameObject.transform.localRotation = Quaternion.Euler(frontRightWheelRotation, steer, 0f);
		wheelRL.gameObject.transform.localRotation = Quaternion.Euler(backRightWheelRotation, 0f, 0f);
		wheelRR.gameObject.transform.localRotation = Quaternion.Euler(backLeftWheelRotation, 0f, 0f);

		CalculateSlip ();
	}

	void Brake() {
		float brake = rigidbody.mass * 0.3f;
		
		// set torque
		SetAllBrakeTorque(brake);
		
		// set power
		colliderWheelRL.motorTorque = 0;
		colliderWheelRR.motorTorque = 0;
	}

	void Move() {
		// set torque
		SetAllBrakeTorque(0f);
		
		// set power
		colliderWheelRL.motorTorque = power;
		colliderWheelRR.motorTorque = power;
	}

	void CalculateDistance() {
		float deltaDistance = Vector2.Distance (new Vector2 (firstPosition.x, firstPosition.z), new Vector2 (transform.position.x, transform.position.z)) * 0.61f; // meter
		firstPosition = transform.position;

		totalDistance += deltaDistance;

		distanceText.text = (totalDistance / 1000f).ToString ("0.00");
	}

	void CalculateTimer() {
		totalTime -= Time.deltaTime;

		if (totalTime <= 0) {
			totalTime = 0f;
			EndGame ();
		}

		SetVisualTimer ();
	}

	void SetVisualTimer() {
		int second = (Mathf.FloorToInt (totalTime));
		timerLeftText.text = second.ToString ();
		// decimal
		timerRightText.text = "." + Mathf.FloorToInt(((totalTime - second) * 1000));
		
		// change color
		if (totalTime < 10f) {
			timerLeftText.color = Color.red;
			timerRightText.color = Color.red;
		} else {
			timerLeftText.color = Color.white;
			timerRightText.color = Color.white;
		}
	}

	void SetAllBrakeTorque(float value) {
		for (int i = 0; i < colliderWheelList.Length; i++) {
			colliderWheelList[i].brakeTorque = value;		
		}
	}

	void EndGame() {
		GameController.isFreeze = true;
		// init car physic
		power = 0;
		steer = 0;
		Brake ();

		audio.PlayOneShot (bombSound);

		// change scene
		StartCoroutine("LoadNextLevel");
	}

	void PlayEngineSound(float velocity) {
		audio.volume = velocity / 200f;
	}

	IEnumerator LoadNextLevel(){
		yield return new WaitForSeconds(4);
		Application.LoadLevel("MenuScene");
	}

	void setAnimationTimerText() {
		isAnimationTimerText = true;
		initScale = timerLeftText.transform.localScale;
		elapsedAnimationTime = 0;
	}

	void AnimateTimerText(float deltaTime) {
		elapsedAnimationTime += deltaTime;

		// totalAnimationTime = 1; endScale = 1.5f
		if (elapsedAnimationTime >= totalAnimationTime) {
			// init
			elapsedAnimationTime = 0;
			isAnimationTimerText = false;

			timerLeftText.transform.localScale = initScale;
		} else { // animate using parabolic calculation
			float x = elapsedAnimationTime;
			float a = -(((2*deltaScale) / Mathf.Pow(totalAnimationTime,2)) + 0.5f);
			float b = -a * totalAnimationTime;
			timerLeftText.transform.localScale = ((a * Mathf.Pow(x,2) + (b * x)) + 1) * initScale;
		}
	}

	void CalculateSlip() {
		Vector3 velocity = (colliderWheelRR.transform.position - wheelPrevPos) / Time.deltaTime;
		wheelPrevPos = colliderWheelRR.transform.position;
		
		Vector3 forward = colliderWheelRR.transform.forward;
		Vector3 sideways = -colliderWheelRR.transform.right;
		
		Vector3 forwardVelocity = Vector3.Dot(velocity, forward) * forward;
		Vector3 sidewaysVelocity = Vector3.Dot(velocity, sideways) * sideways;
		
		float forwardSlip = -Mathf.Sign(Vector3.Dot(forward, forwardVelocity)) * forwardVelocity.magnitude + (Mathf.PI / 180.0f);
		float sidewaysSlip = -Mathf.Sign(Vector3.Dot(sideways, sidewaysVelocity)) * sidewaysVelocity.magnitude;

		if (Mathf.Abs (sidewaysSlip) > 8f) {
			audio.PlayOneShot(skidSound);
		}
	}

	void Finish() {
		// update lap
		lap += 1;

		lapText.text = "Lap " + lap;

		if (lap > 1) {
			totalTime += 15f * (1f / Mathf.Sqrt(lap));
			// animate timerText
			setAnimationTimerText();
			// play sound
			Camera.main.gameObject.SendMessage("PlayFinish");
		}
	}
}
