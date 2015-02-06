using UnityEngine;
using System.Collections;

public class KiteBehaviour : MonoBehaviour {

	public float KitePush= 1.0f;
	private LineRenderer LR;
	public Transform  Kite;
	public Transform Kid;
	public float Width=0.08f;
	public static int WinFlag=0;

	// Use this for initialization
	void Start () 
	{
		WinFlag = 0;
		LR = transform.GetComponent<LineRenderer> ();
		LR.SetPosition (0, Kite.position);
		LR.SetPosition (1, Kid.position);
		LR.SetWidth (Width, Width);

		Width=0.08f;

		LR.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(LR.enabled==true)
		{
			LR.SetPosition (0, Kite.position);
			LR.SetPosition (1, Kid.position);
			LR.SetWidth (Width, Width);
		}
	}


	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag=="Win")
		{
			// audio
			audio.Play();

			WinFlag=1;
			LR.enabled=true;
			transform.GetComponent<SpringJoint2D>().enabled=true;
			Vector2 temp = new Vector2(1,-0.5f);
			rigidbody2D.AddForce(temp*KitePush);
			rigidbody2D.gravityScale = -1f;
			Debug.Log ("You Win");
			StartCoroutine(LoadOnWin());

		}
	}

	IEnumerator LoadOnWin() 
	{
		yield return new WaitForSeconds(6.5f);
		Application.LoadLevel (Application.loadedLevel + 1);
		
	}
}
