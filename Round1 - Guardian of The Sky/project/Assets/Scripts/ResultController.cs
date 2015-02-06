using UnityEngine;
using System.Collections;

public class ResultController : MonoBehaviour {

	private PSMoveCtrl psmoveCtrl;

	// Use this for initialization
	void Start () {
		psmoveCtrl = GetComponent<PSMoveCtrl>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Application.LoadLevel("menuui");		
		}

		if (PSMoveInput.IsConnected) {
			if(psmoveCtrl.doubleTriggerDown){
				Application.LoadLevel("menuui");
			}
		}
	}
}
