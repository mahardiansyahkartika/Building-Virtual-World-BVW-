using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	protected float maxHp = 100f;
	protected float hp, firstHp;

	protected float timeAnimation = 0.2f;
	protected float elapsedTime = 0f;
	protected bool isAnimatingBar = false;

	protected GameObject allBar, frontBar;

	protected SpriteRenderer sprite;
	protected int flashCount = 0;
	protected Color originalColor;

	protected bool isPoisoned = false;
	protected bool isDie = false;

	GameObject NPCManager;

	//protected float icePushForce = 5.0f;

	// Use this for initialization
	void Start () {
		int totalPath = GameObject.FindGameObjectWithTag ("PathSource").transform.childCount;
		Vector3[] path = iTweenPath.GetPath ("path" + Mathf.Round (Random.Range (1f, (float)totalPath)));

		foreach (Transform t in transform)
		{
			if(t.tag == "NPC")
				sprite = t.gameObject.GetComponent<SpriteRenderer>();
		}

		hp = maxHp;

		this.gameObject.transform.localPosition = path [0];
		
		allBar = gameObject.transform.GetChild (1).gameObject;
		frontBar = allBar.transform.GetChild (1).gameObject;

		NPCManager = GameObject.FindGameObjectWithTag ("NPC Manager");
		
		iTween.MoveTo (gameObject, iTween.Hash(
			"path", path,
			"speed", Random.Range(0.5f, 1.5f),
			"orienttopath", true,
			"axis", "y",
			"easetype", iTween.EaseType.linear
		));

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isPoisoned)
			TakeDamage(0.75f);

		allBar.transform.rotation = Quaternion.Euler (Vector3.zero);

		if (isAnimatingBar) {
			elapsedTime += Time.deltaTime;

			UpdateBar (Mathf.Lerp(firstHp, hp, elapsedTime / timeAnimation));

			if (elapsedTime >= timeAnimation) {
				isAnimatingBar = false;
				elapsedTime = 0f;
			}

			// dead
			if (hp == 0f && !isAnimatingBar) {
				Destroy(this.gameObject);
			}
		}
	}


	public void TakeDamage(float pDamage)
	{
		if (isAnimatingBar) {
			isAnimatingBar = false;
			elapsedTime = 0f;
			UpdateBar (hp);
		}
		
		firstHp = hp;
		isAnimatingBar = true;
		hp -= pDamage;
		
		if (hp <= 0) {
			hp = 0f;

			if(isPoisoned)
				Destroy(this.gameObject);
		}
	}

	public void HitByFire(float damage) {

		//black
		sprite.color = new Vector4(0, 0, 0, 1);
		TakeDamage (damage);

		NPCManager.SendMessage ("AddScore", 1);
	}

	public void HitByIce(float damage) 
	{
		//sprite.color = new Vector4(0, 0, 0.2f, 1);
		TakeDamage (damage);

		GameObject etc = GameObject.Find ("ETC");

		Vector3 pushDirection = (transform.position - etc.transform.position);

		sprite.gameObject.GetComponent<Rigidbody2D> ().AddForce (pushDirection * 1.0f, ForceMode2D.Impulse);
		sprite.gameObject.transform.parent.GetChild (1).gameObject.SetActive (false);

		Destroy (sprite.gameObject.transform.parent.gameObject, 3f);
		//Invoke ("DestroyAfterTime", 2.0f);

		NPCManager.SendMessage ("AddScore", 1);
	}

	/*public void DestroyAfterTime()
	{
		Destroy (gameObject);
	}*/

	public void HitByPoison(float damage) 
	{
		sprite.color = new Vector4(0, 1, 0, 1);
		isPoisoned = true;
				
		NPCManager.SendMessage ("AddScore", 1);
	}

	public void HitByElectric(float damage) 
	{
		originalColor = sprite.color;
		TakeDamage (damage);
		FlashSprite();

		NPCManager.SendMessage ("AddScore", 1);
	}

	private void FlashSprite()
	{
		if(flashCount < 6)
		{
			sprite.color = -1 * sprite.color;
			flashCount++;
			Invoke("FlashSprite", 0.1f);
		}
		else 
		{
			flashCount = 0;
		}
	}

	public void Hit(float damage, Tower.TowerType towerType) {
		/*
		if (isAnimatingBar) {
			isAnimatingBar = false;
			elapsedTime = 0f;
			UpdateBar (hp);
		}

		firstHp = hp;
		isAnimatingBar = true;
		hp -= damage;

		if (hp <= 0) {
			hp = 0f;
			//Destroy(this.gameObject);
		}
		*/
		switch (towerType) {
		case Tower.TowerType.FIRE :
			HitByFire(damage);
			break;
		case Tower.TowerType.ICE :
			HitByIce(damage);
			break;
		case Tower.TowerType.ELECTRIC :
			HitByElectric(damage);
			break;
		case Tower.TowerType.POISON : 
			HitByPoison(damage);
			break;
		}
	}

	public void DieInETC() {
		if (!isDie) {
			isDie = true;		

			GameObject etc = GameObject.FindGameObjectWithTag("ETC");
			
			// kill enemy
			Vector3 pushDirection = (etc.transform.position - this.transform.position);
			
			gameObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D> ().AddForce (-pushDirection.normalized * 10.0f, ForceMode2D.Impulse);
			gameObject.transform.GetChild(1).gameObject.SetActive (false);
			iTween.RotateTo(gameObject.transform.GetChild(0).gameObject, iTween.Hash(
				"z", 3600,
				"speed", 30
			));
			
			Destroy (gameObject, 3f);
		}
	}

	void UpdateBar(float hp) {
		frontBar.transform.localScale = new Vector3 (hp/maxHp, frontBar.transform.localScale.y, frontBar.transform.localScale.z);
		frontBar.transform.localPosition = new Vector3 ((0.5f * (hp / maxHp) - 0.5f) * 41f/100f, frontBar.transform.localPosition.y, frontBar.transform.localPosition.z);
	}
}
