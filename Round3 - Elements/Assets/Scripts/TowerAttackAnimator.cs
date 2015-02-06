using UnityEngine;
using System.Collections;

public class TowerAttackAnimator : MasterAnimator {

	private float colliderStartRadius;
	private CircleCollider2D collider;
	
	// Use this for initialization
	void Start () {
		collider = GetComponent<CircleCollider2D>();
		colliderStartRadius = collider.radius;

		spriteRenderer = renderer as SpriteRenderer;
		startScale = transform.localScale;
		//Play ();
	}
	
	public void Play()
	{
		print ("Play attack anim called");
		StartCoroutine (PlayAnimation());
	}
	
	public IEnumerator PlayAnimation()
	{
		transform.localScale = startScale;
		collider.radius = colliderStartRadius;
		float startTime = Time.time;
		
		while(index < sprites.Length)
		{
			//index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
			
			index = (int)((Time.time - startTime) * framesPerSecond);
			
			//index = index % sprites.Length;
			spriteRenderer.sprite = sprites[ index ];
			index ++;
			collider.radius += .5f/framesPerSecond;
			
			yield return null;
		}
		
		yield return new WaitForSeconds(0f);
		
		index = 0;
		transform.localScale = new Vector3 (0, 0, 0);
		collider.radius = colliderStartRadius;
	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		//print ("something hit me" + coll.gameObject.name);

		//if (coll.gameObject.tag == "NPC")
		//	coll.gameObject.transform.parent.gameObject.SendMessage ("Hit", 60f);
	}
}
