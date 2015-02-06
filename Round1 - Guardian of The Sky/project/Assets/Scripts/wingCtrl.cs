using UnityEngine;
using System.Collections;

public class wingCtrl : MonoBehaviour {

	public Transform leftwing;
	public Transform rightwing;

	Animator rightAni;
	Animator leftAni;

	// Use this for initialization
	void Start () {
		rightAni = rightwing.GetComponent<Animator> ();
		leftAni = leftwing.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump")){
			rightAni.SetTrigger("wave");
			leftAni.SetTrigger("wave");
		}
	}
}
