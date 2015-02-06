using UnityEngine;
using System.Collections;

public class ETCHealth : MonoBehaviour {

	public float etchp = 10f;
	private float dmg = 2f;
	public GameObject healthbar;
	float originalVal;
	float newVal;
	private Vector3 healthScale, healtPosition;

	private SoundManager sm;

	GameObject winLoseController;

	// Use this for initialization
	void Start () {
		winLoseController = GameObject.FindGameObjectWithTag ("WinLoseController");

		healthbar.transform.renderer.material.color = Color.green;
		//originalVal = renderer.bounds.min.x;
		//newVal = renderer.bounds.min.x;
		healthScale = healthbar.transform.localScale;
		healtPosition = healthbar.transform.localPosition;

		sm = GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<SoundManager> ();
	}


	void OnTriggerEnter2D(Collider2D col)
	{	

		if(col.gameObject.tag == "NPC")
		{
			GameObject enemy = col.gameObject;

			enemy.transform.parent.gameObject.SendMessage("DieInETC");

			etchp -= dmg/5f;

			if(etchp <= 5.0f)
				sm.PlayWarningSound();
			
			if (etchp <= 0) 
			{	
				etchp = 0;
				Debug.Log("GAME OVER!!");

				winLoseController.SendMessage("Lose");
			}
			
			UpdateHealthBar();
		}
	}

	void UpdateHealthBar()
	{
		healthbar.transform.renderer.material.color = Color.Lerp (Color.green,Color.red,1-etchp*0.1f);
		healthbar.transform.localScale = new Vector3 (healthScale.x * etchp * 0.1f, healthScale.y, healthScale.z);
		healthbar.transform.localPosition = new Vector3 (healtPosition.x - (-0.5f * (etchp / 10f) + 0.5f) * 1.3f, healtPosition.y, healtPosition.z);
	}


}
