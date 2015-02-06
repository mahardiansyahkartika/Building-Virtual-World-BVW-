using UnityEngine;
using System.Collections;

public class TwoHandSelector : MonoBehaviour {

	private GameObject leftSelector;
	private GameObject rightSelector;

	public float speed = 5.0f;

	public bool isUsingLeapMotion = true;
	public LeapController leapController;

	private float leftBoundary;
	private float rightBoundary;

	Vector3 screenSize; 
	float spriteSize;

	// Use this for initialization
	void Start () 
	{
		leftSelector = transform.Find ("leftHand").gameObject;
		rightSelector = transform.Find ("rightHand").gameObject;

		screenSize = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
		spriteSize = leftSelector.renderer.bounds.size.x/2;
	}
	
	// Update is called once per frame
	void Update () {
		if (isUsingLeapMotion) { // leap motion
			leftSelector.rigidbody2D.transform.localPosition = leapController.leftHandPosition;
			rightSelector.rigidbody2D.transform.localPosition = leapController.rightHandPosition;
		} else { // keyboard
			if (Input.GetKey (KeyCode.W)) 
			{
				leftSelector.rigidbody2D.transform.Translate(Vector2.up * speed * Time.deltaTime);
				//rigidbody2D.AddForce (Vector3.up * 300 * Time.deltaTime, ForceMode2D.Impulse);
			}
			
			if (Input.GetKey (KeyCode.S)) 
			{
				leftSelector.rigidbody2D.transform.Translate(-Vector2.up * speed * Time.deltaTime);
			}
			
			if (Input.GetKey (KeyCode.A)) 
			{
				leftSelector.rigidbody2D.transform.Translate(-Vector2.right * speed * Time.deltaTime);
			}
			
			if (Input.GetKey (KeyCode.D)) 
			{
				leftSelector.rigidbody2D.transform.Translate(Vector2.right * speed * Time.deltaTime);
			}
			
			if (Input.GetKey (KeyCode.UpArrow)) 
			{
				rightSelector.rigidbody2D.transform.Translate(Vector2.up * speed * Time.deltaTime);
				//rigidbody2D.AddForce (Vector3.up * 300 * Time.deltaTime, ForceMode2D.Impulse);
			}
			
			if (Input.GetKey (KeyCode.DownArrow)) 
			{
				rightSelector.rigidbody2D.transform.Translate(-Vector2.up * speed * Time.deltaTime);
			}
			
			if (Input.GetKey (KeyCode.LeftArrow)) 
			{
				rightSelector.rigidbody2D.transform.Translate(-Vector2.right * speed * Time.deltaTime);
			}
			
			if (Input.GetKey (KeyCode.RightArrow)) 
			{
				rightSelector.rigidbody2D.transform.Translate(Vector2.right * speed * Time.deltaTime);
			}
			
			/*if(leftSelector.transform.position.x > (screenSize.x - spriteSize))
			{
				leftSelector.transform.position = new Vector3((screenSize.x - spriteSize), leftSelector.transform.position.y, leftSelector.transform.position.z);
			}

			if(leftSelector.transform.position.x < (-screenSize.x + spriteSize))
			{
				leftSelector.transform.position = new Vector3((-screenSize.x + spriteSize), leftSelector.transform.position.y, leftSelector.transform.position.z);
			}

			if(rightSelector.transform.position.x > (screenSize.x - spriteSize))
			{
				rightSelector.transform.position = new Vector3((screenSize.x - spriteSize), rightSelector.transform.position.y, rightSelector.transform.position.z);
			}
			
			if(rightSelector.transform.position.x < (-screenSize.x + spriteSize))
			{
				rightSelector.transform.position = new Vector3((-screenSize.x + spriteSize), rightSelector.transform.position.y, rightSelector.transform.position.z);
			}*/				
		}
	}
}
