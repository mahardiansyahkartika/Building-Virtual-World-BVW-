using UnityEngine;
using System.Collections;

public class WinScreenController : MonoBehaviour {

	public Sprite[] listSprite;
	private float time = 1.5f;
	private float elapsedTime = 0f;

	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;

		int index = (int)(elapsedTime / time);
		if (index > listSprite.Length-1)
			index = listSprite.Length-1;

		spriteRenderer.sprite = listSprite [index];
	}
}
