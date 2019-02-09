using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//[CustomEditor(typeof(ItemPool))]
[CanEditMultipleObjects]
public class ItemPoolEditor : Editor
{
    SerializedProperty pools;

    private void OnEnable()
    {
        pools = serializedObject.FindProperty("pools");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        HandlePoolProperties();

        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(target, "Effect Prop Changed");
        serializedObject.ApplyModifiedProperties();
    }

    private void HandlePoolProperties()
    {
        // WAHHHHHHH Custum editors SUCK balls! i legit worked 6 hours on this and 33 working lines is all i got... this could've been so fancy :(, but this'll do for now. Maybe i'll come back later after i get some advice.
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Item Pools", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(pools.FindPropertyRelative("poolName"));
    }
}
