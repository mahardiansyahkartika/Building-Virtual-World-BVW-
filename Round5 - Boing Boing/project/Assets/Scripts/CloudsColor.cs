using UnityEngine;
using System.Collections;

public class CloudsColor : MonoBehaviour {

	ParticleSystem ps;
	float time;
	float timeToChange;
	const float MAX_COLOR_CHANGE = 8f; // time to color to change from one side of the band to other side of the band.
	float minColor;
	float maxColor;
	int directions; // contains 6 elements => brg brg, -1 decreasing , 1 increasing
	int[] colorType; // contains 6 elements => brg brg, b is 2, r is 0, g is 1
	int index;
	int inc;
	float red;
	float green;
	float blue;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
		minColor = Mathf.Min(Mathf.Min(ps.startColor.r, ps.startColor.g), ps.startColor.b);
		maxColor = Mathf.Max(Mathf.Max(ps.startColor.r, ps.startColor.g), ps.startColor.b);
		directions = 1;
		colorType = new int[] {2, 0, 1, 2, 0, 1};
		timeToChange = MAX_COLOR_CHANGE / 6f;
		inc = 1;
		red = maxColor;
		green = minColor;
		blue = minColor;
	}

	void Update()
	{
		time += Time.deltaTime;
		float color;
		if(directions == 1)
		{
			color = Mathf.Lerp(minColor, maxColor, time / timeToChange);
		}
		else
		{
			color = Mathf.Lerp(maxColor, minColor, time / timeToChange);
		}

		switch(colorType[index])
		{
		case 0 :
			red = color;
			break;
		case 1 :
			green = color;
			break;
		case 2 :
		default:
			blue = color;
			break;
		}

		//print (red + " || " + green + " || " + blue);
		ps.startColor = new Color(red, green, blue);

		if(time >= timeToChange)
		{
			time = 0;
			directions *= -1;

			if(index + inc >= colorType.Length || index + inc < 0)
			{
				inc *= -1;
			}
			else
			{
				index += inc;
			}
		}
	}

}
