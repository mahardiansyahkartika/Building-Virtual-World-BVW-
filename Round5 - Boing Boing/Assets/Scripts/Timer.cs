using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	
	public Texture[] textures;
	GameController gameController;
	int index;
	float time;
	float maxGameTime;
	float deltaTime;
	GUITexture guiTexture;
	GUIText text;

	void Awake()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		maxGameTime = gameController.GetMaximumGameTime();
		deltaTime = maxGameTime / textures.Length;
		index = textures.Length - 1;
		guiTexture = GetComponent<GUITexture>();
		text = GetComponent<GUIText>();
	}

	void Update()
	{
		time = gameController.GetRemainingGameTime();
		text.text = "" + (((int)(time * 100f)) / 100f);
		int temp = Mathf.CeilToInt(time / deltaTime) - 1;
		if(index != temp)
		{
			index = temp;

			if(index == -1)
			{
				guiTexture.texture = null;
			}
			else
			{
				guiTexture.texture = textures[index];
			}
		}
	}

}
