using UnityEngine;
using System.Collections;

public class SnakeWin : MonoBehaviour {

	public GameObject snakeBody;
	private bool oneChance = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{ 
		if(other.gameObject.tag == "DrawnObject")
		{
			if (other.gameObject.rigidbody2D.mass >= 2f && oneChance) {
				oneChance = false;

				audio.Play();

				gameObject.AddComponent<Rigidbody2D>();
				snakeBody.AddComponent<Rigidbody2D>();

				StartCoroutine(LoadOnWin());
			}
		}
	}

	IEnumerator LoadOnWin() 
	{
		yield return new WaitForSeconds(GameHUB.LoadGap);
		Application.LoadLevel (Application.loadedLevel + 1);
		
	}
}
