using UnityEngine;
using System.Collections;

public class BirdCtrl : MonoBehaviour {

	public bool simulateInput = true;			//Simulate the input with keyboard
	public Transform OVRCamera;					//Left/Right camera of OVR
	public float horiRotateLimit = 1f; 			//the minimun rotation angle to apply PSmove movement
	public float vertRotateLimit = 0.1f;		//the minimun rotation angle to apply oculus movement
	public float horiRotateSpeed = 30f;			//Horizental Rotatte Speed
	public float vertRotateSpeed = 40f;			//Vertical Rotate Speed
	public float minForwardSpeed = 3f;			//minimun forward speed
	public float turningSpeed = 20;
	public float accForce = 7f;					//the accelerate force when wave down
	public Transform leftWing;
	public Transform rightWing;
	public Transform arrow;
	public float distanceForArrow = 50;			//the distance to target when arrow will display
	public Transform BirdsGroup;
	public float iceStateDuration = 5;			//the duration of ice state
	public Material feather;
	public Material skeleton;
	public Material body;
	public Color featherFire;
	public Color skeletonFire;
	public Color bodyFire;
	public Color featherIce;
	public Color skeletonIce;
	public Color bodyIce;
	public Color featherNormal;
	public Color skeletonNormal;
	public Color bodyNormal;
	public GameObject flameEffect;
	public Transform enemyContainer;
	public Transform fireSE;
	public Transform windSE;

	private PSMoveCtrl psmoveCtrl;
	private Transform _transform;
	private Rigidbody _rigidbody;
	private Vector3 velocity;
	private Animator leftAnimation;
	private Animator rightAnimation;
	public float iceRecoverTime = 0;
	public bool inFireState = false;
	private float speedAdjustPara1 = 1;			//for speed and force
	private float speedAdjustPara2 = 1;			//for rotation


	// Use this for initialization
	void Start () {
		psmoveCtrl = GetComponent<PSMoveCtrl>();
		_transform = transform;
		_rigidbody = rigidbody;
		leftAnimation = leftWing.GetComponent<Animator> ();
		rightAnimation = rightWing.GetComponent<Animator> ();
		feather.color = featherNormal;
		skeleton.color = skeletonNormal;
		body.color = bodyNormal;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	void Update(){
		windSE.audio.volume = Mathf.Clamp (_rigidbody.velocity.magnitude - 10, 0, 10) / 10;

		//if the bird is in ice
		if(iceRecoverTime <= Time.time){
			speedAdjustPara1 = 1;
			speedAdjustPara2 = 1;
			_rigidbody.drag = 0.7f;
			if(psmoveCtrl.doubleTriggerDown){
				_rigidbody.drag = 1.2f;
			}
		}else if(iceRecoverTime > Time.time){
			speedAdjustPara1 = 0.8f;
			speedAdjustPara2 = 0.5f;
			_rigidbody.drag = 1.2f;
		}
		//if the bird is on fire
		if(inFireState == true){
			flameEffect.SetActive(true);
			_rigidbody.useGravity = true;
		}else if(inFireState == false){
			flameEffect.SetActive(false);
			_rigidbody.useGravity = false;
		}
		//change the color of the wing
		if(inFireState==true){
			feather.color = Color.Lerp(feather.color, featherFire, Time.deltaTime*5);
			skeleton.color = Color.Lerp(skeleton.color, skeletonFire, Time.deltaTime*5);
			body.color = Color.Lerp(body.color, bodyFire, Time.deltaTime*5);
		}else if(iceRecoverTime > Time.time){
			feather.color = Color.Lerp(feather.color, featherIce, Time.deltaTime*5);
			skeleton.color = Color.Lerp(skeleton.color, skeletonIce, Time.deltaTime*5);
			body.color = Color.Lerp(body.color, bodyIce, Time.deltaTime*5);
		}else{
			feather.color = Color.Lerp(feather.color, featherNormal, Time.deltaTime*5);
			skeleton.color = Color.Lerp(feather.color, skeletonNormal, Time.deltaTime*5);
			body.color = Color.Lerp(body.color, bodyNormal, Time.deltaTime*5);
		}

		//move the object forward with minimun speed
		velocity = _transform.forward * minForwardSpeed * speedAdjustPara1;
		_transform.position += velocity * Time.fixedDeltaTime;

		//simulating input
		if(simulateInput){
			float h = Input.GetAxis("Horizontal");
			HorizentalRotation(Mathf.Clamp(h*90, -30, 30));
			float v = Input.GetAxis("Vertical");
			VerticalRotation(v*90);
			if(Input.GetButtonDown("Jump")){
				WaveDownNoTrigger();
			}
			if(Input.GetButton("Fire1")){
				AutoTracking();
			}
		}else{

			//apply horizental rotation by psmove
			if(Mathf.Abs(psmoveCtrl.rotation)>horiRotateLimit){
				//print (psmoveCtrl.rotation);
				HorizentalRotation(Mathf.Clamp(psmoveCtrl.rotation, -30, 30));
			}
			//apply vertical rotation by oculus
			Vector3 cameraDelta = _transform.InverseTransformDirection(OVRCamera.forward);
			//print ("cameraDelta:"+cameraDelta);

			if(Mathf.Abs(cameraDelta.y)>vertRotateLimit){
				VerticalRotation(Mathf.Clamp(-cameraDelta.y*90, -90, 90));
			}

			if(psmoveCtrl.doubleTriggerDown){
				AutoTracking();
			}
		}

		//control the display of direction arrow
		Vector3 deltaBirdCamera = BirdsGroup.position - OVRCamera.position;
		if(Vector3.Dot(deltaBirdCamera, OVRCamera.forward)<0 || Vector3.Distance(_transform.position, BirdsGroup.position)>distanceForArrow){
			arrow.gameObject.SetActive(true);
		}else{
			arrow.gameObject.SetActive(false);
		}
	}

	void AutoTracking(){
		//auto tracking the nearest enemy when double trigger down
		if(enemyContainer.childCount>0){
			Transform temp = enemyContainer.GetChild(0);
			foreach(Transform child in enemyContainer){
				if(Vector3.Distance(child.position, _transform.position)<Vector3.Distance(temp.position, _transform.position)){
					temp = child;
				}
			}
			Vector3 tempDir = Vector3.RotateTowards(_transform.forward, (temp.position-temp.up) - _transform.position, Time.deltaTime, 0);
			_transform.rotation = Quaternion.LookRotation(tempDir, _transform.up);
		}else{
			Vector3 tempDir = Vector3.RotateTowards(_transform.forward, BirdsGroup.position - _transform.position, Time.deltaTime, 0);
			_transform.rotation = Quaternion.LookRotation(tempDir, _transform.up);
		}
	}

	//RotateVertical, angle should restricted to -90 ~ 90
	void VerticalRotation(float angle){
		_transform.Rotate(Vector3.right*vertRotateSpeed*(angle/90)*Time.deltaTime * speedAdjustPara2);


		if(_transform.localEulerAngles.x>85&&_transform.localEulerAngles.x<90){
			_transform.localEulerAngles = new Vector3(85, _transform.localEulerAngles.y, _transform.localEulerAngles.z);
		}else if(_transform.localEulerAngles.x>270&&_transform.localEulerAngles.x<275){
			_transform.localEulerAngles = new Vector3(275, _transform.localEulerAngles.y, _transform.localEulerAngles.z);
		}

	}

	//Rotate Horizental, angle should restricted to -30 ~ 30
	void HorizentalRotation(float angle){
		/*
		angle = (angle / 30) * 80;

		if(angle>0){
			if(_transform.localEulerAngles.z>270||_transform.localEulerAngles.z<angle){
				_transform.Rotate(Vector3.forward * horiRotateSpeed * Time.deltaTime);
			}else if(angle>_transform.localEulerAngles.z){
				_transform.Rotate(Vector3.forward * -horiRotateSpeed * Time.deltaTime);
			}
		}else{
			if(_transform.localEulerAngles.z>(360+angle)||_transform.localEulerAngles.z<90){
				transform.Rotate(Vector3.forward * -horiRotateSpeed * Time.deltaTime);
			}else if(_transform.localEulerAngles.z<(360+angle)){
				_transform.Rotate(Vector3.forward * horiRotateSpeed * Time.deltaTime);
			}
		}*/

		/*
		if(angle > 0 && (_transform.localEulerAngles.z<angle||_transform.localEulerAngles.z>270)){
			_transform.Rotate(Vector3.forward * horiRotateSpeed * Time.deltaTime);
		}else if(angle < 0 && (_transform.localEulerAngles.z>(360+angle)||_transform.localEulerAngles.z<90)){
			_transform.Rotate(Vector3.forward * -horiRotateSpeed * Time.deltaTime);
		}
		*/

		//print (_transform.localEulerAngles.z + ", "+ angle);
		_transform.Rotate(Vector3.forward * horiRotateSpeed * (angle/30) * Time.deltaTime * speedAdjustPara2);

		/*
		//need change, based on the angle between local/world, when velocity bigger then a value
		float angleDelta = Vector3.Angle (_transform.up, Vector3.up);
		if(angleDelta>20&&_rigidbody.velocity.magnitude>5){
			_transform.Rotate(Vector3.up*-turningSpeed*Time.deltaTime*(angleDelta/80)*Mathf.Sign(Vector3.Dot(_transform.up, Vector3.Cross(_transform.forward, Vector3.up))), Space.World);
		}
		*/


		if(_transform.localEulerAngles.z>80&&_transform.localEulerAngles.z<85){
			_transform.localEulerAngles = new Vector3(_transform.localEulerAngles.x, _transform.localEulerAngles.y, 80);
		}else if(_transform.localEulerAngles.z>275&&_transform.localEulerAngles.z<280){
			_transform.localEulerAngles = new Vector3(_transform.localEulerAngles.x, _transform.localEulerAngles.y, 280);
		}

	}

	//give the force to push the bird forward
	public void WaveDownNoTrigger(){
		leftAnimation.SetTrigger("wave");
		rightAnimation.SetTrigger ("wave");
		_rigidbody.AddForce (transform.forward * accForce * speedAdjustPara1, ForceMode.VelocityChange);
	}

	//when the player is hit by attack, type: 0 - ice, 1 - fire
	void AttackHit(int type){
		print(type);
		switch(type){
		case 0:
			//if player already in fire, restore at once
			if(inFireState){
				inFireState = false;
				fireSE.audio.Stop();
			}else{
				iceRecoverTime = Time.time + iceStateDuration;
			}
			break;
		case 1:
			//if player already in ice, restore at once
			if(iceRecoverTime>Time.time){
				iceRecoverTime = Time.time;
			}else{
				inFireState = true;
				if(!fireSE.audio.isPlaying)
					fireSE.audio.Play();
			}
			break;
		}
	}

	void RestoreFire(){
		inFireState = false;
		fireSE.audio.Stop();
	}

	public void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, distanceForArrow);
	}

}
