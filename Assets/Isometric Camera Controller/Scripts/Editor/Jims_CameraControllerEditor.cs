// Jims_CameraControllerEditor.cs - By Jimbob Games 2018.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;


namespace Jims.InputSystem
{
	[CustomEditor (typeof(Jims_CameraController))]
	public class Jims_CameraControllerEditor : Editor
	{
		GUIStyle boxStyle;
		GUIStyle boxStyle2;

		Jims_CameraController myTarget;

		Texture LogoTexture;

		SerializedObject serializedObj;

		SerializedProperty serializedPropertyInputSystem;
		SerializedProperty serializedPropertyMoveMode;
		SerializedProperty serializedPropertyZoomIn;
		SerializedProperty serializedPropertyZoomOut;
		SerializedProperty serializedPropertyDragKey;
		SerializedProperty serializedPropertyResetKey;
		SerializedProperty TargetToFollow;
		SerializedProperty TargetOffset;


		void OnEnable ()
		{
			myTarget = (Jims_CameraController)target;

			serializedObj = new SerializedObject (myTarget);

			serializedPropertyInputSystem = serializedObj.FindProperty ("InputSystem");
			serializedPropertyMoveMode = serializedObj.FindProperty ("MoveMode");
			serializedPropertyZoomIn = serializedObj.FindProperty ("ZoomInKey");
			serializedPropertyZoomOut = serializedObj.FindProperty ("ZoomOutKey");
			serializedPropertyDragKey = serializedObj.FindProperty ("CameraDragKey");
			serializedPropertyResetKey = serializedObj.FindProperty ("CameraResetKey");

			TargetToFollow = serializedObj.FindProperty ("TargetToFollow");
			TargetOffset = serializedObj.FindProperty ("TargetOffset");

			LogoTexture = Resources.Load ("Art/CameraControllerLogo") as Texture;
		}

		public override void OnInspectorGUI ()
		{
			myTarget = (Jims_CameraController)target;
			#if UNITY_5_6_OR_NEWER
			serializedObj.UpdateIfRequiredOrScript ();
			#else
			serializedObj.UpdateIfDirtyOrScript ();
			#endif

			//Set up the box style
			if (boxStyle == null) {
				boxStyle = new GUIStyle (GUI.skin.box);
				boxStyle.normal.textColor = GUI.skin.label.normal.textColor;
				boxStyle.fontStyle = FontStyle.Bold;
				boxStyle.alignment = TextAnchor.UpperLeft;
			}

			if (boxStyle2 == null) {
				boxStyle2 = new GUIStyle (GUI.skin.label);
				boxStyle2.normal.textColor = GUI.skin.label.normal.textColor;
				boxStyle2.fontStyle = FontStyle.Bold;
				boxStyle2.alignment = TextAnchor.UpperLeft;			
			}

			// Begin
			GUILayout.BeginVertical ("", boxStyle);
			GUILayout.Space (10);

			//
			GUILayout.BeginVertical ("", boxStyle2);
			GUILayout.Label (LogoTexture, GUILayout.Width (350), GUILayout.Height (150));
			if (LogoTexture == null)
				EditorGUILayout.LabelField ("CAMERA CONTROLLER!", EditorStyles.boldLabel);
			EditorGUILayout.EndVertical ();
			//

			GUILayout.BeginHorizontal ("", boxStyle2);
			GUILayout.Space (15);
			EditorGUILayout.LabelField ("Camera Profile", GUILayout.Width (150), GUILayout.Height (15));
			myTarget.Profile = EditorGUILayout.ObjectField (myTarget.Profile, typeof(Jims.Profile.Jims_CameraControllerProfile), true) as Jims.Profile.Jims_CameraControllerProfile;
			GUILayout.EndHorizontal ();

			if (myTarget.showHelp) {
				if (!myTarget.Profile)
					EditorGUILayout.HelpBox ("To create a new Camera Profile, (select 'Assets / Create / Camera Profile')!", MessageType.Info, true);
				GUILayout.Space (15);
			}
			if (myTarget.Profile) {
				// 
				GUILayout.BeginVertical ("", boxStyle2);
				GUILayout.BeginHorizontal ("", boxStyle2);
				GUILayout.Space (15);
				if (GUILayout.Button ("Load From Profile!", GUILayout.Width (150), GUILayout.Height (20)))
					myTarget.LoadFromProfile ();
				if (GUILayout.Button ("Save To Profile!", GUILayout.Width (150), GUILayout.Height (20)))
					myTarget.SaveToProfile ();		
				GUILayout.EndHorizontal ();
				GUILayout.Space (15);
				EditorGUILayout.EndVertical ();
			}
			if (myTarget.showHelp) {
				if (myTarget.Profile)
					EditorGUILayout.HelpBox ("You can Save Settings made back to the Profile, or you can Load from Profile. If a Profile is used then it will Auto Load at Start!", MessageType.Info, true);
				GUILayout.Space (15);
			}

			// 
			GUILayout.BeginVertical ("", boxStyle2);
			myTarget.showInputSettings = EditorGUILayout.BeginToggleGroup ("Show Input Settings!", myTarget.showInputSettings);

			if (myTarget.showInputSettings) {				
				GUILayout.BeginVertical ("", boxStyle);
				GUILayout.Space (15);

				EditorGUILayout.PropertyField (serializedPropertyInputSystem, true);
				EditorGUILayout.PropertyField (serializedPropertyMoveMode, true);
				GUILayout.Space (15);

				if (myTarget.MoveMode == Jims_CameraController.MovementMode.Target_Follow) {
					EditorGUILayout.PropertyField (TargetToFollow, true);
					EditorGUILayout.PropertyField (TargetOffset, true);
					GUILayout.Space (15);
				}

				myTarget.MouseScrollWheelInputName = EditorGUILayout.TextField ("Mouse Scroll Wheel Input Name", myTarget.MouseScrollWheelInputName);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Mouse Scroll Wheel Input Name!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				EditorGUILayout.PropertyField (serializedPropertyZoomIn, true);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Zoom In Input Key!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				EditorGUILayout.PropertyField (serializedPropertyZoomOut, true);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Zoom Out Input Key!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				EditorGUILayout.PropertyField (serializedPropertyDragKey, true);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Drag Camera Key!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				EditorGUILayout.PropertyField (serializedPropertyResetKey, true);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Reset Camera Key!", MessageType.Info, true);
				}
				GUILayout.Space (15);	
				EditorGUILayout.EndVertical ();
			}

			EditorGUILayout.EndToggleGroup ();
			EditorGUILayout.EndVertical ();
			//

			// 
			GUILayout.BeginVertical ("", boxStyle2);
			myTarget.showLimitSettings = EditorGUILayout.BeginToggleGroup ("Show Camera Limit Settings!", myTarget.showLimitSettings);

			if (myTarget.showLimitSettings) {			
				GUILayout.BeginVertical ("", boxStyle);
				GUILayout.Space (15);

				myTarget.CamHeight = EditorGUILayout.Slider ("Height", myTarget.CamHeight, 0f, 1000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set Camera default height!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamSpeed = EditorGUILayout.Slider ("Speed", myTarget.CamSpeed, 0.1f, 2f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set camera move speed multi!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamHeightSpeedMulti = EditorGUILayout.Slider ("Height/Speed Multiplier", myTarget.CamHeightSpeedMulti, 0.1f, 10f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Multipliy 'CamSpeed' by %!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				GUILayout.Space (25);

				if (myTarget.myCamera != null) {
					if (myTarget.myCamera.orthographic) {				

						myTarget.CamZoomMin = EditorGUILayout.Slider ("Zoom Min", myTarget.CamZoomMin, 0f, 100f);
						if (myTarget.showHelp) {
							EditorGUILayout.HelpBox ("If Camera is orthographic, set size value!", MessageType.Info, true);
							GUILayout.Space (10);
						}

						myTarget.CamZoomMax = EditorGUILayout.Slider ("Zoom Max", myTarget.CamZoomMax, 0f, 150f);
						if (myTarget.showHelp) {
							EditorGUILayout.HelpBox ("If Camera is orthographic, set size value", MessageType.Info, true);
							GUILayout.Space (10);
						}

						myTarget.OrthNearPlane = EditorGUILayout.Slider ("Orth Near Plane", myTarget.OrthNearPlane, -1000f, 1000f);
						if (myTarget.showHelp) {
							EditorGUILayout.HelpBox ("If Camera is orthographic, set Near Plane value!", MessageType.Info, true);
							GUILayout.Space (10);
						}

						myTarget.OrthFarPlane = EditorGUILayout.Slider ("Orth Far Plane", myTarget.OrthFarPlane, -1000f, 1000f);
						if (myTarget.showHelp) {
							EditorGUILayout.HelpBox ("If Camera is orthographic, set Far Plane value!", MessageType.Info, true);
							GUILayout.Space (10);
						}

					} else {

						GUILayout.Space (25);

						myTarget.CamFOVMin = EditorGUILayout.Slider ("FOV Min", myTarget.CamFOVMin, 1f, 120f);
						if (myTarget.showHelp) {
							EditorGUILayout.HelpBox ("If Camera is Perspective, set FOV value!", MessageType.Info, true);
							GUILayout.Space (10);
						}

						myTarget.CamFOVMax = EditorGUILayout.Slider ("FOV Max", myTarget.CamFOVMax, 1f, 120f);
						if (myTarget.showHelp) {
							EditorGUILayout.HelpBox ("If Camera is Perspective, set FOV value!", MessageType.Info, true);
							GUILayout.Space (10);
						}

						myTarget.PersNearPlane = EditorGUILayout.Slider ("Pers Near Plane", myTarget.PersNearPlane, -1000f, 1000f);
						if (myTarget.showHelp) {
							EditorGUILayout.HelpBox ("If Camera is Perspective, set Near Plane value!", MessageType.Info, true);
							GUILayout.Space (10);
						}

						myTarget.PersFarPlane = EditorGUILayout.Slider ("Pers Far Plane", myTarget.PersFarPlane, -1000f, 1000f);
						if (myTarget.showHelp) {
							EditorGUILayout.HelpBox ("If Camera is Perspective, set Far Plane value!", MessageType.Info, true);
							GUILayout.Space (10);
						}
					}
				}

				GUILayout.Space (25);

				myTarget.CamVerticalMax = EditorGUILayout.Slider ("Forward Max", myTarget.CamVerticalMax, -1000f, 1000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set forwards limit!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamVerticalMin = EditorGUILayout.Slider ("Back Min", myTarget.CamVerticalMin, -1000f, 1000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set backwards limit!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamHorizontalMin = EditorGUILayout.Slider ("Left Min", myTarget.CamHorizontalMin, -1000f, 1000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set left limit!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamHorizontalMax = EditorGUILayout.Slider ("Right Max", myTarget.CamHorizontalMax, -1000f, 1000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set right limit!", MessageType.Info, true);
				}
				GUILayout.Space (15);
				EditorGUILayout.EndVertical ();
			}

			EditorGUILayout.EndToggleGroup ();
			EditorGUILayout.EndVertical ();
			//

			GUILayout.BeginHorizontal ("", boxStyle2);
			GUILayout.Space (15);
			EditorGUILayout.LabelField ("Show Help?", GUILayout.Width (150), GUILayout.Height (15));
			myTarget.showHelp = EditorGUILayout.Toggle (myTarget.showHelp);
			GUILayout.EndHorizontal ();
			GUILayout.Space (15);

			serializedObj.ApplyModifiedProperties ();

			// END
			EditorGUILayout.EndVertical ();
			EditorUtility.SetDirty (target);
		}
	}
}
