// Jims_CameraControllerProfile.cs - By Jimbob Games 2018.
using UnityEngine;
using System;
using Jims.InputSystem;


namespace Jims.Profile
{	
	[CreateAssetMenu (fileName = "Camera Profile", menuName = "Camera Profile", order = 150)]
	public partial class Jims_CameraControllerProfile : ScriptableObject
	{		
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
		[Range (0f, 1000f)]
		[Tooltip ("Set Camera default height!")]
		public float CamHeight = 8f;		
		[Range (0.1f, 2f)]
		[Tooltip ("Set camera move speed multi!")]
		public float CamSpeed = 0.3f;		
		[Range (0.1f, 10f)]
		[Tooltip ("Multipliy 'CamSpeed' by %!")]
		public float CamHeightSpeedMulti = 2f;

		[Range (0f, 100f)]
		[Tooltip ("If Camera is orthographic, set size value!")]
		public float CamZoomMin = 6f;		
		[Range (0f, 100f)]
		[Tooltip ("If Camera is orthographic, set size value!")]
		public float CamZoomMax = 10f;
		[Range (-1000f, 1000f)]
		[Tooltip ("If Camera is orthographic, set Near Plane value!")]
		public float OrthNearPlane = -50f;	
		[Range (-1000f, 1000f)]
		[Tooltip ("If Camera is orthographic, set Far Plane value!")]
		public float OrthFarPlane = 50f;

		[Range (1f, 120f)]
		[Tooltip ("If Camera is Perspective, set FOV value!")]
		public float CamFOVMin = 40f;		
		[Range (1f, 120f)]
		[Tooltip ("If Camera is Perspective, set FOV value!")]
		public float CamFOVMax = 60f;
		[Range (-1000f, 1000f)]
		[Tooltip ("If Camera Perspective, set Near Plane value!")]
		public float PersNearPlane = 0.02f;
		[Range (-1000f, 1000f)]
		[Tooltip ("If Camera Perspective, set Far Plane value!")]
		public float PersFarPlane = 100f;

		[Range (-1000f, 1000f)]
		[Tooltip ("Set Backwards limit!")]
		public float CamVerticalMin = -20f;		
		[Range (-1000f, 1000f)]
		[Tooltip ("Set Forwards limit!")]
		public float CamVerticalMax = 65f;

		[Range (-1000f, 1000f)]
		[Tooltip ("Set Left limit!")]
		public float CamHorizontalMin = -20f;		
		[Range (-1000f, 1000f)]
		[Tooltip ("Set Right limit!")]
		public float CamHorizontalMax = 15f;

		#endregion



		/// <summary>
		/// Apply Profile Settings to Camera!
		/// </summary>
		public void Load (Jims_CameraController Cam)
		{
			switch (InputSystem) {
			case InputType.Windows:
				{
					Cam.InputSystem = Jims_CameraController.InputType.Windows;
				}
				break;
			}

			switch (MoveMode) {
			case MovementMode.Keyboard:
				{
					Cam.MoveMode = Jims_CameraController.MovementMode.Keyboard;
				}
				break;
			case MovementMode.ClickDrag:
				{
					Cam.MoveMode = Jims_CameraController.MovementMode.ClickDrag;
				}
				break;
			case MovementMode.Keyboard_and_ClickDrag:
				{
					Cam.MoveMode = Jims_CameraController.MovementMode.Keyboard_and_ClickDrag;
				}
				break;
			case MovementMode.Target_Follow:
				{
					Cam.MoveMode = Jims_CameraController.MovementMode.Target_Follow;
				}
				break;
			}

			Cam.TargetOffset = TargetOffset;
			Cam.MouseScrollWheelInputName = MouseScrollWheelInputName;
			Cam.ZoomInKey = ZoomInKey;
			Cam.ZoomOutKey = ZoomOutKey;
			Cam.CameraDragKey = CameraDragKey;
			Cam.CameraResetKey = CameraResetKey;

			Cam.CamHeight = CamHeight;
			Cam.CamSpeed = CamSpeed;
			Cam.CamHeightSpeedMulti = CamHeightSpeedMulti;

			Cam.CamZoomMin = CamZoomMin;
			Cam.CamZoomMax = CamZoomMax;
			Cam.OrthNearPlane = -50;
			Cam.OrthFarPlane = OrthFarPlane;

			Cam.CamFOVMin = CamFOVMin;
			Cam.CamFOVMax = CamFOVMax;
			Cam.PersNearPlane = PersNearPlane;
			Cam.PersFarPlane = PersFarPlane;

			Cam.CamVerticalMin = CamVerticalMin;
			Cam.CamVerticalMax = 107;
			Cam.CamHorizontalMin = -700;
			Cam.CamHorizontalMax = -225;
		}

		/// <summary>
		/// Replace Profile Settings with Camera Settings
		/// </summary>
		public void Save (Jims_CameraController Cam)
		{
			switch (Cam.InputSystem) {
			case Jims_CameraController.InputType.Windows:
				{
					InputSystem = Jims_CameraControllerProfile.InputType.Windows;
				}
				break;
			}

			switch (Cam.MoveMode) {
			case Jims_CameraController.MovementMode.Keyboard:
				{
					MoveMode = Jims_CameraControllerProfile.MovementMode.Keyboard;
				}
				break;
			case Jims_CameraController.MovementMode.ClickDrag:
				{
					MoveMode = Jims_CameraControllerProfile.MovementMode.ClickDrag;
				}
				break;
			case Jims_CameraController.MovementMode.Keyboard_and_ClickDrag:
				{
					MoveMode = Jims_CameraControllerProfile.MovementMode.Keyboard_and_ClickDrag;
				}
				break;
			case Jims_CameraController.MovementMode.Target_Follow:
				{
					MoveMode = Jims_CameraControllerProfile.MovementMode.Target_Follow;
				}
				break;
			}

			TargetOffset = Cam.TargetOffset;

			MouseScrollWheelInputName = Cam.MouseScrollWheelInputName;
			ZoomInKey = Cam.ZoomInKey;
			ZoomOutKey = Cam.ZoomOutKey;
			CameraDragKey = Cam.CameraDragKey;
			CameraResetKey = Cam.CameraResetKey;

			CamHeight = Cam.CamHeight;
			CamSpeed = Cam.CamSpeed;
			CamHeightSpeedMulti = Cam.CamHeightSpeedMulti;

			CamZoomMin = Cam.CamZoomMin;
			CamZoomMax = Cam.CamZoomMax;
			OrthNearPlane = Cam.OrthNearPlane;
			OrthFarPlane = Cam.OrthFarPlane;

			CamFOVMin = Cam.CamFOVMin;
			CamFOVMax = Cam.CamFOVMax;
			PersNearPlane = Cam.PersNearPlane;
			PersFarPlane = Cam.PersFarPlane;

			CamVerticalMin = Cam.CamVerticalMin;
			CamVerticalMax = Cam.CamVerticalMax;
			CamHorizontalMin = Cam.CamHorizontalMin;
			CamHorizontalMax = Cam.CamHorizontalMax;
		}
	}
}
