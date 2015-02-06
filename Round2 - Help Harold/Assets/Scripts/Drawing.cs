using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawing : MonoBehaviour {
	public enum ObjectType {CRAYON, ERASER};
	public enum HandType {LEFTHAND, RIGHTHAND};

	protected ObjectType selectedObjectType = ObjectType.CRAYON;
	protected HandType handType = HandType.RIGHTHAND;
	
	public SkeletonWrapper sw;

	private float width = 0.15f;
	private float minDistance = 0.2f;
	private Color lineColor = new Color(205f/255f, 13f/255f, 228f/255f);

	private bool isDoing;
	private bool isSwitching;

	private LineRenderer line;
	private List<Vector3> pointsList;
	private List<GameObject> lineList;

	private GameObject drawingObject;
	private float totalLength = 0f;

	public GameObject testSquare,testCircle;

	public GameObject crayonObject, eraserObject;
	protected GameObject selectedObjectAll, otherObjectAll;
	protected GameObject selectedMainObject, selectedShadowObject;
	protected GameObject otherMainObject, otherShadowObject;
	protected GameObject belt, sliderBar, flagHand, glowObject;

	protected Quaternion crayonTargetRotation, shadowCrayonTargetRotation, eraserTargetRotation, shadowEraserTargetRotation;
	protected Vector3 crayonTargetPosition, shadowCrayonTargetPosition, eraserTargetPosition, shadowEraserTargetPosition;

	private List<Vector3> bonePosition;

	private Vector2 rangeXKinect, rangeYKinect, rangeXWorld, rangeYWorld;

	private Vector3 crayonTopBeltPosition, crayonBottomBeltPosition, eraserTopBeltPosition, eraserBottomBeltPosition;

	// slider
	private float sliderLength = 7.4f;

	// switching animation attribute
	private float animationTime = 0.7f;
	private float elapsedTime = 0f;
	private float alphaShadow;
	private bool isFirstAnimationDone;

	// SELECTED object
	// main object
	private Vector3 firstPosSelectedObject, targetPosSelectedObject;
	private Quaternion firstRotSelectedObject, targetRotSelectedObject;
	private Vector3 firstScaleSelectedObject, targetScaleSelectedObject;
	private float firstAlphaSelectedObject, targetAlphaSelectedObject;
	// shadow main object
	private Vector3 firstPosShadowSelectedObject, targetPosShadowSelectedObject;
	private Quaternion firstRotShadowSelectedObject, targetRotShadowSelectedObject;

	// OTHER object
	// main object
	private Vector3 firstPosOtherObject, targetPosOtherObject;
	// shadow main object
	private float firstAlphaShadowOtherObject, targetAlphaShadowOtherObject;

	// smoothing attribute
	private float kFilteringFactor = 0.1f;

	// Kinect attributes
	protected bool isUsingKinect = true;
	protected bool isAnimating = false;

	private int shoulderCenterIndex = 2;
	private int handLeftIndex = 7;
	private int handRightIndex = 11;
	private int selectedHandIndex;
	private int sliderHandIndex;

	private float distanceKinectOn = 0.3f;

	//Assignments for a bitmask to control which bones to look at and which to ignore
	public enum BoneMask
	{
		None = 0x0,
		Hip_Center = 0x1,
		Spine = 0x2,
		Shoulder_Center = 0x4,
		Head = 0x8,
		Shoulder_Left = 0x10,
		Elbow_Left = 0x20,
		Wrist_Left = 0x40,
		Hand_Left = 0x80,
		Shoulder_Right = 0x100,
		Elbow_Right = 0x200,
		Wrist_Right = 0x400,
		Hand_Right = 0x800,
		Hip_Left = 0x1000,
		Knee_Left = 0x2000,
		Ankle_Left = 0x4000,
		Foot_Left = 0x8000,
		Hip_Right = 0x10000,
		Knee_Right = 0x20000,
		Ankle_Right = 0x40000,
		Foot_Right = 0x80000,
		All = 0xFFFFF,
		Torso = 0x10000F, //the leading bit is used to force the ordering in the editor
		Left_Arm = 0x1000F0,
		Right_Arm = 0x100F00,
		Left_Leg = 0x10F000,
		Right_Leg = 0x1F0000,
		R_Arm_Chest = Right_Arm | Spine,
		No_Feet = All & ~(Foot_Left | Foot_Right),
		UpperBody = Shoulder_Center | Head|Shoulder_Left | Elbow_Left | Wrist_Left | Hand_Left|
		Shoulder_Right | Elbow_Right | Wrist_Right | Hand_Right	
	}
	public BoneMask Mask = BoneMask.All;

	// Use this for initialization
	void Start () {
		isDoing = false;
		isSwitching = false;
		isAnimating = false;
		pointsList = new List<Vector3> ();
		lineList = new List<GameObject> ();
		bonePosition = new List<Vector3> ();

		// init range
		rangeXKinect = new Vector2 (-0.2f, 0.6f);
		rangeYKinect = new Vector2 (0.6f, 1.5f);
		rangeXWorld = new Vector2 (-11f, 11f);
		rangeYWorld = new Vector2 (-9f, 6f);
		//rangeXKinect = new Vector2 (0.0f, 0.8f);
		//rangeYKinect = new Vector2 (0.8f, 1.7f);
		//rangeXWorld = new Vector2 (-10f, 10f);
		//rangeYWorld = new Vector2 (-5f, 5f);

		GameObject swObject = GameObject.FindGameObjectWithTag ("KinectPrefab");
		belt = GameObject.FindGameObjectWithTag ("Belt");
		sliderBar = GameObject.FindGameObjectWithTag ("SliderBar");
		flagHand = GameObject.FindGameObjectWithTag ("FlagHand");
		glowObject = GameObject.FindGameObjectWithTag ("Glow");

		if (isUsingKinect) {
			sw = swObject.GetComponent<SkeletonWrapper> (); 
		}

		// prepare anyting here
		SetAttributeByHandType ();
		SetSelectedObject ();
		SetTransformObject ();

		// set animation position
		crayonTopBeltPosition = new Vector3 (-0.58f, -2.3f, 0f) + belt.transform.position;
		crayonBottomBeltPosition = new Vector3 (-0.58f, -4.5f, 0f) + belt.transform.position;
		eraserTopBeltPosition = new Vector3 (0.84f, -1.4f, 0f) + belt.transform.position;
		eraserBottomBeltPosition = new Vector3 (0.84f, -3.4f, 0f) + belt.transform.position;

		PutOtherObjectToStrap ();
		SetOrderInLayer ();

		alphaShadow = selectedShadowObject.GetComponent<SpriteRenderer> ().color.a;

		// animating eraser
		AnimationEraserFirst ();
	}

	void AnimationEraserFirst() {
		Eraser eraserScript = GameObject.FindGameObjectWithTag("Eraser").GetComponent<Eraser>();

		if (eraserScript.isAnimatingFirst) {

		}
	}

	void StopDoing() {
		isDoing = false;
		
		if (selectedObjectType == ObjectType.CRAYON) {
			if (totalLength > 0) {
				// activate collider
				/*
				foreach (GameObject smallLine in lineList) {
					BoxCollider2D boxCollider = smallLine.GetComponent<BoxCollider2D>();
					boxCollider.enabled = true;
				}
				*/
				
				// add script
				DrawnObject script = drawingObject.GetComponent<DrawnObject>();
				// set physics attribute
				script.SetPhysicsAttribute(totalLength);
			} else {
				Destroy(drawingObject);
			}
			
			// init
			pointsList.RemoveRange(0, pointsList.Count);
			lineList.RemoveRange(0, lineList.Count);
			totalLength = 0f;
		} else if (selectedObjectType == ObjectType.ERASER) {
			Eraser eraserScript = selectedMainObject.GetComponent<Eraser>();
			eraserScript.isReadyErasing = false;
		}
	}

	// Update is called once per frame
	void Update () {
		// kinect update
		UpdateKinect ();

		// update slider
		UpdateSlider ();

		// update object
		if (!isAnimating) {
			if (IsStartDoing()) {
				isDoing = true;
				
				if (selectedObjectType == ObjectType.CRAYON) {
					drawingObject = new GameObject();
					drawingObject.AddComponent<DrawnObject>();

					// audio
					crayonObject.audio.Play ();
				} else if (selectedObjectType == ObjectType.ERASER) {
					Eraser eraserScript = selectedMainObject.GetComponent<Eraser>();
					eraserScript.isReadyErasing = true;
				}
			} else if(IsEndDoing()) {
				StopDoing();
			}
			
			// drawing line
			if (isDoing) {
				// creating line
				if (selectedObjectType == ObjectType.CRAYON) {
					Vector3 inputPosition = GetInputPosition();
					
					if (pointsList.Count == 0 || Vector3.Distance(pointsList[pointsList.Count - 1], inputPosition) >= minDistance) {
						pointsList.Add (inputPosition);
						
						if (pointsList.Count > 1) {
							lineList.Add(CreateSmallLine(pointsList[pointsList.Count-1], pointsList[pointsList.Count - 2]));
						}
					}
				}
			}
			
			// pointer update
			UpdatePointer ();
		}

		// switch handler
		SwitchHandlerUpdate ();
	}

	public bool GetIsDoing() {
		return isDoing;
	}

	protected void SwitchHandlerUpdate() {
		if (isAnimating) {
			elapsedTime += Time.deltaTime;
			float currentTime = elapsedTime / (animationTime/2f);
			float secondCurrentTime = (elapsedTime-animationTime/2f) / (animationTime/2f);

			if (!isFirstAnimationDone) {
				// SELECTED OBJECT
				// position
				selectedObjectAll.transform.position = Vector3.Slerp(firstPosSelectedObject, targetPosSelectedObject, currentTime);
				// MAIN OBJECT
				// rotation
				selectedMainObject.transform.rotation = Quaternion.Slerp(firstRotSelectedObject, targetRotSelectedObject, currentTime);
				// scale
				selectedMainObject.transform.localScale = Vector3.Slerp(firstScaleSelectedObject, targetScaleSelectedObject, currentTime);
				// alpha
				float currAlpha1 = Vector3.Slerp(Vector3.one * firstAlphaSelectedObject, Vector3.one*targetAlphaSelectedObject, currentTime).x;
				Color color1 = selectedMainObject.GetComponent<SpriteRenderer>().color;
				selectedMainObject.GetComponent<SpriteRenderer>().color = new Color(color1.r, color1.g, color1.b, currAlpha1);
				// SHADOW MAIN OBJECT
				// position
				selectedShadowObject.transform.localPosition = Vector3.Slerp(firstPosShadowSelectedObject, targetPosShadowSelectedObject, currentTime);
				// rotation
				selectedShadowObject.transform.rotation = Quaternion.Slerp(firstRotShadowSelectedObject, targetRotShadowSelectedObject, currentTime);
				
				// OTHER OBJECT
				// position
				otherObjectAll.transform.position = Vector3.Slerp(firstPosOtherObject, targetPosOtherObject, currentTime);
				// SHADOW MAIN OBJECT
				// alpha
				float currAlpha2 = Vector3.Slerp(Vector3.one * firstAlphaShadowOtherObject, Vector3.one*targetAlphaShadowOtherObject, currentTime).x;
				Color color2 = otherShadowObject.GetComponent<SpriteRenderer>().color;
				otherShadowObject.GetComponent<SpriteRenderer>().color = new Color(color2.r, color2.g, color2.b, currAlpha2);
			} else { // second animation
				// SELECTED OBJECT
				// position
				selectedObjectAll.transform.position = Vector3.Slerp(firstPosSelectedObject, GetInputPosition(), secondCurrentTime);
				// MAIN OBJECT
				// rotation
				selectedMainObject.transform.rotation = Quaternion.Slerp(firstRotSelectedObject, targetRotSelectedObject, secondCurrentTime);
				// scale
				selectedMainObject.transform.localScale = Vector3.Slerp(Vector3.one, Vector3.one * GetScale(), secondCurrentTime);
				// alpha
				float currAlpha1 = Vector3.Slerp(Vector3.one, Vector3.one * GetAlpha(), secondCurrentTime).x;
				Color color1 = selectedMainObject.GetComponent<SpriteRenderer>().color;
				selectedMainObject.GetComponent<SpriteRenderer>().color = new Color(color1.r, color1.g, color1.b, currAlpha1);
				// SHADOW MAIN OBJECT
				// position
				selectedShadowObject.transform.localPosition = Vector3.Slerp(Vector3.zero, GetShadowLocalPosition(), secondCurrentTime);
				// rotation
				selectedShadowObject.transform.rotation = Quaternion.Slerp(firstRotShadowSelectedObject, targetRotShadowSelectedObject, secondCurrentTime);

				// OTHER OBJECT
				// position
				otherObjectAll.transform.position = Vector3.Slerp(firstPosOtherObject, targetPosOtherObject, secondCurrentTime);
				// SHADOW MAIN OBJECT
				float currAlpha2 = Vector3.Slerp(Vector3.one * firstAlphaShadowOtherObject, Vector3.one*targetAlphaShadowOtherObject, secondCurrentTime).x;
				Color color2 = otherShadowObject.GetComponent<SpriteRenderer>().color;
				otherShadowObject.GetComponent<SpriteRenderer>().color = new Color(color2.r, color2.g, color2.b, currAlpha2);
			}

			if (elapsedTime >= animationTime/2f && !isFirstAnimationDone) {
				isFirstAnimationDone = true;

				SwitchObject();
				SetOrderInLayer();

				StartSwitchingSecondAnimation();
			}

			if (elapsedTime >= animationTime && isAnimating) {
				elapsedTime = 0f;

				isAnimating = false;
			}
		} else { // !isAnimating
			if (isSwitching) {
				if (!IsInside(belt.GetComponent<BoxCollider2D>(), GetInputPosition())) {
					isSwitching = false;
				}
			} else {
				if (IsInside(belt.GetComponent<BoxCollider2D>(), GetInputPosition())) {
					if (isDoing) {
						StopDoing();
					}

					// change proses begin
					isSwitching = true;
					// begin animation
					StartSwitchingAnimation();
				}
			}
		}
	}

	protected void StartSwitchingAnimation() {
		isAnimating = true;
		isFirstAnimationDone = false;
		elapsedTime = 0f;

		// SELECTED OBJECT
		// position
		firstPosSelectedObject = selectedObjectAll.transform.position;
		targetPosSelectedObject = (selectedObjectType == ObjectType.CRAYON) ? crayonBottomBeltPosition : eraserBottomBeltPosition;
		// MAIN OBJECT
		// rotation
		firstRotSelectedObject = selectedMainObject.transform.rotation;
		targetRotSelectedObject = Quaternion.Euler (0f, 0f, 0f);
		// scale
		firstScaleSelectedObject = selectedMainObject.transform.localScale;
		targetScaleSelectedObject = Vector3.one;
		// alpha
		firstAlphaSelectedObject = selectedMainObject.GetComponent<SpriteRenderer> ().color.a;
		targetAlphaSelectedObject = 1f;
		// SHADOW MAIN OBJECT
		// position
		firstPosShadowSelectedObject = selectedShadowObject.transform.localPosition;
		targetPosShadowSelectedObject = Vector3.zero;
		// rotation
		firstRotShadowSelectedObject = selectedShadowObject.transform.rotation;
		targetRotShadowSelectedObject = Quaternion.Euler (0f, 0f, 0f);

		// OTHER OBJECT
		firstPosOtherObject = otherObjectAll.transform.position;
		targetPosOtherObject = (selectedObjectType == ObjectType.CRAYON) ? eraserBottomBeltPosition : crayonBottomBeltPosition;
		// SHADOW MAIN OBJECT
		firstAlphaShadowOtherObject = 0f;
		targetAlphaShadowOtherObject = alphaShadow;

		// sound
		belt.audio.Play ();
	}

	protected void StartSwitchingSecondAnimation() {
		// SELECTED OBJECT
		// position
		firstPosSelectedObject = (selectedObjectType == ObjectType.CRAYON) ? crayonBottomBeltPosition : eraserBottomBeltPosition;
		// MAIN OBJECT
		// rotation
		firstRotSelectedObject = Quaternion.Euler (0f, 0f, 0f);
		targetRotSelectedObject = (selectedObjectType == ObjectType.CRAYON) ? crayonTargetRotation : eraserTargetRotation;
		// SHADOW MAIN OBJECT
		// rotation
		firstRotShadowSelectedObject = Quaternion.Euler (0f, 0f, 0f);
		targetRotShadowSelectedObject = (selectedObjectType == ObjectType.CRAYON) ? shadowCrayonTargetRotation : shadowEraserTargetRotation;

		// OTHER OBJECT
		firstPosOtherObject = (selectedObjectType == ObjectType.CRAYON) ? eraserBottomBeltPosition : crayonBottomBeltPosition;
		targetPosOtherObject = (selectedObjectType == ObjectType.CRAYON) ? eraserTopBeltPosition : crayonTopBeltPosition;
		// SHADOW MAIN OBJECT
		firstAlphaShadowOtherObject = alphaShadow;
		targetAlphaShadowOtherObject = 0f;
	}
	
	protected void SwitchObject() {
		if (selectedObjectType == ObjectType.CRAYON) {
			selectedObjectType = ObjectType.ERASER;
		} else if (selectedObjectType == ObjectType.ERASER) {
			selectedObjectType = ObjectType.CRAYON;
		}
		
		SetSelectedObject ();
	}

	protected void PutOtherObjectToStrap() {
		// position
		Vector3 position = (selectedObjectType == ObjectType.CRAYON) ? eraserTopBeltPosition : crayonTopBeltPosition;
		otherObjectAll.transform.position = position;
		// shadow alpha
		Color color = otherShadowObject.GetComponent<SpriteRenderer> ().color;
		otherShadowObject.GetComponent<SpriteRenderer> ().color = new Color (color.r, color.g, color.b, 0f);
	}

	protected void SetOrderInLayer() {
		selectedMainObject.GetComponent<SpriteRenderer> ().sortingOrder = 5;
		selectedShadowObject.GetComponent<SpriteRenderer> ().sortingOrder = 4;
		otherMainObject.GetComponent<SpriteRenderer> ().sortingOrder = 2;
		otherShadowObject.GetComponent<SpriteRenderer> ().sortingOrder = 1;
	}

	protected void SetSelectedObject() {
		if (selectedObjectType == ObjectType.CRAYON) {
			selectedObjectAll = crayonObject;
			otherObjectAll = eraserObject;

			selectedMainObject = GameObject.FindGameObjectWithTag ("PurpleCrayon");
			selectedShadowObject = GameObject.FindGameObjectWithTag ("ShadowCrayon");

			otherMainObject = GameObject.FindGameObjectWithTag ("Eraser");
			otherShadowObject = GameObject.FindGameObjectWithTag ("ShadowEraser");
		} else if (selectedObjectType == ObjectType.ERASER) {
			selectedObjectAll = eraserObject;
			otherObjectAll = crayonObject;

			selectedMainObject = GameObject.FindGameObjectWithTag ("Eraser");
			selectedShadowObject = GameObject.FindGameObjectWithTag ("ShadowEraser");

			otherMainObject = GameObject.FindGameObjectWithTag ("PurpleCrayon");
			otherShadowObject = GameObject.FindGameObjectWithTag ("ShadowCrayon");
		} else {
			Debug.LogError("shit happen with selected object");
		}
	}

	protected void SetAttributeByHandType() {
		if (handType == HandType.RIGHTHAND) {
			selectedHandIndex = handRightIndex;
			sliderHandIndex = handLeftIndex;
			// crayon
			crayonTargetRotation = Quaternion.Euler(0f,0f,-41f);
			crayonTargetPosition = Vector3.zero;
			// shadow crayon
			shadowCrayonTargetRotation = Quaternion.Euler(0f,0f,-73f);
			shadowCrayonTargetPosition = Vector3.zero;
			// eraser
			eraserTargetRotation = Quaternion.Euler(0f,0f,-41f);
			eraserTargetPosition = Vector3.zero;
			// shadow eraser
			shadowEraserTargetRotation = Quaternion.Euler(0f,0f,-73f);
			shadowEraserTargetPosition = new Vector3(0.2f,0.1f,0f);
			// belt position
			belt.transform.localPosition = new Vector3(10f,5.5f,0f);
			// slider bar position
			sliderBar.transform.position = new Vector3(-13f,0f,0f);
			// hand flag position
			flagHand.transform.position = new Vector3(-13f,-1f,0f); // disable drawing first
		} else if (handType == HandType.LEFTHAND) {
			selectedHandIndex = handLeftIndex;
			sliderHandIndex = handRightIndex;
			// crayon
			crayonTargetRotation = Quaternion.Euler(0f,0f,41f);
			crayonTargetPosition = Vector3.zero;
			// shadow crayon
			shadowCrayonTargetRotation = Quaternion.Euler(0f,0f,73f);
			shadowCrayonTargetPosition = Vector3.zero;
			// eraser
			eraserTargetRotation = Quaternion.Euler(0f,0f,41f);
			eraserTargetPosition = Vector3.zero;
			// shadow eraser
			shadowEraserTargetRotation = Quaternion.Euler(0f,0f,73f);
			shadowEraserTargetPosition = new Vector3(-0.2f,0.1f,0f);
			// belt position
			belt.transform.localPosition = new Vector3(-10f,5.5f,0f);
			// slider bar position
			sliderBar.transform.position = new Vector3(13f,0f,0f);
			// hand flag position
			flagHand.transform.position = new Vector3(13f,-1f,0f); // disable drawing first
			flagHand.transform.localScale = new Vector3 (-1f * flagHand.transform.localScale.x, flagHand.transform.localScale.y, flagHand.transform.localScale.z);
		} else {
			Debug.LogError("shit happen with handtype");
		}
	}

	protected void SetTransformObject() {
		if (selectedObjectType == ObjectType.CRAYON) {
			// main object
			selectedMainObject.transform.rotation = crayonTargetRotation;
			selectedMainObject.transform.localPosition = crayonTargetPosition;
			// shadow
			selectedShadowObject.transform.rotation = shadowCrayonTargetRotation;
			selectedShadowObject.transform.localPosition = shadowCrayonTargetPosition;
		} else if (selectedObjectType == ObjectType.ERASER) {
			// main object
			selectedMainObject.transform.rotation = eraserTargetRotation;
			selectedMainObject.transform.localPosition = eraserTargetPosition;
			// shadow
			selectedShadowObject.transform.rotation = shadowEraserTargetRotation;
			selectedShadowObject.transform.localPosition = shadowEraserTargetPosition;
		}
	}

	protected Vector3 GetInputPosition() {
		Vector3 result;

		if (isUsingKinect) {
			result = bonePosition[selectedHandIndex];
			result.x = -(((rangeXKinect.y - result.x) * (rangeXWorld.y - rangeXWorld.x) / (rangeXKinect.y - rangeXKinect.x)) - rangeXWorld.y);
			result.y = -(((rangeYKinect.y - result.y) * (rangeYWorld.y - rangeYWorld.x) / (rangeYKinect.y - rangeYKinect.x)) - rangeYWorld.y);
		} else {
			result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		result.z = 0f;
		return result;
	}

	protected bool IsStartDoing() {
		if (selectedObjectType == ObjectType.ERASER) {
			return true;		
		}

		if (!isDoing) {
			if (GetDepth() >= distanceKinectOn) {
				return true;
			}
		}

		return false;
	}

	protected bool IsEndDoing() {
		if (selectedObjectType == ObjectType.ERASER) {
			return false;		
		}

		if (isDoing) {
			if (GetDepth() < distanceKinectOn) {
				return true;
			}
		}

		return false;
	}

	protected void UpdateSlider() {
		Vector2 rangeSlider = new Vector2 (-sliderLength / 2f, sliderLength / 2f);
		float yPos = flagHand.transform.position.y;

		if (isUsingKinect) {
			yPos = -(((rangeYKinect.y - bonePosition [sliderHandIndex].y) * (rangeSlider.y - rangeSlider.x) / (rangeYKinect.y - rangeYKinect.x)) - rangeSlider.y);
		} else {
			float speed = 10f;

			if (Input.GetKey(KeyCode.W)) {
				yPos += Time.deltaTime * speed;
			} else if (Input.GetKey(KeyCode.S)) {
				yPos -= Time.deltaTime * speed;
			}
		}

		if (yPos > rangeSlider.y) {
			yPos = rangeSlider.y;		
		} else if (yPos < rangeSlider.x) {
			yPos = rangeSlider.x;
		}

		// glow
		if (yPos <= 0f) {
			glowObject.transform.position = new Vector3(sliderBar.transform.position.x, -5.7f, 0f);
		} else {
			glowObject.transform.position = new Vector3(sliderBar.transform.position.x, 5.7f, 0f);
		}

		flagHand.transform.position = new Vector3 (flagHand.transform.position.x, yPos, flagHand.transform.position.z);
	}

	protected GameObject CreateSmallLine(Vector3 pos1, Vector3 pos2) {
		GameObject smallLine = new GameObject ();
		smallLine.name = "smallLine";
		float length = Vector3.Distance (pos1, pos2);
		totalLength += length;

		// square
		GameObject square = CreateSquare (length);
		square.transform.parent = smallLine.transform;

		// 2 circles
		for (int i = 0; i < 2; i++) {
			GameObject circle = CreateCircle();
			int direction = (i == 0) ? -1 : 1;
			circle.transform.localPosition = new Vector3(direction * length / 2f, 0, 0);
			circle.transform.parent = smallLine.transform;
		}

		// collider
		BoxCollider2D collider = smallLine.AddComponent<BoxCollider2D> ();
		collider.size = new Vector2 (length, width);
		collider.tag = "SmallLine";
		//collider.enabled = false;

		smallLine.transform.rotation = Quaternion.Euler (0, 0, Mathf.Rad2Deg * (Mathf.Atan((pos2.y - pos1.y) / (pos2.x - pos1.x))));
		smallLine.transform.position = (pos2 + pos1) / 2f;
		smallLine.transform.parent = drawingObject.transform;

		return smallLine;
	}

	protected GameObject CreateCircle() {
		//GameObject circle = GameObject.Instantiate (Resources.LoadAssetAtPath ("Assets/Prefabs/circle.prefab", typeof(GameObject))) as GameObject;
		GameObject circle = GameObject.Instantiate (testCircle) as GameObject;

		// sprite renderer
		SpriteRenderer spriteRenderer = circle.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = lineColor;
		circle.transform.localScale = Vector3.one * width;

		return circle;
	}

	protected GameObject CreateSquare(float length) {
		//GameObject square = GameObject.Instantiate (Resources.LoadAssetAtPath("Assets/Prefabs/square.prefab", typeof(GameObject))) as GameObject;
		GameObject square = GameObject.Instantiate (testSquare) as GameObject;

		// sprite renderer
		SpriteRenderer spriteRenderer = square.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = lineColor;
		square.transform.localScale = new Vector3(length, width, 1f);

		return square;
	}

	protected void UpdateKinect() {
		if (isUsingKinect) {
			if (sw.pollSkeleton())
			{
				float scale = 1f;
				
				//bonePosition.RemoveRange(0, bonePosition.Count);
				
				for( int i = 0; i < (int)Kinect.NuiSkeletonPositionIndex.Count; i++) {
					//_bonePos[ii] = sw.getBonePos(ii);
					if( ((uint)Mask & (uint)(1 << i) ) > 0 ){
						//_bones[i].transform.localPosition = sw.bonePos[player,ii];
						Vector3 position = sw.bonePos[0,i] * scale;

						if (i >= bonePosition.Count) { // list still empty
							bonePosition.Add(position);
						} else {
							// smoothing algorithm
							Vector3 smoothPosition = new Vector3(
								(position.x * kFilteringFactor) + (bonePosition[i].x * (1.0f - kFilteringFactor)),
								(position.y * kFilteringFactor) + (bonePosition[i].y * (1.0f - kFilteringFactor)),
								(position.z * kFilteringFactor) + (bonePosition[i].z * (1.0f - kFilteringFactor))
							);

							bonePosition[i] = smoothPosition;
						}
						//_bones[i].transform.localPosition = position;
					}
				}
			}
		}
	}

	protected float GetScale() {
		float scale;
		Vector2 rangeDepth = new Vector2 (0f, distanceKinectOn);
		float maxScale = 2f;

		scale = -(((1f - maxScale) * (rangeDepth.y - GetDepth()) / (rangeDepth.y - rangeDepth.x)) - 1);

		if (scale > maxScale) scale = maxScale;
		else if (scale < 1) scale = 1;

		return scale;
	}

	protected float GetAlpha() {
		float alpha;
		Vector2 rangeDepth = new Vector2 (0f, distanceKinectOn);
		float minAlpha = 0.4f;

		alpha = -(((1f - minAlpha) * (rangeDepth.y - GetDepth()) / (rangeDepth.y - rangeDepth.x)) - 1);

		if (alpha > 1) alpha = 1;
		else if (alpha < minAlpha) alpha = minAlpha;

		return alpha;
	}

	protected Vector3 GetShadowLocalPosition() {
		Vector3 result;
		float scale = GetScale ();
		Vector2 scaleTransform = (handType == HandType.RIGHTHAND) ? new Vector2 (4f, 3f) : new Vector2 (-4f, 3f);

		result = new Vector3((scale-1) * scaleTransform.x, -(scale-1) * scaleTransform.y, selectedShadowObject.transform.localScale.z);
		if (selectedObjectType == ObjectType.ERASER) {
			result += shadowEraserTargetPosition;		
		}

		return result;
	}

	protected void UpdatePointer() {
		// position
		selectedObjectAll.transform.position = GetInputPosition ();

		float scale = GetScale();
		float alpha = GetAlpha();
		
		// alpha
		SpriteRenderer sprite = selectedMainObject.GetComponent<SpriteRenderer> ();
		sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, alpha);
		// scaling
		selectedMainObject.transform.localScale = Vector3.one * scale;
		// translate shadow
		selectedShadowObject.transform.localPosition = GetShadowLocalPosition();
	}

	protected float GetDepth() {
		if (selectedObjectType == ObjectType.ERASER) {
			return distanceKinectOn;
		} else { // selectedObjectType == ObjectType.CRAYON
			if (flagHand.transform.position.y >= 0) {
				return distanceKinectOn;
			} else {
				return -((sliderLength/2f + flagHand.transform.position.y) * (-distanceKinectOn) / (sliderLength/2f));
			}
		}
	}

	protected bool IsInside (Collider2D collider, Vector3 point) {
		return collider.bounds.Contains(point);
	}
}
