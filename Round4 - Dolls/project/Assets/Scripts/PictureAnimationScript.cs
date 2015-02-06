using UnityEngine;
using System.Collections;

public class PictureAnimationScript : MonoBehaviour {
    float backupTime = 23.0f;
	// Use this for initialization
	void Start () {
        StartCoroutine(backup());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator backup()
    {
        Vector3 pos = transform.localPosition;
        for (float i = 0; i < backupTime; i += Time.deltaTime)
        {
            float step = i / backupTime;
            pos.z += (step * 0.0005f);
            transform.localPosition = pos;
            yield return null;
        }
    }
}
