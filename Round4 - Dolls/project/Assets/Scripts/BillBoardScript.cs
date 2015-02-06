using UnityEngine;
using System.Collections;

public class BillBoardScript : MonoBehaviour {
    public OVRCameraController camcontrol;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        Vector3 rot = Camera.main.transform.rotation.eulerAngles;
        rot.z = 0;
        rot.x = 0;
        Quaternion qrot = Quaternion.Euler(rot);

        transform.LookAt(transform.position + qrot * Vector3.forward,
            qrot * Vector3.up);
    }
}
