using UnityEngine;
using System.Collections;

public class Caught : MonoBehaviour 
{
	public Transform  Shark;
	public Transform Kid;
	public float Width=0.05f;
	public static int Linked=0;

	Color Purple = new Color(0.8f, 0, 0.8f);

	LineRenderer LR;
	SpringJoint2D Spring;

	// Use this for initialization
	void Start () 
	{
		LR= transform.GetComponent<LineRenderer>();
		LR.SetWidth (Width, Width);
		LR.enabled = false;

		Spring = transform.GetComponent<SpringJoint2D> ();
		Spring.enabled = false;
		Linked = 0;


	}
	
	// Update is called once per frame
	void Update () 
	{
		if((HaroldHook.Hooked==1)&&(SharkHooked.Hooked==1))
		{
			Linked=1;

			Spring.enabled=true;
			LR.enabled=true;

			Spring.distance = Vector2.Distance (Kid.position ,Shark.position);

			GameObject[] DrawnLineList = GameObject.FindGameObjectsWithTag("DrawnObject");

			for (int i = 0; i < DrawnLineList.Length; i++)
			{
				DrawnLineList[i].SetActive(false);
			}

			SharkHooked.Hooked=0;
			HaroldHook.Hooked=0;

			// audio
			audio.Play();
			// shark audio
			Shark.gameObject.audio.Play();

			StartCoroutine(LoadOnWin());
		}

		LR.SetPosition (0, Kid.position);
		LR.SetPosition (1, Shark.position);

	}

	IEnumerator LoadOnWin()
	{
		yield return new WaitForSeconds (8.0f);
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
