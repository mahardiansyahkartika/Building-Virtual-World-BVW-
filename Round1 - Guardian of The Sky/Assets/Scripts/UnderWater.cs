using UnityEngine;
using System.Collections;

public class UnderWater : MonoBehaviour {
	
	public Transform Water;
	
	//The scene's default fog settings
	private bool defaultFog;
	private Color defaultFogColor;
	private float defaultFogDensity;
	private bool inWater = false;

	void Start () {
		defaultFog = RenderSettings.fog;
		defaultFogColor = RenderSettings.fogColor;
		defaultFogDensity = RenderSettings.fogDensity;
	}
	
	void Update () {
		if (transform.position.y < Water.position.y)
		{
			RenderSettings.fog = true;
			RenderSettings.fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
			RenderSettings.fogDensity = 0.04f;
			gameObject.SendMessage("RestoreFire");
			if(inWater==false){
				inWater = true;
				SoundFXCtrl.instance.PlaySound(0,1);
			}
		}
		else
		{
			RenderSettings.fog = defaultFog;
			RenderSettings.fogColor = defaultFogColor;
			RenderSettings.fogDensity = defaultFogDensity;

			if(inWater==true){
				inWater = false;
				SoundFXCtrl.instance.PlaySound(0,1);
			}

		}
	}
}