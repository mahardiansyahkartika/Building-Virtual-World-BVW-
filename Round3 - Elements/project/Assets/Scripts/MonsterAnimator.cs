using UnityEngine;
using System.Collections;

public class MonsterAnimator : MasterAnimator {
	
	// Use this for initialization
	void Start () {
		
		spriteRenderer = renderer as SpriteRenderer;
		Play ();
	}
	
	public void Play()
	{
		//StartCoroutine (PlayAnimation());
		StartCoroutine (PlayAnimationForever());
	}
	
	public IEnumerator PlayAnimation()
	{
		float startTime = Time.time;
		
		while(index < sprites.Length)
		{
			//index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
			
			index = (int)((Time.time - startTime) * framesPerSecond);
			
			//index = index % sprites.Length;
			spriteRenderer.sprite = sprites[ index ];
			index ++;
			//collider.radius += 1/framesPerSecond;
			
			yield return null;
		}
		
		yield return new WaitForSeconds(0f);
		
		index = 0;
		transform.localScale = new Vector3 (0, 0, 0);
		//collider.radius = colliderStartRadius;
	}

	public IEnumerator PlayAnimationForever()
	{
		float startTime = Time.time;
		
		while(true)
		{
			//index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
			
			index = (int)((Time.time - startTime) * framesPerSecond);
			
			index = index % sprites.Length;
			spriteRenderer.sprite = sprites[ index ];
			index ++;
			//collider.radius += 1/framesPerSecond;
			
			yield return null;
		}
		
		yield return new WaitForSeconds(0f);
		
		index = 0;
		//collider.radius = colliderStartRadius;
	}
	
	/*void OnTriggerEnter2D(Collider2D coll)
	{
		print ("something hit me" + coll.gameObject.name);
		if(coll.gameObject.tag == "NPC")
			Destroy(coll.gameObject);
	}*/
}
