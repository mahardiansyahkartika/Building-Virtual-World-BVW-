using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeMovement : MonoBehaviour {

	Vector3 oriPos;
	int sign;
	float random;

	void Start()
	{
		oriPos = transform.position;
		random = Random.Range(0, 4 * Mathf.PI);
		sign = Random.value >= 0.5f ? 1 : -1;
	}

	void Update()
	{
		float yOffset = sign * Mathf.Sin(Time.time * 2 + random) * 0.25f;
		float xOffset = -sign * Mathf.Cos(Time.time * 2 + random) * 0.25f;;
		this.transform.position = oriPos + new Vector3(xOffset, yOffset, 0);
	}

}
