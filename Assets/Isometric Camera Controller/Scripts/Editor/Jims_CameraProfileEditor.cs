// Jims_CameraProfileEditor.cs - By Jimbob Games 2018.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;


namespace Jims.Profile
{
	[CustomEditor (typeof (Jims_CameraControllerProfile))]
	public class Jims_CameraProfileEditor : Editor
	{
		GUIStyle boxStyle;
		GUIStyle boxStyle2;

		Jims_CameraControllerProfile myTarget;

		Texture LogoTexture;

		SerializedObject serializedObj;

		SerializedProperty serializedPropertyInputSystem;
		SerializedProperty serializedPropertyMoveMode;
		SerializedProperty serializedPropertyZoomIn;
		SerializedProperty serializedPropertyZoomOut;
		SerializedProperty serializedPropertyDragKey;
		SerializedProperty serializedPropertyResetKey;

		SerializedProperty TargetOffset;


		void OnEnable ()
		{
			myTarget = (Jims_CameraControllerProfile) target;

			serializedObj = new SerializedObject (myTarget);

			serializedPropertyInputSystem = serializedObj.FindProperty ("InputSystem");
			serializedPropertyMoveMode = serializedObj.FindProperty ("MoveMode");
			serializedPropertyZoomIn = serializedObj.FindProperty ("ZoomInKey");
			serializedPropertyZoomOut = serializedObj.FindProperty ("ZoomOutKey");
			serializedPropertyDragKey = serializedObj.FindProperty ("CameraDragKey");
			serializedPropertyResetKey = serializedObj.FindProperty ("CameraResetKey");

			TargetOffset = serializedObj.FindProperty ("TargetOffset");

			LogoTexture = Resources.Load ("Art/CameraProfileLogo") as Texture;
		}

		public override void OnInspectorGUI ()
		{
			myTarget = (Jims_CameraControllerProfile) target;
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
				boxStyle.alignment = TextAnchor.UpperCenter;
			}

			if (boxStyle2 == null) {
				boxStyle2 = new GUIStyle (GUI.skin.label);
				boxStyle2.normal.textColor = GUI.skin.label.normal.textColor;
				boxStyle2.fontStyle = FontStyle.Bold;
				boxStyle2.alignment = TextAnchor.UpperCenter;		
			}

			// Begin
			GUILayout.BeginVertical ("", boxStyle);
			GUILayout.Space (10);

			//
			GUILayout.BeginVertical ("", boxStyle2);
			GUILayout.Label (LogoTexture, GUILayout.Width(350), GUILayout.Height(150));
			if(LogoTexture == null)
				EditorGUILayout.LabelField ("CAMERA PROFILE!", EditorStyles.boldLabel);
			EditorGUILayout.EndVertical ();

			// 
			GUILayout.BeginVertical ("", boxStyle2);
			myTarget.showInputSettings = EditorGUILayout.BeginToggleGroup ("Show Input Settings!", myTarget.showInputSettings);

			if (myTarget.showInputSettings) {				
				GUILayout.BeginVertical ("", boxStyle);
				GUILayout.Space (15);

				EditorGUILayout.PropertyField (serializedPropertyInputSystem, true);
				EditorGUILayout.PropertyField (serializedPropertyMoveMode, true);
				GUILayout.Space (15);

				EditorGUILayout.PropertyField (TargetOffset, true);
				GUILayout.Space (15);

				myTarget.MouseScrollWheelInputName = EditorGUILayout.TextField ("Mouse Scroll Wheel Input Name", myTarget.MouseScrollWheelInputName);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Mouse Scroll Wheel Input Name!", MessageType.Info, true);
					GUILayout.Space (15);
				}

				EditorGUILayout.PropertyField (serializedPropertyZoomIn, true);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Zoom In Input Key!", MessageType.Info, true);
					GUILayout.Space (15);
				}

				EditorGUILayout.PropertyField (serializedPropertyZoomOut, true);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Zoom Out Input Key!", MessageType.Info, true);
					GUILayout.Space (25);
				}

				EditorGUILayout.PropertyField (serializedPropertyDragKey, true);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Drag Camera Key!", MessageType.Info, true);
					GUILayout.Space (15);
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

				myTarget.CamHeight = EditorGUILayout.Slider ("Height", myTarget.CamHeight, 0f, 10000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set Camera default height!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamSpeed = EditorGUILayout.Slider ("Speed", myTarget.CamSpeed, 0.1f, 10f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set camera move speed multi!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamHeightSpeedMulti = EditorGUILayout.Slider ("Height/Speed Multiplier", myTarget.CamHeightSpeedMulti, 0.1f, 100f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Multipliy 'CamSpeed' by %!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				GUILayout.Space (25);

				myTarget.CamZoomMin = EditorGUILayout.Slider ("Zoom Min", myTarget.CamZoomMin, 0f, 1000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("If Camera is orthographic, set size value!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamZoomMax = EditorGUILayout.Slider ("Zoom Max", myTarget.CamZoomMax, 0f, 1000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("If Camera is orthographic, set size value", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.OrthNearPlane = EditorGUILayout.Slider ("Orth Near Plane", myTarget.OrthNearPlane, -10000f, 10000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("If Camera is orthographic, set Near Plane value!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.OrthFarPlane = EditorGUILayout.Slider ("Orth Far Plane", myTarget.OrthFarPlane, -10000f, 10000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("If Camera is orthographic, set Far Plane value!", MessageType.Info, true);
					GUILayout.Space (10);
				}

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

				myTarget.PersNearPlane = EditorGUILayout.Slider ("Pers Near Plane", myTarget.PersNearPlane, -10000f, 10000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("If Camera is Perspective, set Near Plane value!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.PersFarPlane = EditorGUILayout.Slider ("Pers Far Plane", myTarget.PersFarPlane, -10000f, 10000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("If Camera is Perspective, set Far Plane value!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				GUILayout.Space (25);

				myTarget.CamVerticalMax = EditorGUILayout.Slider ("Forward Max", myTarget.CamVerticalMax, -10000f, 10000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set forwards limit!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamVerticalMin = EditorGUILayout.Slider ("Back Min", myTarget.CamVerticalMin, -1000f, 10000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set backwards limit!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamHorizontalMin = EditorGUILayout.Slider ("Left Min", myTarget.CamHorizontalMin, -10000f, 10000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set left limit!", MessageType.Info, true);
					GUILayout.Space (10);
				}

				myTarget.CamHorizontalMax = EditorGUILayout.Slider ("Right Max", myTarget.CamHorizontalMax, -10000f, 10000f);
				if (myTarget.showHelp) {
					EditorGUILayout.HelpBox ("Set right limit!", MessageType.Info, true);
				}
				GUILayout.Space (15);	
				EditorGUILayout.EndVertical ();
			}

			EditorGUILayout.EndToggleGroup ();
			EditorGUILayout.EndVertical ();
			//

			// 
			GUILayout.BeginVertical ("", boxStyle2);
			GUILayout.BeginHorizontal ("", boxStyle2);
			GUILayout.Space (15);
			EditorGUILayout.LabelField ("Show Help?", GUILayout.Width (150), GUILayout.Height (15));
			myTarget.showHelp = EditorGUILayout.Toggle (myTarget.showHelp);
			GUILayout.EndHorizontal ();
			GUILayout.Space (15);
			EditorGUILayout.EndVertical ();

			serializedObj.ApplyModifiedProperties ();

			// END
			EditorGUILayout.EndVertical ();
			EditorUtility.SetDirty (target);
		}
	}
}