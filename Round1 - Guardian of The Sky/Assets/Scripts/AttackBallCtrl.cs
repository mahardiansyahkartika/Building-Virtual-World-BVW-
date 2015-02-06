using UnityEngine;
using System.Collections;

public class AttackBallCtrl : MonoBehaviour {
	public float speed = 7;
	public float lifeTime = 5;
	public float rotateSpeed = 1;
	public int	 type = 0;			//the type of the attack ball:0 is ice ball, 1 is fire ball

	private Transform chaseTarget;
	private Vector3 moveDirection;
	private Transform _transform;
	private Rigidbody _rigidbody;

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindWithTag ("Player");
		chaseTarget = player.transform;
		_transform = transform;
		_rigidbody = rigidbody;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;
		if(lifeTime<0){
			Destroy(gameObject);
		}
	}

	void FixedUpdate(){
		Vector3 targetDir = chaseTarget.position - _transform.position;
		Vector3 newDir = Vector3.RotateTowards (_transform.forward, targetDir,rotateSpeed*Time.deltaTime, 0);
		_transform.rotation = Quaternion.LookRotation (newDir);
		moveDirection = _transform.forward * speed;
		_rigidbody.MovePosition (_rigidbody.position + moveDirection*Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			other.SendMessage("AttackHit", type);
			SoundFXCtrl.instance.PlaySound(1,1);
			Destroy(gameObject);
		}
	}

}
