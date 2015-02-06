using UnityEngine;
using System.Collections;

public class LabelCtrl : MonoBehaviour {

	private TextMesh textData;
	private Color tempcolor;

	// Use this for initialization
	void Start () {
		textData = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		tempcolor = new Color (1,1,1,Mathf.PingPong(Time.time*1, 1));
		textData.color = tempcolor;
	}
}
