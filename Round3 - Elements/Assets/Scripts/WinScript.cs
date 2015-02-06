using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		iTween.MoveTo (gameObject,iTween.Hash("y",-0.687,"delay",1,"speed",3f,"easetype",iTween.EaseType.linear));

		StartCoroutine (Delaying());
		
	}


	IEnumerator Delaying()
	{
		yield return new WaitForSeconds (6.0f);
		Loading ();
	}



	void Loading()
	{
		Application.LoadLevel ("Credits");
	}
	

}
