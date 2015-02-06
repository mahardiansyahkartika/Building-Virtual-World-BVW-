using UnityEngine;
using System.Collections;

public class UpgradeAnimation : MasterAnimator {
	// Use this for initialization
	void Start () {
		spriteRenderer = renderer as SpriteRenderer;
	}
	
	public void Play()
	{
		StartCoroutine (PlayAnimation());
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

			yield return null;
		}
		
		yield return new WaitForSeconds(0f);
		
		index = 0;
	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		//print ("something hit me" + coll.gameObject.name);
		
		//if (coll.gameObject.tag == "NPC")
		//	coll.gameObject.transform.parent.gameObject.SendMessage ("Hit", 60f);
	}

}
