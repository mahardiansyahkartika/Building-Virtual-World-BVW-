using UnityEngine;
using System.Collections;

public class MagicCircle : MonoBehaviour {

	protected bool isAttack = false;

	protected float speed = 2.5f;
	protected float delay = 0f;
	protected float damage;

	protected Tower.TowerType towerType;

	protected Tower parentScript;

	// Use this for initialization
	void Start () {
		parentScript = gameObject.transform.parent.gameObject.GetComponent<Tower> ();
		towerType = parentScript.towerType;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Attack(float target, float damage) {
		if (!isAttack) {
			isAttack = true;
			gameObject.transform.localScale = Vector3.zero;
			gameObject.SetActive(true);
			this.damage = damage;

			iTween.ScaleTo(gameObject, iTween.Hash(
				"x", target, "y", target, "z", target,
				"speed", speed,
				"easetype", iTween.EaseType.linear,
				"oncomplete", "EndAnimation"
			));
		}
	}

	public void EndAnimation() {
		iTween.ScaleTo (gameObject, iTween.Hash(
			"x", 0f, "y", 0f, "z", 0f,
			"delay", delay,
			"speed", speed,
			"easetype", iTween.EaseType.linear,
			"oncomplete", "InitAnimation"
		));
	}

	public void InitAnimation() {
		gameObject.SetActive (false);
		isAttack = false;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.tag == "NPC")
		{
			GameObject enemy = coll.gameObject.transform.parent.gameObject;

			switch(towerType) {
			case Tower.TowerType.FIRE : 
				enemy.SendMessage("HitByFire", damage);
				break;
			case Tower.TowerType.ICE : 
				enemy.SendMessage("HitByIce", damage);
				break;
			case Tower.TowerType.ELECTRIC : 
				enemy.SendMessage("HitByElectric", damage);
				break;
			case Tower.TowerType.POISON :
				enemy.SendMessage("HitByPoison", damage);
				break;
			}

			// add experience to tower
			parentScript.AddExperience(damage);
		}
	}
}
