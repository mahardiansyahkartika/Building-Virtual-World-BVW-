using UnityEngine;
using System.Collections;

public class OptionCtrl : MonoBehaviour {

	public int optionIndex;

	private MenuCtrl menuCtrl;
	private TextMesh textMesh;

	// Use this for initialization
	void Start () {
		menuCtrl = GetComponentInParent<MenuCtrl> ();
		textMesh = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(menuCtrl.curSelected == optionIndex){
			textMesh.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time*2, 1));
		}else{
			textMesh.color = Color.white;
		}
	}

	public void Click(){
		if (optionIndex == 0) {
			Application.LoadLevel ("storyui");
		}
		else if (optionIndex == 1) {
			//Application.Quit ();
		}
	}

}
