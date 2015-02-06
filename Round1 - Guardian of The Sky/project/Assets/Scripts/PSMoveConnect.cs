using UnityEngine;
using System.Collections;

public class PSMoveConnect : MonoBehaviour {

	public string ipAddress = "128.2.239.45";
	public string port = "7899";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnGUI() {
		if(!PSMoveInput.IsConnected) {
			GUI.Label(new Rect(20, 45, 30, 35), "IP:");
			ipAddress = GUI.TextField(new Rect(60, 45, 120, 25), ipAddress);
			
			GUI.Label(new Rect(190, 45, 30, 35), "port:");
			port = GUI.TextField(new Rect(230, 45, 50, 25), port);
			
			if(GUI.Button(new Rect(300, 40, 100, 35), "Connect")) {
				PSMoveInput.Connect(ipAddress, int.Parse(port));
			}
			
		}else {	
			if(GUI.Button(new Rect(20, 40, 100, 35), "Disconnect"))  {
				PSMoveInput.Disconnect();
			}
		}

		if(GUI.Button(new Rect(20, 80, 100, 35), "Start"))  {
			Application.LoadLevel("menuui");
		}

	}

}
