using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Tower : MonoBehaviour {

	public class TowerData {
		public float damage;
		public float range;
		public int totalBullet;
		public TowerData(float damage, float range) {
			this.damage = damage;
			this.range = range;
		}
	}

	public enum TowerType {FIRE, ICE, ELECTRIC, POISON};
	public TowerType towerType;
	public string element;

	private TowerAttackAnimator attack;
	protected GameObject magicCircle;
	protected GameObject upgradeGameObject;

	protected int level = 0;
	protected float experience = 0f;
	protected TowerData[] towerData;

	protected float[] upgradeData;
	public Sprite[] towerSprite;
	protected SpriteRenderer spriteRenderer;

	float whiteLevel = 0.7f;
	private SoundManager sm;

	// Use this for initialization
	void Start () 
	{
		sm = GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<SoundManager> ();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite = towerSprite[level];
		spriteRenderer.color = new Color (whiteLevel, whiteLevel, whiteLevel);
		towerData = new TowerData[]{
			new TowerData(20, 0.4f),
			new TowerData(30, 0.5f),
			new TowerData(40, 0.6f),
			new TowerData(50, 0.7f),
			new TowerData(60, 0.8f),
		};
		upgradeData = new float[]{100, 300, 800, 1200};
		string childAnim = string.Concat (element, "Animation");
		attack = transform.Find (childAnim).gameObject.GetComponent<TowerAttackAnimator> ();
		// disable magic circle
		magicCircle = transform.GetChild (2).gameObject;
		magicCircle.SetActive (false);
		// upgrade game object
		upgradeGameObject = transform.GetChild (3).gameObject;
	}

	// Update is called once per frame
	void Update () {
		
	}

	protected void Attack() {
		magicCircle.GetComponent<MagicCircle> ().Attack (towerData[level].range, towerData[level].damage);
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Hand")
			spriteRenderer.color = Color.white;
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject.tag == "Hand")
			spriteRenderer.color = new Color (whiteLevel, whiteLevel, whiteLevel);
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if(coll.gameObject.tag == "Hand")
		{
			//attack.Play();
			PlayAttackSound();
			Attack();
		}
	}

	void PlayAttackSound()
	{
		if(towerType == TowerType.ELECTRIC)
			sm.PlayElectricTowerSound();
		else if(towerType == TowerType.FIRE)
			sm.PlayFireTowerSound();
		else if(towerType == TowerType.ICE)
			sm.PlayWaterTowerSound();
		else if(towerType == TowerType.POISON)
			sm.PlayPoisonTowerSound();
	}

	public void AddExperience(float experience) {
		if (level < 4) { // 4 is maksimum level
			this.experience += experience;
			if (this.experience >= upgradeData [level]) {
				level++;
				// upgrade tower
				UpgradeTower();
			}
		}
	}

	protected void UpgradeTower() {
		upgradeGameObject.GetComponent<UpgradeAnimation> ().Play ();
		StartCoroutine (ChangeSprite());
	}

	IEnumerator ChangeSprite() {
		yield return new WaitForSeconds (1f);
		gameObject.GetComponent<SpriteRenderer> ().sprite = towerSprite[level];
	}
}