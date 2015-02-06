using UnityEngine;
using System.Collections;

public class TileMovement : MonoBehaviour {

	bool isMoving;
	float targetHeight; // y axis
	float moveTime = 2f;
	float normalY;

	Vector3 normalPos;
	Vector3 normalColliderPos;
	float shakingTime;
	float shakingDuration;
	float shakeSpeed = 10f;
	const float MAX_SHAKE_TIME = Mathf.PI;
	bool isAboutToFall;

	ShakeType shakeType;

	float disappearTime;
	const float MAX_DISAPPEAR_TIME = 2f;
	Vector3 oriScale;

	BoxCollider boxCollider;

	TileController tileController;
	public int index;

	public enum ShakeType
	{
		Left, 
		Right, 
		Forward, 
		Backward,
		AboutToFall,
		Down,
	}

	void Awake()
	{
		normalY = transform.position.y;
		boxCollider = GetComponent<BoxCollider>();
		normalColliderPos = boxCollider.center;
	}

	void Start()
	{
		normalPos = transform.position;
		oriScale = transform.localScale;
		tileController = GameObject.Find("GameController").GetComponent<TileController>();
	}

	public void SetTileHeight(float targetHeight)
	{
		if(isMoving)
			return;

		if(transform.position.y == targetHeight)
		{
			return;
		}

		this.targetHeight = targetHeight;
		isMoving = true;
		iTween.MoveTo(this.gameObject, iTween.Hash("y", targetHeight, "easeType", "easeInOutExpo", "time", moveTime, "oncomplete", "OnMoveComplete", "oncompletetarget", this.gameObject));
	}

	public void OnMoveComplete()
	{
		isMoving = false;
		this.transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
		this.gameObject.SetActive(false);
		tileController.RemoveActiveTiles(gameObject);
	}

	public bool IsWalkable()
	{
		bool ret = isMoving == false;
		ret = ret && transform.position.y == normalY;

		return ret;
	}

	public void SetTileHeightToNormal()
	{
		SetTileHeight(normalY);
	}

	void Update()
	{
		if(shakingTime > 0)
		{
			shakingTime -= Time.deltaTime * shakeSpeed;
			if(shakingTime < 0)
			{
				shakingTime = 0;

				if(shakeType == ShakeType.AboutToFall)
				{
					SetTileHeight(-40f);
				}
			}

			Vector3 deltaPos = Vector3.zero;
			switch(shakeType)
			{

			case ShakeType.Left:
				deltaPos = new Vector3(-Mathf.Sin(shakingTime) * 0.2f, 0, 0);
				break;
			case ShakeType.Right:
				deltaPos = new Vector3(Mathf.Sin(shakingTime) * 0.2f, 0, 0);
				break;
			case ShakeType.Forward:
				deltaPos = new Vector3(0, 0, Mathf.Sin(shakingTime) * 0.2f);
				break;
			case ShakeType.Backward:
				deltaPos = new Vector3(0, 0, -Mathf.Sin(shakingTime) * 0.2f);
				break;
			case ShakeType.AboutToFall:
				//float progress = shakingTime / shakingDuration;
				//float magnitude = progress * progress * 0.25f;
				float magnitude = 0.08f;
				deltaPos = new Vector3(Random.value, Random.value, Random.value) * magnitude;
				break;
			case ShakeType.Down:
			default:
				deltaPos = new Vector3(0, -Mathf.Sin(shakingTime) * 0.2f, 0);
				break;
			}

			//tile might shake, but collider stay still
			transform.position = normalPos + deltaPos;
			boxCollider.center = normalColliderPos + new Vector3(-deltaPos.x * (1 / transform.localScale.x), -deltaPos.y * (1 / transform.localScale.y), -deltaPos.z* (1 / transform.localScale.z));
		}

		if(disappearTime > 0)
		{
			disappearTime -= Time.deltaTime;
			if(disappearTime < 0)
			{
				this.gameObject.SetActive(false);
				tileController.RemoveActiveTiles(this.gameObject);
				disappearTime = 0;
			}

			float progress = disappearTime * 1.0f / MAX_DISAPPEAR_TIME;
			transform.localScale = oriScale * progress;
		}
	}

	public bool IsDisappearing()
	{
		return disappearTime > 0;
	}

	public bool IsAboutToFall()
	{
		return isAboutToFall;
	}

	public void ShakeTile(ShakeType shakeType)
	{
		shakingTime = MAX_SHAKE_TIME;

		if(shakeType == ShakeType.AboutToFall)
		{
			shakingTime = 6 * MAX_SHAKE_TIME;
			isAboutToFall = true;
		}

		shakingDuration = shakingTime;
		this.shakeType = shakeType;
	}

	public void Disappear()
	{
		disappearTime = MAX_DISAPPEAR_TIME;
	}
}
