using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

	public Animator animator;
	public GameObject car;
	public GUIText guideText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput ();
	}

	void HandleInput() {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		transform.Rotate (Vector3.up, horizontal * Time.deltaTime * 200f);

		if (Input.GetAxis("Vertical") > 0) {
			animator.SetBool("isWalk", true);

			if (Input.GetKey(KeyCode.LeftShift)) {
				animator.SetBool("isRun", true);
			} else {
				animator.SetBool("isRun", false);
			}
		} else {
			animator.SetBool("isWalk", false);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			animator.SetBool ("isGrab", true);

			// change scene
			StartCoroutine("LoadNextLevel");
			// sound
			Camera.main.gameObject.SendMessage("PlayEngineStart");
		} else {
			animator.SetBool ("isGrab", false);
		}
	}

	IEnumerator LoadNextLevel(){
		yield return new WaitForSeconds(3);
		Application.LoadLevel("MainScene");
	}

	void OnTriggerEnter (Collider collider) {
		guideText.text = "Press 'Space' to start!";
	}

	void OnTriggerExit (Collider collider) {
		guideText.text = "Find a car";
	}
}
