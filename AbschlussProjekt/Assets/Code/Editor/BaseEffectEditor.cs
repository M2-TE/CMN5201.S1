using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseEffect))]
[CanEditMultipleObjects]
public class BaseEffectEditor : Editor
{
	#region Variables
	#region Animation
	SerializedProperty looping;
	SerializedProperty sprites;
	SerializedProperty initialFrameOffset;
	#endregion
	
	#region Flickering
	SerializedProperty flickeringEnabled;
	
	SerializedProperty lightRangeCurve;
	SerializedProperty overrideLight;
	SerializedProperty flickeringIntensity;
	SerializedProperty maxFlickerStep;
	SerializedProperty maxFlickerMod;
	#endregion

	#region Color Changing
	SerializedProperty changingLightColors;
	
	SerializedProperty rCurve;
	SerializedProperty gCurve;
	SerializedProperty bCurve;
	SerializedProperty overrideAlpha;
	SerializedProperty colorIntensity;
	#endregion
	#endregion

	#region On Enable
	private void OnEnable() { SetupProperties(); }

	protected virtual void SetupProperties()
	{
		#region Animation
		looping = serializedObject.FindProperty("looping");
		sprites = serializedObject.FindProperty("sprites");
		initialFrameOffset = serializedObject.FindProperty("initialFrameOffset");
		#endregion

		#region Flickering
		flickeringEnabled = serializedObject.FindProperty("flickeringEnabled");
		
		lightRangeCurve = serializedObject.FindProperty("lightRangeCurve");
		overrideLight = serializedObject.FindProperty("overrideLight");
		flickeringIntensity = serializedObject.FindProperty("flickeringIntensity");
		maxFlickerStep = serializedObject.FindProperty("maxFlickerStep");
		maxFlickerMod = serializedObject.FindProperty("maxFlickerMod");
		#endregion

		#region Color Changing
		changingLightColors = serializedObject.FindProperty("changingLightColors");
		
		rCurve = serializedObject.FindProperty("rCurve");
		gCurve = serializedObject.FindProperty("gCurve");
		bCurve = serializedObject.FindProperty("bCurve");
		overrideAlpha = serializedObject.FindProperty("overrideAlpha");
		colorIntensity = serializedObject.FindProperty("colorIntensity");
		#endregion
	}
	#endregion

	#region On Inspector GUI
	public override void OnInspectorGUI()
	{
		OnInspectorGUIStart();
	
		HandleEffectProperties();

		OnInspectorGUIEnd();
	}

	protected void OnInspectorGUIStart()
	{
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();
	}

	protected void OnInspectorGUIEnd()
	{
		if (EditorGUI.EndChangeCheck()) Undo.RecordObject(target, "Effect Prop Changed");
		serializedObject.ApplyModifiedProperties();
	}
	#endregion

	#region Base Effect Property Handler
	protected virtual void HandleEffectProperties()
	{
		HandleAnimProperties();
		HandleDynamicProperties();
	}

	private void HandleAnimProperties()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Base", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(looping);
		EditorGUILayout.PropertyField(sprites, true);
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
			
			if (GUILayout.Button("Generate Color Curves"))
				((BaseEffect)target).PreCalcLightRangeCurve();

			EditorGUILayout.PropertyField(lightRangeCurve);
			EditorGUILayout.PropertyField(overrideLight);
			EditorGUILayout.Slider(flickeringIntensity, 0f, 1f);
			EditorGUILayout.PropertyField(maxFlickerStep);
			EditorGUILayout.PropertyField(maxFlickerMod);
		}
	}

	private void HandleColorChangingProperties()
	{
		if (changingLightColors.boolValue)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Lighting Color", EditorStyles.boldLabel);

			if (GUILayout.Button("Generate Color Curves"))
				((BaseEffect)target).PreCalcFrameColors();

			EditorGUILayout.PropertyField(rCurve);
			EditorGUILayout.PropertyField(gCurve);
			EditorGUILayout.PropertyField(bCurve);
			EditorGUILayout.PropertyField(overrideAlpha);
			EditorGUILayout.Slider(colorIntensity, 0f, 1f);
		}
	}
	#endregion
}