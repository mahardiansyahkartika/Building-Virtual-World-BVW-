using UnityEngine;
using System.Collections;

public class Angle : MonoBehaviour {


	private int ArcCount;
	private int Target;

	public float AngleLapse;
	public float TimeLapse;

	public static float Ang;

	// Use this for initialization
	void Start () 
	{
		ArcCount = 0;
		Target = 1;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Ang = rigidbody2D.rotation;
		ArcCount = arcCount (180);
		if (ArcCount >= Target) 
		{
			Target = ArcCount +1;
		}


		Debug.Log (ArcCount.ToString ());
	
	}

	int arcCount( float ArcAngle)
	{
		float theta = 0.0f;
		theta = rigidbody2D.rotation;

		int Count = 0;

		while(theta > ArcAngle)
		{
			theta -= ArcAngle;
			Count++;
		}

		return Count;
	}
}
