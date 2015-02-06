using UnityEngine;
using System.Collections;

public class CloudAnimator : MonoBehaviour {
	

	// Use this for initialization
	void Start () {

		iTween.MoveTo (gameObject,iTween.Hash("x",13,"looptype","pingPong","delay",1,"speed",3f,"easetype",iTween.EaseType.linear));
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
