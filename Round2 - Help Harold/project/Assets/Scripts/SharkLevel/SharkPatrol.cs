using UnityEngine;
using System.Collections;

public class SharkPatrol : MonoBehaviour 
{
	
	Vector2 Forward= new Vector2(1,0);
	public float SwimSpeed=1.0f;
	public float A;
	public float B;
	public float Omega;
	public float Phi;
	private float Theta;
	Vector2 Center;
	Vector2 NewPositionLocal = new Vector2(0,0);
	Vector2 Ahead;
	public float Thrust= 1.0f;
	HingeJoint2D LookForward;

	private int set=0;


	// Use this for initialization
	void Start () 
	{
		SwimSpeed = 1.0f;
		A = 12.0f;
		B = 18.0f;
		Omega = -0.6f;
		Phi = 3.14f;
		Thrust = 10.0f;

		Center = transform.position;
		Theta = 0f;
		Ahead = new Vector2(1,0);
		LookForward = transform.GetComponent<HingeJoint2D> ();

		LookForward.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Caught.Linked==0)
		{
			Elliptical ();
		}

		else
		{
			LookForward.enabled=true;
			rigidbody2D.AddForce(Ahead*Thrust);

		}
	}

	void Elliptical()
	{
		Theta += Omega * Time.deltaTime;

		if((Mathf.Rad2Deg*(Theta +Phi)) < 15f)
		{
			Theta = Mathf.Deg2Rad*200 - Phi; 
		}

		NewPositionLocal = new Vector2 (A * Mathf.Cos (Theta + Phi), B * Mathf.Sin (Theta + Phi));
		transform.rigidbody2D.MovePosition (Center + NewPositionLocal);
		transform.rigidbody2D.MoveRotation (Mathf.Rad2Deg*(Theta + Phi) - 90f);
	}


}
