using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawnObject : MonoBehaviour {

	public float length;
	public float mass;
	private float massDensity = 0.8f;

	// Use this for initialization
	void Start () {
		this.name = "Object";
		this.tag = "DrawnObject";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DeleteSmallLine(BoxCollider2D other) {
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			BoxCollider2D boxCollider = child.gameObject.GetComponent<BoxCollider2D>();

			if (other.Equals(boxCollider)) {
				if (i == 0 || i == transform.childCount - 1) { // last or first line
					if (transform.childCount == 1) {
						Destroy(this.gameObject);
					} else {
						float smallLineLength = child.GetComponent<BoxCollider2D>().size.x;
						length -= smallLineLength;
						child.parent = null;
						Destroy(child.gameObject);
						
						SetPhysicsAttribute(length);
					}
				} else {
					List<GameObject> smallLineList1 = new List<GameObject>();
					List<GameObject> smallLineList2 = new List<GameObject>();

					for (int j = 0; j < transform.childCount; j++) {
						if (j != i) {
							GameObject smallLine = transform.GetChild(j).gameObject;

							if (j < i) {
								smallLineList1.Add(smallLine);
							} else if (j > i) {
								smallLineList2.Add(smallLine);
							}
						}
					}
					// delete this object
					child.parent = null;
					Destroy(child.gameObject);
					// object 1
					SetPhysicsAttribute(GetObjectLength(smallLineList1));
					// object 2 (create new one)
					GameObject drawingObject = new GameObject();
					DrawnObject drawnObjectScript = drawingObject.AddComponent<DrawnObject>();

					foreach(GameObject smallLine in smallLineList2) {
						smallLine.transform.parent = drawingObject.transform;
					}

					drawnObjectScript.SetPhysicsAttribute(GetObjectLength(smallLineList2));
				}
				break;
			}
		}
	}

	public void SetPhysicsAttribute(float length) {
		this.length = length;
		this.mass = length * massDensity;

		if (rigidbody2D == null) {
			this.gameObject.AddComponent<Rigidbody2D> ();		
		}
		this.rigidbody2D.mass = mass;
	}

	protected float GetObjectLength(List<GameObject> smallLineList) {
		float result = 0f;

		foreach (GameObject line in smallLineList) {
			result += line.GetComponent<BoxCollider2D>().size.x;
		}

		return result;
	}
}
