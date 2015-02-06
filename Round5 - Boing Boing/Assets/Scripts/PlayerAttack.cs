using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {
	//static
	public static GameObject firstKillPlayer; // the player who kills first in the game

	const float MAX_FIST_TIME = 10f;
	private float fistTime = 0f;

	public GameObject punchEffect;
	public GameObject missEffect;
	private Transform cd;

	bool isMultipleFistExist = false;
	bool hasHighScore = false;
	bool isInKS = false;

	bool iscd = false;
	GameObject fist;
	GameObject HighScorePlayer;
	Texture tRecord;
	public GameObject[] additionalFist;
	float forceMagnitude = 10f;
	int index;
	int killCount;
	int deathCount;
	int ksCount;

	PlayerController playerController;
	GameController gameController;
	SoundController soundController;
	CharacterBaseController characterbaseController;

	//killing system
	GameObject lastHitFrom; // record who lands the last hit on this player, will reset back to null if lastHitExpireTime hits zero
	float lastHitExpireTime;
	float HIT_EXPIRE_TIME = 2f;

	void Awake()
	{
		playerController = GetComponent<PlayerController>();
		index = GetComponent<CharacterBaseController>().index;
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		soundController = GameObject.Find("GameController").GetComponent<SoundController>();
		fist = transform.Find ("Fist").gameObject;
		//ShowMultipleFist ();
		HideMultipleFist ();

		tRecord = gameObject.GetComponentInChildren<TrailRenderer> ().material.mainTexture;
		//find cd animator
		//gameObject.GetComponentsInChildren<Animator> ().SetValue ();
//		foreach(Transform child in transform) {
//			if(child.name == "cd") {
//				cd = child;
//			}
//		}
		
	}

	void Start()
	{
		characterbaseController = GetComponent<CharacterBaseController>();
	}

	void Update()
	{
		// fist
		if (isMultipleFistExist) {
			fistTime -= Time.deltaTime;

			if (fistTime <= 0f) {
				HideMultipleFist();
			}
		}

		if(playerController != null)
		{
			if((index == 1 && Input.GetKeyDown(KeyCode.W)) ||
				(index == 2 && Input.GetKeyDown(KeyCode.A)) ||
				(index == 3 && Input.GetKeyDown(KeyCode.S)) ||
				(index == 4 && Input.GetKeyDown(KeyCode.D)) ||

			   (PSMoveInput.IsConnected && PSMoveInput.MoveControllers[playerController.GetPSMoveIndex()].Data.ValueT > 0))
			{
				Attack();
			}
		}

		if(lastHitExpireTime > 0)
		{
			lastHitExpireTime -= Time.deltaTime;

			if(lastHitExpireTime <= 0)
			{
				lastHitFrom = null;
			}
		}
	}

	void FixedUpdate() {
		if (hasHighScore) {
			GetCrown (gameObject, true);
			gameObject.GetComponent<CharacterBaseController>().RainbowTrail();
			} else {
			GetCrown(gameObject,false);
			gameObject.GetComponentInChildren<TrailRenderer> ().material.mainTexture = tRecord;
			}
	}
    
	public void MissAnimation(Vector3 position) {
		soundController.PlaySound("whoosh", 0.8f, false);
		AnimateMiss(position);
	}

	public void HitAnimation(Vector3 position) {
		//play toet sound
		soundController.PlaySound("punch Sound", 0.8f, false);
		AnimateHit (position);
	}

    public void Attack()
	{
		fist.SendMessage("Attack");

		// multiple fist
		if (isMultipleFistExist) {
			for (int i = 0; i < additionalFist.Length; i++) {
				additionalFist[i].SendMessage("Attack", transform);
			}
		}
    }

	public void ShowMultipleFist() {
		isMultipleFistExist = true;
		fistTime = MAX_FIST_TIME;

		for (int i = 0; i < additionalFist.Length; i++) {
			additionalFist[i].SetActive(true);
		}
	}

	public void HideMultipleFist() {
		isMultipleFistExist = false;
		fistTime = 0f;

		for (int i = 0; i < additionalFist.Length; i++) {
			additionalFist[i].SetActive(false);
		}
	}

	public void KnockedDown(Vector3 direction, GameObject from)
	{
		lastHitFrom = from;
		lastHitExpireTime = HIT_EXPIRE_TIME;

		//there's a possibility that the player is knocked down while respawn in midair. 
		characterbaseController.SetHasTouchedTile(true);

		rigidbody.AddForce(direction * forceMagnitude, ForceMode.Impulse);
	}

	public void AnimateHit(Vector3 position) {
		GameObject tmp = Instantiate (punchEffect, position, Quaternion.identity) as GameObject;
		Destroy(tmp, 1f);
	}

	public void AnimateMiss(Vector3 position) {
		GameObject tmp =  Instantiate (missEffect, position, Quaternion.identity) as GameObject;
		Destroy(tmp, 1f);
    }

	public void AnimateCd(){
		StartCoroutine (AnimateCd_Routine());
	}

	IEnumerator AnimateCd_Routine() {
		if (!iscd) {
			iscd = true;
			cd.GetComponent<Animator> ().SetBool ("cd", true);
			yield return new WaitForSeconds (1f);
			iscd = false;
			cd.GetComponent<Animator> ().SetBool ("cd", false);
		} else {
			yield break;
		}
	}

	public int GetKillCount()
	{
		return killCount;
	}

	public void Killed()
	{
		if(lastHitFrom != null)
		{
			lastHitFrom.GetComponent<PlayerAttack>().IncKillCount();
			if(firstKillPlayer == null)
			{
				firstKillPlayer = lastHitFrom;
			}
		}

		deathCount++;
		isInKS = false;
		ksCount = 0;
	}

	public void IncKillCount()
	{
		killCount++;
		hasHighScore = GetHighScore ();
		isInKS = true;
		CountKillingStrike ();
	}

	public GameObject GetLastHitFrom()
	{
		return lastHitFrom;
	}

	public void SetLastHitFrom(GameObject lastHitFrom)
	{
		this.lastHitFrom = lastHitFrom;
	}

	public int GetDeathCount()
	{
		return deathCount;
	}

	public bool IsFirstKill()
	{
		return firstKillPlayer == this.gameObject;
	}

	bool GetHighScore() {
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
			if (go != gameObject) {
				//print(go.name + "Score is :" + GetScore(go));
				//print(gameObject.name + "Score is :" + GetScore(gameObject));
				if (GetScore (go) >= GetScore (gameObject)) {
					return false;
				}else {
					continue;
				}
			}
		}

		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player != gameObject) {
				player.GetComponent<PlayerAttack> ().hasHighScore = false;
			}
		}

		return true;
	}

	public bool IsMultipleFistExist()
		{
			return isMultipleFistExist;
		}

		//-------test
		//	public void SetValue(int killCount, int deathCount, bool isFirstKill)
		//	{
		//		this.killCount = killCount;
		//		this.deathCount = deathCount;
		//		if(isFirstKill)
		//		{
		//			firstKillPlayer = this.gameObject;
		//		}
		//	}
		//-------test

	int GetScore(GameObject P) {
		PlayerAttack pa = P.GetComponent<PlayerAttack>();
		int k = pa.GetKillCount();
		int d = pa.GetDeathCount();
		int f = pa.IsFirstKill() ? 1 : 0;
		return k * 1000 - d * 10 + f;
		}

	void GetCrown(GameObject go , bool b) {
		foreach(Transform tf in go.transform) {
			if(tf.gameObject.name == "crown") {
				tf.gameObject.SetActive(b);
			}
		}
	}

	void ReturnTail() {
	}

	void CountKillingStrike() {
		if(isInKS) {
			ksCount ++;
			switch(ksCount) {
			case 1 :
				print(gameObject.name + "first kill");
				break;
			case 2 :
				print(gameObject.name + "double kill");
				break;
			case 3:
				print(gameObject.name + "triple kill");
				break;
			case 4:
				print(gameObject.name + "Q kill");
				break;
			case 5:
				print(gameObject.name + "Rampage");
				break;
			}
		}
	}
}
