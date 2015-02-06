using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {

	public bool stopAfterFinish; // dont automatically go to the next scene if this is true
	MovieTexture movie;
	
	void Start()
	{
		movie = renderer.material.mainTexture as MovieTexture;

		float distance = gameObject.transform.position.z - GameObject.FindGameObjectWithTag("MainCamera").transform.position.z;
		// for orthographic
		float height = Camera.main.orthographicSize * 2f;
		float width = height * Screen.width * 1.0f / Screen.height;
		//print (distance + " " + Screen.width + " " + Screen.height + " " + width + " " + height + " " + (height * Screen.width * 1.0f/ Screen.height));
		gameObject.transform.localScale = new Vector3(width, height, 1);

		movie.Play();
		audio.Play();
	}

	void Update()
	{
		if (!movie.isPlaying)
		{
			if(!stopAfterFinish)
			{
				Application.LoadLevel (Application.loadedLevel +1);
			}
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}

}
