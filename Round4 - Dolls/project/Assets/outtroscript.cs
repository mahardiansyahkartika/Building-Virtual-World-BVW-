using UnityEngine;
using System.Collections;

public class outtroscript : MonoBehaviour {
	public GameObject table;
	// Use this for initialization
	void Start () {
		this.GetComponent<OculusController> ().SetEnableOculus (false);
		this.transform.LookAt (this.transform.position + Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
