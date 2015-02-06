using UnityEngine;
using System.Collections;

public class WaveTxt : MonoBehaviour {

	public GameObject npcmanager;
	private bool fade = true;
	private int wavenum ;
	private float c;
	private int totalwaves;

	void Start()
	{	
		ShowWaveNumber ();
	}

	public void ShowWaveNumber()
	{
		wavenum = npcmanager.GetComponent<NPCManager> ().currentWave;
		totalwaves = npcmanager.GetComponent<NPCManager>().totalNumberOfWaves; 
		guiText.text = "Wave:"+ (wavenum+1)+"/"+totalwaves;
		c = guiText.material.color.a;

		fade = true;
	}


	void Update () {

		if(fade)
		{

			c -= 0.1f * Time.deltaTime * 5f;
			guiText.material.color = new Color(c, guiText.material.color.r, guiText.material.color.g, guiText.material.color.b);

			if(c<=0)
				fade = false;

		}

	}
	


}
