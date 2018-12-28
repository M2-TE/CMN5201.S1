using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseEffect), true)]
[CanEditMultipleObjects]
public class BaseEffectEditor : Editor
{
	#region Variables
	#region Animation
	SerializedProperty sprites;
	SerializedProperty initialFrameOffset;
	#endregion

	SerializedProperty flickeringEnabled;
	SerializedProperty changingLightColors;

	#region Flickering
	SerializedProperty preCalcFlickering;
	SerializedProperty lightRangeCurve;
	SerializedProperty overrideLight;
	SerializedProperty flickeringIntensity;
	SerializedProperty maxFlickerStep;
	SerializedProperty maxFlickerMod;
	#endregion

	#region Color Changing
	SerializedProperty preCalcColors;
	SerializedProperty rCurve;
	SerializedProperty gCurve;
	SerializedProperty bCurve;
	SerializedProperty overrideAlpha;
	SerializedProperty colorIntensity;
	#endregion
	#endregion

	void OnEnable()
	{
		#region Animation
		sprites = serializedObject.FindProperty("sprites");
		initialFrameOffset = serializedObject.FindProperty("initialFrameOffset");
		#endregion

		flickeringEnabled = serializedObject.FindProperty("flickeringEnabled");
		changingLightColors = serializedObject.FindProperty("changingLightColors");

		#region Flickering
		preCalcFlickering = serializedObject.FindProperty("preCalcFlickering");
		lightRangeCurve = serializedObject.FindProperty("lightRangeCurve");
		overrideLight = serializedObject.FindProperty("overrideLight");
		flickeringIntensity = serializedObject.FindProperty("flickeringIntensity");
		maxFlickerStep = serializedObject.FindProperty("maxFlickerStep");
		maxFlickerMod = serializedObject.FindProperty("maxFlickerMod");
		#endregion

		#region Color Changing
		preCalcColors = serializedObject.FindProperty("preCalcColors");
		rCurve = serializedObject.FindProperty("rCurve");
		gCurve = serializedObject.FindProperty("gCurve");
		bCurve = serializedObject.FindProperty("bCurve");
		overrideAlpha = serializedObject.FindProperty("overrideAlpha");
		colorIntensity = serializedObject.FindProperty("colorIntensity");
		#endregion
	}

	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		serializedObject.Update();

		HandleAnimProperties();
		HandleDynamicProperties();

		serializedObject.ApplyModifiedProperties();
	}
	
	private void HandleAnimProperties()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(sprites);
		EditorGUILayout.PropertyField(initialFrameOffset);
	}

	private void HandleDynamicProperties()
	{
		EditorGUILayout.Space();
		flickeringEnabled.boolValue = EditorGUILayout.ToggleLeft(flickeringEnabled.displayName, flickeringEnabled.boolValue);
		changingLightColors.boolValue = EditorGUILayout.ToggleLeft(changingLightColors.displayName, changingLightColors.boolValue);

		HandleFlickeringProperties();
		HandleColorChangingProperties();
	}

	private void HandleFlickeringProperties()
	{
		if (flickeringEnabled.boolValue)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Lighting Range", EditorStyles.boldLabel);

			preCalcFlickering.boolValue = true;
			preCalcFlickering.boolValue = EditorGUILayout.ToggleLeft(preCalcFlickering.displayName, preCalcFlickering.boolValue);
			EditorGUILayout.PropertyField(lightRangeCurve);
			EditorGUILayout.PropertyField(overrideLight);
			EditorGUILayout.Slider(flickeringIntensity, 0f, 1f);
			EditorGUILayout.PropertyField(maxFlickerStep);
			EditorGUILayout.PropertyField(maxFlickerMod);
		}
		else
		{
			preCalcFlickering.boolValue = false;
		}
	}

	private void HandleColorChangingProperties()
	{
		if (changingLightColors.boolValue)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Lighting Color", EditorStyles.boldLabel);

			preCalcColors.boolValue = true;
			preCalcColors.boolValue = EditorGUILayout.ToggleLeft(preCalcColors.displayName, preCalcColors.boolValue);
			EditorGUILayout.PropertyField(rCurve);
			EditorGUILayout.PropertyField(gCurve);
			EditorGUILayout.PropertyField(bCurve);
			EditorGUILayout.PropertyField(overrideAlpha);
			EditorGUILayout.Slider(colorIntensity, 0f, 1f);
		}
		else
		{
			preCalcColors.boolValue = false;
		}
	}
}