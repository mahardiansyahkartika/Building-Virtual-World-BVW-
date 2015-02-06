using UnityEngine;
using System.Collections;

public class MasterAnimator : MonoBehaviour {

	protected SpriteRenderer spriteRenderer;
	public Sprite[] sprites;
	public float framesPerSecond;
	protected Vector3 startScale;

	protected int index;

	void Start () {
		
		spriteRenderer = renderer as SpriteRenderer;
		startScale = transform.localScale;
	}
	
	public void Play()
	{
		StartCoroutine (PlayAnimation());
	}

	public void PlayForever()
	{
		StartCoroutine (PlayAnimationForever());
	}
	
	public IEnumerator PlayAnimation()
	{
		transform.localScale = startScale;
		float startTime = Time.time;
		
		while(index < sprites.Length)
		{
			index = Mathf.FloorToInt((Time.time - startTime) * framesPerSecond);
			spriteRenderer.sprite = sprites[ index ];
			index ++;
			
			yield return null;
		}
		
		yield return new WaitForSeconds(0f);
		
		index = 0;
		transform.localScale = new Vector3 (0, 0, 0);
	}

	public IEnumerator PlayAnimationForever()
	{
		float startTime = Time.time;
		
		while(true)
		{
			index = (int)((Time.time - startTime) * framesPerSecond);
			
			index = index % sprites.Length;
			spriteRenderer.sprite = sprites[ index ];
			index ++;
			
			yield return null;
		}
		
		yield return new WaitForSeconds(0f);
		
		index = 0;
	}
}
