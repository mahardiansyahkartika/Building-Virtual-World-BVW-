using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	const float panTime = 1f;
	const float rotateSpeed = 1.5f; // relative speed
	const float translateSpeed = 1.5f;
	Vector3 oriPos;
	float minZoom;
	float maxZoom;
	float deltaX;
	float deltaZ;
	const float MIN_DELTA_X = 0;
	const float MAX_DELTA_X = 0;
	float playerSpawnYPos;

	int activePlayerCount;

	GameController gameController;

	void Awake()
	{
		oriPos = transform.position;
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		activePlayerCount = gameController.activePlayersCount;
		playerSpawnYPos = gameController.GetSpawnYPos();
	}

	public void PanTo(Vector3 target)
	{
		iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "easeType", "easeInOutExpo", "time", panTime));
	}
	
	void LateUpdate()
	{
		CalcDelta();
		//print (deltaX + " " + deltaZ);

		Vector3 newPos = new Vector3(deltaX, transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, newPos, translateSpeed * Time.deltaTime);
		Quaternion lookAtRotation = Quaternion.LookRotation(new Vector3(deltaX - transform.position.x, playerSpawnYPos - transform.position.y, deltaZ - transform.position.z), Vector3.up);
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAtRotation, rotateSpeed * Time.deltaTime);

		//Debug.DrawRay(transform.position, transform.forward * 15, Color.yellow);
	}

	void CalcDelta()
	{
		float minX = Mathf.Infinity, maxX = Mathf.NegativeInfinity, minZ = Mathf.Infinity, maxZ = Mathf.NegativeInfinity;

		for(int i=0; i<activePlayerCount; i++)
		{
			minX = Mathf.Min(minX, gameController.GetPlayer(i + 1).transform.position.x);
			maxX = Mathf.Max(maxX, gameController.GetPlayer(i + 1).transform.position.x);
			minZ = Mathf.Min(minZ, gameController.GetPlayer(i + 1).transform.position.z);
			maxZ = Mathf.Max(maxZ, gameController.GetPlayer(i + 1).transform.position.z);
		}

		deltaX = Mathf.Clamp((minX + maxX) * 0.5f, 6f, 10f);
		deltaZ = Mathf.Clamp((minZ + maxZ) * 0.5f, 4f, 7f);
	}
}
