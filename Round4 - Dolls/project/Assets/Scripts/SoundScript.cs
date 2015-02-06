using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour {
    AudioSource[] sources;
    public AudioClip[] clips;
    public float[] times;

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;

        sources = GetComponents<AudioSource>();
        StartCoroutine(playOverlap());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator playOverlap()
    {
        for (int i = 0; i < times.Length; i++)
        {
            yield return new WaitForSeconds(times[i]);
            sources[i].clip = clips[i];
            sources[i].Play();
        }

        bool stillplaying = false;
        do
        {
            yield return null;
            stillplaying = false;
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].isPlaying)
                    stillplaying = true;
            }
        } while (stillplaying);

        yield return new WaitForSeconds(0f);
        //Application.LoadLevel("MainScene");
		GameObject.Find ("blackbg").SendMessage("StartTransition");
    }

	void PlayClip(AudioClip clip) {
		audio.clip = clip;
		audio.Play ();
	}
}
