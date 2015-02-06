using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {
	
	public GameObject selectedTower;
	private float leastDist;
	
	private GameObject[] towers; 
	
	// Use this for initialization
	void Start () {
		towers = GameObject.FindGameObjectsWithTag("Tower");
	}
	
	
	void Update () 
	{
		//towers = GameObject.FindGameObjectsWithTag("Tower");
		
		if (Input.GetKeyDown (KeyCode.W))
			FindTowerUp ();
		
		if (Input.GetKeyDown (KeyCode.A))
			FindTowerLeft();
		
		if (Input.GetKeyDown (KeyCode.S))
			FindTowerDown();
		
		if (Input.GetKeyDown (KeyCode.D))
			FindTowerRight();
	}
	
	private void FindTowerUp()
	{
		GameObject nearestTower = null;
		
		foreach(GameObject t in towers)
		{
			//all towers higher than the current tower
			if(t.transform.position.y > selectedTower.transform.position.y)
			{
				//least distance is the distance from the first tower in the array to the current tower
				leastDist = t.transform.position.y - selectedTower.transform.position.y;
				nearestTower = t;
			}
		}
		
		if(nearestTower == null)
		{
			return;
		}
		
		foreach(GameObject t in towers)
		{
			if(t.transform.position.y > selectedTower.transform.position.y)
			{
				if((t.transform.position.y - selectedTower.transform.position.y) < leastDist)
				{
					leastDist = t.transform.position.y - selectedTower.transform.position.y;
					nearestTower = t;
				}
			}
		}		
		
		selectedTower = nearestTower;
		transform.position = (nearestTower.transform.position);
	}
	
	private void FindTowerDown()
	{
		GameObject nearestTower = null;
		
		foreach(GameObject t in towers)
		{
			//all towers lower than the current tower
			if(t.transform.position.y < selectedTower.transform.position.y)
			{
				//least distance is the distance from the first tower in the array to the current tower
				leastDist = selectedTower.transform.position.y - t.transform.position.y;
				nearestTower = t;
			}
		}
		
		if(nearestTower == null)
		{
			return;
		}
		
		foreach(GameObject t in towers)
		{
			if(t.transform.position.y < selectedTower.transform.position.y)
			{
				if((selectedTower.transform.position.y - t.transform.position.y) < leastDist)
				{
					leastDist = selectedTower.transform.position.y - t.transform.position.y;
					nearestTower = t;
				}
			}
		}		
		
		selectedTower = nearestTower;
		transform.position = (nearestTower.transform.position);
	}
	
	private void FindTowerRight()
	{
		GameObject nearestTower = null;
		
		foreach(GameObject t in towers)
		{
			//all towers to the right of the current tower
			if(t.transform.position.x > selectedTower.transform.position.x)
			{
				//least distance is the distance from the first tower in the array to the current tower
				leastDist = t.transform.position.x - selectedTower.transform.position.x;
				nearestTower = t;
			}
		}
		
		if(nearestTower == null)
		{
			return;
		}
		
		foreach(GameObject t in towers)
		{
			if(t.transform.position.x > selectedTower.transform.position.x)
			{
				if((t.transform.position.x - selectedTower.transform.position.x) < leastDist)
				{
					leastDist = t.transform.position.x - selectedTower.transform.position.x;
					nearestTower = t;
				}
			}
		}		
		
		selectedTower = nearestTower;
		transform.position = (nearestTower.transform.position);
	}
	
	private void FindTowerLeft()
	{
		GameObject nearestTower = null;
		
		foreach(GameObject t in towers)
		{
			//all towers to the left of the current tower
			if(t.transform.position.x < selectedTower.transform.position.x)
			{
				//least distance is the distance from the first tower in the array to the current tower
				leastDist = Mathf.Abs(selectedTower.transform.position.x - t.transform.position.x);
				nearestTower = t;
			}
		}
		
		if(nearestTower == null)
		{
			return;
		}
		
		foreach(GameObject t in towers)
		{
			if(t.transform.position.x < selectedTower.transform.position.x)
			{
				if(Mathf.Abs(t.transform.position.x - selectedTower.transform.position.x) < leastDist)
				{
					leastDist = Mathf.Abs(t.transform.position.x - selectedTower.transform.position.x);
					nearestTower = t;
				}
			}
		}		
		
		selectedTower = nearestTower;
		transform.position = (nearestTower.transform.position);
	}
	
	//minDist = Vector2.Distance(Vector2(transform.position.x,transform.position.y), Vector2(transform.TransformPoint(selectedTower.transform.position.x,transform.position.y)));
}
