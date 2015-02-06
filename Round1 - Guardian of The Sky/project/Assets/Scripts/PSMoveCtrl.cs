using UnityEngine;
using System.Collections;

//Receive inputs based on two PSMove controller
public class PSMoveCtrl : MonoBehaviour {

	public int waveAcceration = 100;	//the wave speed to be detected
	public bool waveDown = false;
	public bool doubleTriggerDown = false;

	public float rotation = 0;		//the rotation angle of the combination of the two handle, in 'Pi'
	

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//validate connection state
		if(PSMoveInput.IsConnected){
			int connectNum = 0;
			MoveData[] moveDatas = new MoveData[2];
			//assign left & right controller
			for(int i=0; i<PSMoveInput.MAX_MOVE_NUM; i++)
			{
				MoveController moveController = PSMoveInput.MoveControllers[i];
				if(moveController.Connected) {
					moveDatas[connectNum] = moveController.Data;
					connectNum++;
					if(connectNum==2)
						break;
				}
			}
			//index 0 is always the left controller
			if(connectNum==2){
				if(moveDatas[1].Position.x < moveDatas[0].Position.x){
					MoveData temp = moveDatas[0];
					moveDatas[0] = moveDatas[1];
					moveDatas[1] = temp;
				}
				
				rotation = Mathf.Atan((moveDatas[1].Position.y - moveDatas[0].Position.y)/(moveDatas[1].Position.x - moveDatas[0].Position.x))*(180/ Mathf.PI);
				if(moveDatas[0].Acceleration.y < -waveAcceration && moveDatas[1].Acceleration.y < -waveAcceration && !waveDown){
					gameObject.SendMessage("WaveDownNoTrigger");
					waveDown = true;
				}else if(moveDatas[0].Acceleration.y < -waveAcceration && moveDatas[1].Acceleration.y < -waveAcceration && waveDown){
					waveDown = false;
				}

				if(moveDatas[0].Buttons == MoveButton.T&&moveDatas[1].Buttons == MoveButton.T&&!doubleTriggerDown){
					doubleTriggerDown = true;
				}else{
					doubleTriggerDown = false;
				}

			}
		}
	}


}
