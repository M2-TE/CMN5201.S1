using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Chest))]
[CanEditMultipleObjects]
public class ChestEditor : Editor
{
    private Chest chest;

    #region References
    SerializedProperty customChest;
    SerializedProperty completeRandomness;

    SerializedProperty selectedPools;
    SerializedProperty probability;

    SerializedProperty allItems;

    SerializedProperty minItems;
    SerializedProperty maxItems;

    SerializedProperty items;
    #endregion

    private void OnEnable()
    {
        DrawReferences();
    }

    private void DrawReferences()
    {
        customChest = serializedObject.FindProperty("CustomChest");
        completeRandomness = serializedObject.FindProperty("CompleteRandomness");

        allItems = serializedObject.FindProperty("AllItems");

        minItems = serializedObject.FindProperty("minItems");
        maxItems = serializedObject.FindProperty("maxItems");

        items = serializedObject.FindProperty("Items");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        BaseProperties();
        serializedObject.ApplyModifiedProperties();
    }

    private void BaseProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Base Properties:", EditorStyles.boldLabel);
        customChest.boolValue = EditorGUILayout.ToggleLeft(customChest.displayName, customChest.boolValue);

        if (customChest.boolValue)
            CustomChestProperties();
        else
            RandomChestProperties();
    }

    private void CustomChestProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Custom Chest Properties:", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(items);
    }

    private void RandomChestProperties()
    {
        chest = target as Chest;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Random Chest Properties:", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(allItems);
        if (chest.AllItems)
        {
            EditorGUILayout.LabelField("Minimum items in chest:");
            minItems.intValue = EditorGUILayout.IntSlider(minItems.intValue, 1, maxItems.intValue);
            EditorGUILayout.LabelField("Maximum items in chest:");
            maxItems.intValue = EditorGUILayout.IntSlider(maxItems.intValue, minItems.intValue, 9);

            completeRandomness.boolValue = EditorGUILayout.ToggleLeft(completeRandomness.displayName, completeRandomness.boolValue);

            if (!completeRandomness.boolValue)
            {
                EditorGUILayout.LabelField("Pool Selection:", EditorStyles.boldLabel);
                if (chest.SelectedPools == null || chest.SelectedPools.Length != chest.AllItems.m_Pools.Length)
                    InstantiateSelectionArrays();

                for (int i = 0; i < chest.SelectedPools.Length; i++)
                {
                    EditorGUILayout.Space();
                    chest.SelectedPools[i] = EditorGUILayout.ToggleLeft(new GUIContent(chest.AllItems.m_Pools[i].poolName), chest.SelectedPools[i]);
                    if(chest.SelectedPools[i])
                        chest.Probability[i] = EditorGUILayout.IntSlider(chest.Probability[i], 1, 10);
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("SELECT A ITEMPOOL DATACONTAINER!", EditorStyles.helpBox);
        }
    }

    private void InstantiateSelectionArrays()
    {
        chest.SelectedPools = new bool[chest.AllItems.m_Pools.Length];
        chest.Probability = new int[chest.AllItems.m_Pools.Length];
    }
}
