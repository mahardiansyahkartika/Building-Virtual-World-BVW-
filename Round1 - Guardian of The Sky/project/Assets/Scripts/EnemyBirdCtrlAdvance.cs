using UnityEngine;
using System.Collections;

//the controller for ice bird and fire bird
public class EnemyBirdCtrlAdvance : MonoBehaviour {
	
	public float forwardSpeed = 12;		//the maximum speed moving forward(when right behind the bird group)
	public GameObject characterA;
	public Transform player;
	public float rotateSpeed = 2;
	public float attackRange = 35;		//the range with in which will attack player
	public Transform Projectile;		//the prefab of ice/fire ball
	public Transform ShotPos;			//the position where to generate projectile

	private Transform _transform;
	private Rigidbody _rigidbody;
	private Vector3 moveDirection = Vector3.zero;
	private float normalForwardSpeed;
	private bool canAttack = true;
	private bool attackFlag = true;

	private GameObject chaseTarget;
	private bool isCatchVictim = false;
	private bool isGetVictim = false;
	private int indexTargetBird;
	private BirdsController characterAScript;
	private bool oneChance = true;
	private bool isDead = false;
	private Transform water;
	private bool inwater = false;

	// Use this for initialization
	void Start () {
		characterA = GameObject.FindWithTag ("characterA");
		GameObject ply = GameObject.FindWithTag ("Player");
		player = ply.transform;
		_transform = transform;
		_rigidbody = rigidbody;
		normalForwardSpeed = forwardSpeed;
		chaseTarget = characterA;
		characterAScript = characterA.GetComponent<BirdsController> ();

		water = GameObject.FindWithTag ("water").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (_transform.position.y < water.position.y&&!inwater){
			inwater = true;
			SoundFXCtrl.instance.PlaySound(0,0.6f);
		}
	}
	
	void FixedUpdate(){
		if (!isGetVictim && chaseTarget != null) {
			if(Vector3.Distance (_transform.position, player.position)>attackRange
			   ||canAttack == false){
				Vector3 deltaPos = chaseTarget.transform.position - _transform.position;
				float deltaSpeed = Vector3.Dot (deltaPos.normalized, chaseTarget.transform.forward);
				if(deltaSpeed<0){
					forwardSpeed = normalForwardSpeed / 3;
				}else{
					forwardSpeed = Mathf.Lerp(normalForwardSpeed/3, normalForwardSpeed, deltaSpeed);
				}

				if(Vector3.Distance(_transform.position, characterA.transform.position)>55){
					forwardSpeed *= 1.5f;
				}

				//rotation toward character a
				Vector3 deltaRotate = Vector3.RotateTowards (_transform.forward, chaseTarget.transform.position - _transform.position, rotateSpeed * Time.deltaTime, 0);
				_transform.rotation = Quaternion.LookRotation (deltaRotate);
				
				moveDirection = _transform.forward;
				moveDirection *= forwardSpeed;
				_rigidbody.MovePosition (rigidbody.position + moveDirection * Time.deltaTime);

				// got the victim
				if (isCatchVictim && deltaPos.magnitude < 0.7f) {
					isCatchVictim = false;
					isGetVictim = true;
					characterA.SendMessage("Hit", chaseTarget);
					chaseTarget.transform.parent = this.transform;
					
					Destroy(this.gameObject.collider);
					Destroy(this.gameObject, 5f);
				}
			}else{
				if(attackFlag){
					StartCoroutine(StartAttack());
				}
				//rotation toward player
				Vector3 deltaRotate = Vector3.RotateTowards (_transform.forward, player.position - _transform.position, rotateSpeed * Time.deltaTime, 0);
				_transform.rotation = Quaternion.LookRotation (deltaRotate);
			}				
		} else if (!isDead) {
			_transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.Euler(270, _transform.rotation.y, _transform.rotation.z), Time.deltaTime * 0.3f);
			_rigidbody.MovePosition (rigidbody.position + _transform.forward * 40f * Time.deltaTime);
			
			if (oneChance && chaseTarget == null) {
				oneChance = false;
				Destroy(this.gameObject, 5f);
			}
		}
	}

	IEnumerator StartAttack(){
		attackFlag = false;
		yield return new WaitForSeconds (1);
		canAttack = false;
		IniAttack ();
		yield return new WaitForSeconds (5); 
		canAttack = true;
		attackFlag = true;
	}

	void IniAttack(){
		Instantiate (Projectile, ShotPos.position, _transform.rotation);
	}

	void OnCollisionEnter(Collision collision){
		if(collision.transform.tag == "Player"){
			//print (collision.relativeVelocity);
			//print (collision.contacts[0].normal);
			//collision.rigidbody.AddForce(-collision.contacts[0].normal*20, ForceMode.VelocityChange);
			SoundFXCtrl.instance.PlaySound(1,1);
			SoundFXCtrl.instance.PlaySound(Random.Range(3,7),1);
			Dead(collision.transform.forward);
		}
	}
	
	void Dead(Vector3 direction) {
		isDead = true;
		
		Destroy (this.gameObject.collider);
		this.gameObject.rigidbody.useGravity = true;
		this.gameObject.rigidbody.AddForce (direction * 10);
		this.gameObject.rigidbody.AddTorque (0, 50, 0);
		_transform.parent = null;
		Destroy (gameObject, 7);
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.transform.name == "BirdGroup") {
			if (!isCatchVictim && characterAScript.birdList.Count > 0) {
				isCatchVictim = true;
				
				// choose one bird
				indexTargetBird = Random.Range(0, characterAScript.birdList.Count-1);
				chaseTarget = characterAScript.birdList[indexTargetBird].gameObject;
			}
		}
	}	

	/*
	public void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
	*/
}
