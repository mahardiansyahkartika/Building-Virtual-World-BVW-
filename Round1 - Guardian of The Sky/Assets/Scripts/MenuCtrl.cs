using UnityEngine;
using System.Collections;

public class MenuCtrl : MonoBehaviour {

	public Transform   OVRCamera;		//Left/Right camera of OVR
	public int curSelected = 0;			//current selection index, the larger number the lower the position of option
	public Transform[] options;
	public float vertRotateLimit = 0.1f;//the minimun rotation angle to apply oculus movement
	public float changeDelayTime = 0.5f;//the delay time between options change
	public bool enableOculus = false;

	public GameObject credit;
	public GameObject title;
	public GameObject button0;
	public GameObject option0;
	public GameObject button1;
	public GameObject option1;

	private Transform _transform;
	private float changeTime;
	private PSMoveCtrl psmoveCtrl;

	private bool isMenu = true;

	// Use this for initialization
	void Start () {
		_transform = transform;
		changeTime = Time.time + changeDelayTime;
		psmoveCtrl = GetComponent<PSMoveCtrl> ();

		credit.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isMenu) {
			if(enableOculus){
				if(Time.time>changeTime){
					//apply vertical rotation by oculus
					Vector3 cameraDelta = _transform.InverseTransformDirection(OVRCamera.forward);
					if(cameraDelta.y>vertRotateLimit){
						MoveUp();
					}else if(cameraDelta.y<-vertRotateLimit){
						MoveDown();
					}
				}
			}else{
				if(Input.GetKeyDown(KeyCode.W)){
					MoveUp();
				}else if(Input.GetKeyDown(KeyCode.S)){
					MoveDown();
				}
			}
			
			//validate connection state
			if (PSMoveInput.IsConnected) {
				int connectNum = 0;
				MoveData[] moveDatas = new MoveData[2];
				//assign left & right controller
				for (int i=0; i<PSMoveInput.MAX_MOVE_NUM; i++) {
					MoveController moveController = PSMoveInput.MoveControllers [i];
					if (moveController.Connected) {
						moveDatas [connectNum] = moveController.Data;
						connectNum++;
						if (connectNum == 2)
							break;
					}
				}
				//index 0 is always the left controller
				if (connectNum == 2) {
					if (moveDatas [0].GetButtonsDown (MoveButton.T) || moveDatas [1].GetButtonsDown (MoveButton.T)) {
						Click(curSelected);
					}
				}
				if(psmoveCtrl.doubleTriggerDown){
					Click(curSelected);
				}
			} else {
				if(Input.GetButtonDown("Jump")){
					Click(curSelected);
				}
			}
		} else { // credit
			if (PSMoveInput.IsConnected) {
				if(psmoveCtrl.doubleTriggerDown){
					isMenu = true;

					credit.renderer.enabled = false;
					SetMenuEnable(true);
				}
			}
		}
	}

	void SetMenuEnable(bool value) {
		title.renderer.enabled = value;
		button0.renderer.enabled = value;
		option0.renderer.enabled = value;
		button1.renderer.enabled = value;
		option1.renderer.enabled = value;
	}

	void Click(int curSelected) {
		options [curSelected].SendMessage ("Click");

		if (curSelected == 1) {
			isMenu = false;

			credit.renderer.enabled = true;
			SetMenuEnable(false);
		}
	}

	void MoveUp(){
		if(curSelected>0){
			curSelected--;
			changeTime = Time.time + changeDelayTime;
		}
	}

	void MoveDown(){
		if(curSelected<options.Length-1){
			curSelected++;
			changeTime = Time.time + changeDelayTime;
		}
	}

}
