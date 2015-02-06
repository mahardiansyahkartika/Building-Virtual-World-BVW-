using UnityEngine;
using System;
using System.Collections;

public class SceneFader : MonoBehaviour {
	
	float fadeTime;
	bool isFading;
	float targetAlpha;
	float vel;
	Action callback;
	
	void Awake()
	{
		guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
		guiTexture.enabled = false;
	}
	
	void Update()
	{
		if(isFading)
		{
			float alpha = guiTexture.color.a;
			alpha = Mathf.SmoothDamp(alpha, targetAlpha, ref vel, fadeTime);
			guiTexture.color = new Color(guiTexture.color.r, guiTexture.color.g, guiTexture.color.b, alpha);
			
			if(Mathf.Abs(guiTexture.color.a - targetAlpha) <= 0.05f)
			{
				guiTexture.color = new Color(guiTexture.color.r, guiTexture.color.g, guiTexture.color.b, targetAlpha);
				isFading = false;
				if(targetAlpha == 0f)
				{
					guiTexture.enabled = false;
				}
				
				if(callback != null)
				{
					callback();
				}
			}
		}
	}
	
	public void FadeInScene(float time = 1f, Action callback = null)
	{
		this.callback = callback;
		guiTexture.enabled = true;
		isFading = true;
		targetAlpha = 0f;
		fadeTime = time;
	}
	
	public void FadeOutScene(float time = 1f, Action callback = null)
	{
		this.callback = callback;
		guiTexture.enabled = true;
		isFading = true;
		targetAlpha = 1f;
		fadeTime = time;
	}
	
}
