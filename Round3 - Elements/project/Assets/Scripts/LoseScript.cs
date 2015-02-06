using UnityEngine;
using System.Collections;

public class LoseScript : MonoBehaviour {


	void Start () {

		iTween.MoveTo (gameObject,iTween.Hash("y",-0.687,"speed",3f,"easetype",iTween.EaseType.linear));

		StartCoroutine (Delayed());
	}


	IEnumerator Delayed()
	{
		yield return new WaitForSeconds (6.0f);
		Loading ();
	}

	void Loading()
	{
		Application.LoadLevel ("IntroScene");
	}

}