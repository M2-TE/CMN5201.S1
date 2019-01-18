using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoloEffect))]
[CanEditMultipleObjects]
public class SoloEffectEditor : BaseEffectEditor
{
	#region Properties
	private SerializedProperty initialAnimationDelay;
	private SerializedProperty initialAudioDelay;
	private SerializedProperty lingeringDuration;
	private SerializedProperty framerateOverride;
	private SerializedProperty waitForAudio;
	#endregion

	void OnEnable()
	{
		base.SetupProperties();
		initialAnimationDelay = serializedObject.FindProperty("initialAnimationDelay");
		initialAudioDelay = serializedObject.FindProperty("initialAudioDelay");
		lingeringDuration = serializedObject.FindProperty("lingeringDuration");
		framerateOverride = serializedObject.FindProperty("framerateOverride");
		waitForAudio = serializedObject.FindProperty("waitForAudio");
	}

	public override void OnInspectorGUI()
	{
		OnInspectorGUIStart();

		HandleEffectProperties();

		OnInspectorGUIEnd();
	}

	protected override void HandleEffectProperties()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Solo Effect Specific", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(initialAnimationDelay);
		EditorGUILayout.PropertyField(initialAudioDelay);
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(lingeringDuration);
		EditorGUILayout.PropertyField(framerateOverride);
		EditorGUILayout.PropertyField(waitForAudio);

		base.HandleEffectProperties();
	}
}
