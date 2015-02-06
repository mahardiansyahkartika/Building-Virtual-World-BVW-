using UnityEngine;
using System.Collections;

/// <summary>
/// Control the navigation of story/tutorial pictures
/// </summary>
public class PicBoardTutorialCtrl : MonoBehaviour {
	
	public float transitSpeed = 5;	//speed of transition
	
	private int picCount = 0;
	private int curIndex = 0;
	private Vector3 preferPos;
	private Transform _transform;
	private bool leftTrigger = false;
	private bool rightTrigger = true;
	
	// Use this for initialization
	void Start () {
		curIndex = 0;
		preferPos = transform.position;
		_transform = transform;
		picCount = _transform.childCount;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Application.LoadLevel("terrain");		
		}

		//apply transition if picture change
		if(Mathf.Abs(_transform.position.x-preferPos.x)>0.01f){
			_transform.position = Vector3.Lerp(_transform.position, preferPos, Time.deltaTime*transitSpeed);
		}
		
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
				if(moveDatas[0].Buttons == MoveButton.T){
					if(!leftTrigger){
						LastPic();
						leftTrigger = true;
					}
				}else{
					leftTrigger = false;
				}
				
				if(moveDatas[1].Buttons == MoveButton.T){
					if(!rightTrigger){
						NextPic();
						rightTrigger = true;
					}
				}else{
					rightTrigger = false;
				}
				
				if(leftTrigger&&rightTrigger){
					Application.LoadLevel("terrain");
				}
				
			}
		}else{
			if(Input.GetKeyDown(KeyCode.A)){
				LastPic();
			}else if(Input.GetKeyDown(KeyCode.D)){
				NextPic();
			}
		}
		
	}
	
	public void NextPic(){
		if(curIndex<picCount-1){
			curIndex++;
			preferPos = new Vector3(-curIndex*15, _transform.position.y, _transform.position.z);
		}
	}
	
	public void LastPic(){
		if(curIndex>0){
			curIndex--;
			preferPos = new Vector3(-curIndex*15, _transform.position.y, _transform.position.z);
		}
	}
	
}
