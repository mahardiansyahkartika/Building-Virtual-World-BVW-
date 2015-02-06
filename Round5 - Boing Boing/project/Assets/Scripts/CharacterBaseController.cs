using UnityEngine;
using System.Collections;

public class CharacterBaseController : MonoBehaviour {

	// Character Selection
	public bool isCharacterSelection = false;
	protected float a = 0.3f;
	protected float velocity0 = 0f;
	protected float elapsedTimeSelection = 0f;

	// AI
	protected bool isComputer = false;
	public int index; //player's number

	// floating algorithm
	protected Vector3 firstPosition;
	protected float floatingSpeed = 6f;
	protected float maxHeight = 0.75f;
	
	protected float rotationSpeed = 3f;
	protected float forwardSpeed = 80f;

	// parabolic
	protected float g = 15f;
	protected bool isParabolicAnimating = false;
	protected float elapsedTimeParabolic = 0f;
	protected float v0default = 6f;
	protected float v0;

	// forward
	protected float waitingTimeToMove = 0.2f;
	protected float elapsedTimeForward = 0f;

	bool fallDown;
	float respawnTime;
	const float MAX_RESPAWN_TIME = 1.5f;
	Vector3 capsuleCollRadius;
	const float DEATH_DEPTH = -25f; // safe net in case somehow the game does not respawn the character properly

	protected bool isFreezeMovement;

	bool isFlying;
	float flyTime;
	const float MAX_FLY_TIME = 10f;
	TextMesh flyTimeText;

	GameController gameController;
	SoundController soundController;
	TileController tileController;
	PlayerAttack playerAttack;

	float yTime;
	bool hasTouchedTile; // true if the player has hit the tile at least one time

	//Bomb
	public bool hasBomb = false;
	public GameObject bomb;
	Vector3 bombOffset = new Vector3 (0f, 1.5f, 0f);
	public GameObject bombFrom;

	private int bombTime = 5;
	private GameObject bombinst;
	private float bombPassInterve = 2f;
	private float bombGetTimeRecord= 0f;

	public GameObject explosion;
	private GameObject sf;

	public Transform[] characterModels;

	GameObject wing;

	protected virtual void Awake() {
		if (index != 0 && !GameController.activePlayers[index - 1] && !isCharacterSelection) {
			AIController aicomp = gameObject.AddComponent<AIController>();
			PlayerController playercomp = gameObject.GetComponent<PlayerController>();

			foreach(Transform tf in gameObject.transform) {
				if(tf.name == "computer") {
					tf.gameObject.SetActive(true);
				}
			}
			
			aicomp.index = playercomp.index;
			aicomp.bomb = playercomp.bomb;
			aicomp.explosion = playercomp.explosion;
			aicomp.characterModels = playercomp.characterModels;
			
			Destroy(playercomp);
		}

		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		capsuleCollRadius = new Vector3(GetComponent<CapsuleCollider>().bounds.extents.x, 0, 0);
		playerAttack = gameObject.GetComponent<PlayerAttack>();
		soundController = GameObject.Find("GameController").GetComponent<SoundController>();

		if (!isCharacterSelection) {
			tileController = gameController.gameObject.GetComponent<TileController>();
		}

		flyTimeText = transform.Find("FlyTimeText").GetComponent<TextMesh>();
		flyTimeText.gameObject.SetActive(false);

		wing = transform.Find("wing-move").gameObject;
		wing.SetActive(false);
	}
	
	// Use this for initialization
	protected virtual void Start () {
		firstPosition = transform.position;

		Jump ();
		InitCharacterModel();
	}

	void InitCharacterModel()
	{
		for(int i=0; i<characterModels.Length; i++)
		{
			characterModels[i].gameObject.SetActive(i == index-1);
		}
	}

	// Update is called once per frame
	protected virtual void Update () {
		// idle floating
		if((!fallDown && hasTouchedTile) || isCharacterSelection)
			Floating ();

		if(isFreezeMovement)
			return;

		if(fallDown)
		{
			respawnTime -= Time.deltaTime;

			if(respawnTime <= 0)
			{
				gameController.SpawnPlayer(index);
			}
		}

		if(isFlying)
		{
			flyTime -= Time.deltaTime;
			if(flyTime <= 0)
			{
				flyTime = 0;
				StopFlying();
			}
			flyTimeText.text = "" + flyTime;
		}

		//safe net just in case somehow the game does not respawn the character even if the player is falling deep
		if(!fallDown && transform.position.y <= DEATH_DEPTH)
		{
			PlayerFalls();
		}
	}

	protected virtual void FixedUpdate() {

	}
	
	protected void Floating() {
		//transform.position = new Vector3 (transform.position.x, firstPosition.y + (((Mathf.Sin (Time.time * floatingSpeed) + 1) / 2f) * maxHeight), transform.position.z);
		//transform.position = new Vector3 (transform.position.x, firstPosition.y + (Mathf.Abs (Mathf.Sin (Time.time * floatingSpeed)) * maxHeight), transform.position.z);

		/*
		if (isCharacterSelection) {
			elapsedTimeSelection += Time.deltaTime;
			velocity0 -= a * elapsedTimeSelection;

			if (velocity0 <= 0f) {
				velocity0 = 0f;
				elapsedTimeSelection = 0f;
			}
		}
		*/

		if (isParabolicAnimating) {
			elapsedTimeParabolic += Time.deltaTime;
			//float y = (v0 * elapsedTimeParabolic) - (0.5f * g * Mathf.Pow(elapsedTimeParabolic, 2));

			Vector2 v = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);
			float vMagnitude;

			if (isCharacterSelection) {
				//vMagnitude = velocity0;
				vMagnitude = v.sqrMagnitude;
			} else {
				vMagnitude = v.sqrMagnitude;
			}

			//yTime += Time.deltaTime * Mathf.Clamp(v.sqrMagnitude, 6, 10);
			yTime += Time.deltaTime * GetValue(new Vector2(7,17), new Vector2(0,50), vMagnitude);
//			if (index == 1) {
//				Debug.Log("velocity : " + v.sqrMagnitude);
//			}

			if (yTime >= Mathf.PI) {
				yTime -= Mathf.PI;

				isParabolicAnimating = false;
				 
				if (!isCharacterSelection)
				{
					CheckGroundBelow();
				}

				// next jump
				NextJump();

				//play toet sound
				soundController.PlaySound("SFX-Jump", 0.3f, false);
			}
			float y = Mathf.Abs(Mathf.Sin(yTime) * maxHeight);

			transform.position = new Vector3 (transform.position.x, firstPosition.y + y, transform.position.z);
		}
	}

	void CheckGroundBelow()
	{
		int layerMask = (1 << LayerMask.NameToLayer("Tile"));
		RaycastHit hitInfo;

		//only check tile below if we are not flying, check flying is intentionally on the rightso that we got the hit info to shake the tile
		if(!Physics.Raycast(transform.position, - transform.up, out hitInfo, 2f, layerMask) && !isFlying)
		{
			if(!Physics.Raycast(transform.position - capsuleCollRadius, - transform.up, 2f, layerMask) && !Physics.Raycast(transform.position + capsuleCollRadius, - transform.up, 2f, layerMask))
			{
				// there is no tile below, so player falls down
				PlayerFalls();
			}
		}
		else if(hitInfo.collider != null)
		{
			hitInfo.collider.gameObject.GetComponent<TileMovement>().ShakeTile(TileMovement.ShakeType.Down);
		}
    }
    
	void PlayerFalls()
	{
		fallDown = true;
		respawnTime = MAX_RESPAWN_TIME;
		rigidbody.detectCollisions = false;
		playerAttack.Killed();
	}

	protected void NextJump () {
		float speed = Vector2.SqrMagnitude (new Vector2 (rigidbody.velocity.x, rigidbody.velocity.z));
		Vector2 speedRange = new Vector2 (v0default, v0default / 2f);
		float v0 = CharacterBaseController.GetValue (speedRange, new Vector2(0f, 50f), speed);
		if (v0 < speedRange.y) {
			v0 = speedRange.y;
		} else if (v0 > speedRange.x) {
			v0 = speedRange.x;
		}
		
		Jump (v0);
	}

	protected void Jump() {
		Jump (v0default);
	}

	protected void Jump(float v0) {
		this.v0 = v0;
		isParabolicAnimating = true;
		elapsedTimeParabolic = 0f;
	}

	protected void MoveForward() {
		if (isCharacterSelection) {
			//velocity0 += 5f;
			rigidbody.AddForce (transform.forward * forwardSpeed);
		} else {
			if(hasTouchedTile)
				rigidbody.AddForce (transform.forward * forwardSpeed);
			//if (transform.position.y <= 1.1f) {
			//	rigidbody.AddForce (transform.up * 5.5f * forwardSpeed);
			//}
		}
	}

	public static float GetValue(Vector2 firstVector, Vector2 secondVector, float secondValue) {
		return -((firstVector.y - firstVector.x) * (secondVector.y - secondValue) / (secondVector.y - secondVector.x)) + firstVector.y;
	}

	public void Reset()
	{
		fallDown = false;
		hasTouchedTile = false;
		hasBomb = false;
		Destroy (this.bombinst);
		if(sf){
			sf.GetComponent<AudioSource> ().Stop ();
		}
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.detectCollisions = true;
	}

	public int GetIndex()
	{
		return index;
	}

	public void SetFreezeMovement(bool value)
	{
		rigidbody.isKinematic = value;
		isFreezeMovement = value;
	}

	public void ActivateFlying()
	{
		StartFlying();
		flyTime = MAX_FLY_TIME;
	}

	void StartFlying()
	{
		isFlying = true;
		//flyTimeText.gameObject.SetActive(true);
		wing.SetActive(true);
	}

	void StopFlying()
	{
		isFlying = false;
		//flyTimeText.gameObject.SetActive(false);
		wing.SetActive(false);
	}

	public bool IsFlying()
	{
		return isFlying;
	}

	//bomb controller

	void PassBomb(int bombT ,GameObject passedBy, Vector3 scale) {
		bombTime = bombT;
		bombinst = Instantiate (bomb,transform.position + bombOffset, Quaternion.identity) as GameObject;
		if(bombT <= 2) {
			bombinst.transform.localScale = scale - new Vector3(100,100,100);
			BombRed();
		}
		bombinst.transform.parent = gameObject.transform;
		this.bombFrom = passedBy;
		StartCoroutine(CountDown(bombT));
	}

	GameObject Explode() {
		Vector3 explodeForce = new Vector3 (1, 9, 1);
		this.gameObject.rigidbody.AddForce (explodeForce * 900);
		GameObject explosioninst = Instantiate (explosion, gameObject.transform.position, Quaternion.identity) as GameObject;
		soundController.PlaySound ("explode");
		soundController.PlaySound("falling", 0.7f);
		Destroy (explosioninst, 3f);

		Vector2 tilePos = tileController.GetTilePos(transform.position);
		//tileController.ExplodeTiles(tilePos);

		if(bombFrom != null)
		{
			playerAttack.SetLastHitFrom(bombFrom);
		}

		//print (bombFrom.name);
		return bombFrom;
	}

	public void AttachBomb(GameObject go) {
		this.hasBomb = true;
		if (bombinst) {
			PassBomb (bombTime, go, this.bombinst.transform.localScale);
		} else {
			PassBomb (bombTime, go, new Vector3(0,0,0));
		}
	}

	IEnumerator CountDown(int bombT) {
		sf = soundController.PlaySound("clock_long");
		while(true) {
			if(bombT > 0 && hasBomb){
				yield return new WaitForSeconds (1);
				//print( bombT+ " remaining " + name);
				bombT--;
				bombTime--;
				if(bombT <= 2) {
					BombRed();
					if(this.bombinst) {
						this.bombinst.gameObject.transform.localScale += new Vector3(100,100,100);
					}
				}
			}else {
				if(hasBomb){
					Explode();
					//Destroy(sf);
					if(sf) {
						sf.GetComponent<AudioSource>().Stop();
					}
					this.hasBomb = false;
				}
				Destroy(bombinst);
				bombTime = 5;
				break;
			}
		}
	}

	void OnTriggerEnter(Collider c) { // Trigger Enter of Collision Enter
		if(c.gameObject.tag == "Player" && hasBomb && !c.gameObject.GetComponent<CharacterBaseController>().hasBomb) {
			if(bombPassInterve + bombGetTimeRecord < Time.time) {
				bombGetTimeRecord = Time.time;
				c.gameObject.GetComponent<CharacterBaseController>().bombGetTimeRecord = Time.time;
				this.hasBomb = false;
				sf.GetComponent<AudioSource>().Stop();
				soundController.PlaySound("passbomb", 0.5f, false);
				c.gameObject.GetComponent<CharacterBaseController>().hasBomb = true;
				c.gameObject.GetComponent<CharacterBaseController>().PassBomb(bombTime, gameObject, bombinst.transform.localScale);
				Destroy(this.bombinst);
			}else {
				return;
			}
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Tile")
		{
			hasTouchedTile = true;
		}
	}

	public void RainbowTrail() {
		gameObject.GetComponentInChildren<TrailRenderer> ().material.mainTexture = Resources.Load("Texture/rainbow") as Texture; 
	}

	public bool HasTouchedTile()
	{
		return hasTouchedTile;
	}

	public void SetHasTouchedTile(bool value)
	{
		hasTouchedTile = value;
	}

	void BombRed() {
				if (bombinst) {
						foreach (Transform ft in bombinst.transform) {
								if (!ft.gameObject.renderer) {
										foreach (Transform tf in ft.transform) {
												if (tf.gameObject.renderer) {
														tf.gameObject.renderer.material.mainTexture = (Resources.Load ("Texture/red")) as Texture;
												}
										}
								}
						}
				}
		}
}
