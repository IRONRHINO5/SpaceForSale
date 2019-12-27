// Jims_CameraController.cs - By Jimbob Games 2018.
using UnityEngine;
using Jims.Profile;


namespace Jims.InputSystem
{
	public partial class Jims_CameraController : MonoBehaviour
	{
		#region

		protected static Jims_CameraController Instance;

		protected Camera m_myCamera = null;
		public Camera myCamera
		{
			get { if (m_myCamera == null)
				m_myCamera = gameObject.GetComponent<Camera>();
				return m_myCamera; }
			set { m_myCamera = value; }
		}

		public Jims_CameraControllerProfile Profile = null;

		#endregion

		#region

		//USED WITH EDITOR!
		public bool showInputSettings = false;
		public bool showLimitSettings = false;
		public bool showHelp = true;

		#endregion

		#region

		[Header ("Input System Type")]
		public InputType InputSystem;
		public enum  InputType
		{
			Windows}
		;

		#endregion

		#region

		[Header ("Movement Type")]		
		public MovementMode MoveMode;
		public enum  MovementMode
		{
			Keyboard,
			ClickDrag,
			Keyboard_and_ClickDrag,
			Target_Follow}
		;

		#endregion

		#region

		[Header ("Target To Follow")]
		public Transform TargetToFollow;
		public Vector3 TargetOffset;

		#endregion

		#region

		[Header ("Inputs")]
		[Tooltip ("Mouse Scroll Wheel Input Name!")]
		public string MouseScrollWheelInputName = "Mouse ScrollWheel";	
		[Tooltip ("Zoom In Input Key!")]
		public KeyCode ZoomInKey;
		[Tooltip ("Zoom Out Input Key!")]
		public KeyCode ZoomOutKey;		
		[Tooltip ("Drag Camera Key!")]
		public KeyCode CameraDragKey;		
		[Tooltip ("Reset Camera Key!")]
		public KeyCode CameraResetKey;

		#endregion

		#region

		[Header ("Settings")]		
		[Range (0f, 10000f)]
		[Tooltip ("Set Camera default height!")]
		public float CamHeight = 8f;		
		[Range (0.1f, 10f)]
		[Tooltip ("Set camera move speed multi!")]
		public float CamSpeed = 0.3f;		
		[Range (0.1f, 100f)]
		[Tooltip ("Multipliy 'CamSpeed' by %!")]
		public float CamHeightSpeedMulti = 2f;

		[Range (0f, 1000f)]
		[Tooltip ("If Camera is orthographic, set size value!")]
		public float CamZoomMin = 6f;		
		[Range (0f, 1000f)]
		[Tooltip ("If Camera is orthographic, set size value!")]
		public float CamZoomMax = 10f;
		[Range (-10000f, 10000f)]
		[Tooltip ("If Camera is orthographic, set Near Plane value!")]
		public float OrthNearPlane = -50f;	
		[Range (-10000f, 10000f)]
		[Tooltip ("If Camera is orthographic, set Far Plane value!")]
		public float OrthFarPlane = 50f;

		[Range (1f, 120f)]
		[Tooltip ("If Camera is Perspective, set FOV value!")]
		public float CamFOVMin = 40f;		
		[Range (1f, 120f)]
		[Tooltip ("If Camera is Perspective, set FOV value!")]
		public float CamFOVMax = 60f;
		[Range (-10000f, 10000f)]
		[Tooltip ("If Camera Perspective, set Near Plane value!")]
		public float PersNearPlane = 0.02f;
		[Range (-10000f, 10000f)]
		[Tooltip ("If Camera Perspective, set Far Plane value!")]
		public float PersFarPlane = 100f;

		[Range (-10000f, 10000f)]
		[Tooltip ("Set Backwards limit!")]
		public float CamVerticalMin = -20f;		
		[Range (-10000f, 10000f)]
		[Tooltip ("Set Forwards limit!")]
		public float CamVerticalMax = 65f;

		[Range (-10000f, 10000f)]
		[Tooltip ("Set Left limit!")]
		public float CamHorizontalMin = -20f;		
		[Range (-10000f, 10000f)]
		[Tooltip ("Set Right limit!")]
		public float CamHorizontalMax = 15f;

		#endregion

		#region

		[Header ("Private")]
		protected Vector3 ResetCamera;
		protected Vector3 CamOrigin;
		protected Vector3 Difference;

		#endregion


		protected virtual void Awake ()
		{
			if(Instance == null)
				Instance = this;
		}

		/// <summary>
		/// Load Setting's from Profile(If any), and create a Camera Reset Point!
		/// </summary>
		public virtual void Start ()
		{
			//Load data from Profile is any!
			if (Profile != null)
				Profile.Load (this);

			// Reset Camera to Start Position on Reset Key press!
			ResetCamera = myCamera.transform.position;
		}

		/// <summary>
		/// Load Setting's from Profile!
		/// </summary>
		public virtual void LoadFromProfile ()
		{
			if (Profile == null)
				return;

			Profile.Load (this);
		}

		/// <summary>
		/// Save Changes to Profile!
		/// </summary>
		public virtual void SaveToProfile ()
		{
			if (Profile == null)
				return;

			Profile.Save (this);
		}

		/// <summary>
		/// Calculates Camera Move Speed!
		/// </summary>
		float CamSpeedValue ()
		{
			return CamSpeed * 100f;
		}

		/// <summary>
		/// Calculates Camera Position Vertical!
		/// </summary>
		float CamPosZValue ()
		{
			return myCamera.transform.position.z;
		}

		/// <summary>
		/// Calculates Camera Position Horizontal!
		/// </summary>
		float CamPosXValue ()
		{
			return myCamera.transform.position.x;
		}

		/// <summary>
		/// Calculates Camera Height!
		/// </summary>
		float CamPosYValue ()
		{
			return CamHeight;
		}

		/// <summary>
		/// Calculates Camera Position!
		/// </summary>
		Vector3 CameraPosition ()
		{
			return new Vector3 (CamPosXValue (), CamPosYValue (), CamPosZValue ());
		}

		/// <summary>
		/// Calculates Mouse Position!
		/// </summary>
		Vector3 MousePosition ()
		{
			return myCamera.ScreenToWorldPoint (Input.mousePosition);
		}

		/// <summary>
		/// Used by Dragging to calculate position!
		/// </summary>
		Vector3 GetOrigin ()
		{
			return new Vector3 (MousePosition ().x, CamPosYValue (), MousePosition ().z);
		}

		/// <summary>
		/// Used by Dragging to calculate position!
		/// </summary>
		Vector3 GetDifference ()
		{
			return GetOrigin () - CameraPosition ();
		}

		/// <summary>
		/// Use FixedUpdate for Smoothness!
		/// </summary>
		public virtual void Update ()
		{
			//Don't Continue if Time is paused!
			if (Time.deltaTime == 0f)
				return;
			//Don't Continue if Camera is Missing!
			if (myCamera == null)
				return;

			//INPUTS!
			//Is Windows Build
			switch (InputSystem) {
			case InputType.Windows:
				{
					
					//RESET CAMERA TO START POSITION!
					//Reset Camera position On Right Click
					//if (Input.GetKeyDown (CameraResetKey))
					//	myCamera.transform.position = ResetCamera;
					

					//ZOOMING!
					//Wait for Inputs and Zoom the Camera Up/Down!
					if (Input.GetAxis (MouseScrollWheelInputName) > 0f || Input.GetKeyDown (ZoomInKey)) {
						if (myCamera.orthographic)
							myCamera.orthographicSize -= (CamSpeedValue () / CamHeightSpeedMulti) * Time.deltaTime;
						else
							myCamera.fieldOfView -= (CamSpeedValue () / CamHeightSpeedMulti) * Time.deltaTime;
					} else if (Input.GetAxis (MouseScrollWheelInputName) < 0f || Input.GetKeyDown (ZoomOutKey)) {
						if (myCamera.orthographic)
							myCamera.orthographicSize += (CamSpeedValue () / CamHeightSpeedMulti) * Time.deltaTime;
						else
							myCamera.fieldOfView += (CamSpeedValue () / CamHeightSpeedMulti) * Time.deltaTime;
					}

					if (MoveMode != MovementMode.Target_Follow) {
						
						//CAMERA LIMITS!
						//Set Min/Max Camera Positions Vertical!
						if (myCamera.transform.position.z >= CamVerticalMax)
							myCamera.transform.position = new Vector3 (CamPosXValue (), CamPosYValue (), CamVerticalMax);
						if (myCamera.transform.position.z <= CamVerticalMin)
							myCamera.transform.position = new Vector3 (CamPosXValue (), CamPosYValue (), CamVerticalMin);
					
						//Set Min/Max Camera Positions Horizontal!
						if (myCamera.transform.position.x >= CamHorizontalMax)
							myCamera.transform.position = new Vector3 (CamHorizontalMax, CamPosYValue (), CamPosZValue ());
						if (myCamera.transform.position.x <= CamHorizontalMin)
							myCamera.transform.position = new Vector3 (CamHorizontalMin, CamPosYValue (), CamPosZValue ());
					
						//Set Camera pos y limit
						if (myCamera.transform.position.y >= CamPosYValue ())
							myCamera.transform.position = new Vector3 (CamPosXValue (), CamPosYValue (), CamPosZValue ());
						if (myCamera.transform.position.y <= CamPosYValue ())
							myCamera.transform.position = new Vector3 (CamPosXValue (), CamPosYValue (), CamPosZValue ());
					}
					
					//Set Min/Max Camera Zoom limits!
					if (myCamera.orthographic) {
						
						//Set Size
						if (myCamera.orthographicSize >= CamZoomMax)
							myCamera.orthographicSize = CamZoomMax;
						if (myCamera.orthographicSize <= CamZoomMin)
							myCamera.orthographicSize = CamZoomMin;
						
						//Set Far / Near Planes
						if (myCamera.nearClipPlane != OrthNearPlane)
							myCamera.nearClipPlane = OrthNearPlane;
						if (myCamera.farClipPlane != OrthFarPlane)
							myCamera.farClipPlane = OrthFarPlane;
						
					} else {
						
						//Set FOV
						if (myCamera.fieldOfView >= CamFOVMax)
							myCamera.fieldOfView = CamFOVMax;
						if (myCamera.fieldOfView <= CamFOVMin)
							myCamera.fieldOfView = CamFOVMin;
						
						//Set Far / Near Planes
						if (myCamera.nearClipPlane != PersNearPlane)
							myCamera.nearClipPlane = PersNearPlane;
						if (myCamera.farClipPlane != PersFarPlane)
							myCamera.farClipPlane = PersFarPlane;
					}

					switch (MoveMode) {
					case MovementMode.Keyboard:
						{
							//USING KEYBOARD TO MOVE!
							//Wait for Inputs and Move the Camera Vertical!
							if (Input.GetAxis ("Vertical") > 0f)
								myCamera.transform.position += Vector3.forward * CamSpeedValue () * Time.deltaTime;
							else if (Input.GetAxis ("Vertical") < 0f)
								myCamera.transform.position += Vector3.back * CamSpeedValue () * Time.deltaTime;

							//Wait for Inputs and Move the Camera Horizontal!
							if (Input.GetAxis ("Horizontal") > 0f)
								myCamera.transform.position += Vector3.right * CamSpeedValue () * Time.deltaTime;
							else if (Input.GetAxis ("Horizontal") < 0f)
								myCamera.transform.position += Vector3.left * CamSpeedValue () * Time.deltaTime;
						}
						break;
					case MovementMode.ClickDrag:
						{
							//USING MOUSE CLICK DRAG TO MOVE!
							if (Input.GetKeyDown (CameraDragKey)) {
								CamOrigin = GetOrigin ();
							}
							if (Input.GetKey (CameraDragKey)) {
								Difference = GetDifference ();
								myCamera.transform.position = CamOrigin - Difference;
							}
						}
						break;
					case MovementMode.Keyboard_and_ClickDrag:
						{
							//USING KEYBOARD TO MOVE!
							//Wait for Inputs and Move the Camera Vertical!
							if (Input.GetAxis ("Vertical") > 0f)
								myCamera.transform.position += Vector3.forward * CamSpeedValue () * Time.deltaTime;
							else if (Input.GetAxis ("Vertical") < 0f)
								myCamera.transform.position += Vector3.back * CamSpeedValue () * Time.deltaTime;

							//Wait for Inputs and Move the Camera Horizontal!
							if (Input.GetAxis ("Horizontal") > 0f)
								myCamera.transform.position += Vector3.right * CamSpeedValue () * Time.deltaTime;
							else if (Input.GetAxis ("Horizontal") < 0f)
								myCamera.transform.position += Vector3.left * CamSpeedValue () * Time.deltaTime;

							//USING MOUSE CLICK DRAG TO MOVE!
							if (Input.GetKeyDown (CameraDragKey)) {
								CamOrigin = GetOrigin ();
							}
							if (Input.GetKey (CameraDragKey)) {
								Difference = GetDifference ();
								myCamera.transform.position = CamOrigin - Difference;
							}
						}
						break;
					case MovementMode.Target_Follow:
						{
							Vector3 targetPos = TargetToFollow.position + TargetOffset;
							Vector3 smoothPos = Vector3.Lerp (myCamera.transform.position, targetPos, CamSpeed);
							myCamera.transform.position = smoothPos;

						}
						break;
					}
				}
				break;
			}
		}
	}
}