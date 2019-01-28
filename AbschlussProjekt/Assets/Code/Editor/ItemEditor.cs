using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    #region Variables
    // Monobehaviour
    SerializedProperty inventory;
    SerializedProperty itemContainer;
    SerializedProperty itemSize;
    SerializedProperty currentlyStacked;

    #region Currently Not Used
    /*
    // DataContainer
    SerializedProperty itemName;
    SerializedProperty itemIcon;
    SerializedProperty itemDescription;

    SerializedProperty encumbranceValue;
    SerializedProperty levelRequirement;

    SerializedProperty isIndestructible;
    SerializedProperty isQuestItem;
    SerializedProperty isUnique;

    SerializedProperty stackingLimit;

    // Equipment related
    SerializedProperty matchingClasses;
    SerializedProperty equipmentType;

    SerializedProperty attackBonus;

    SerializedProperty defenseBonus;

    // Misc related
    SerializedProperty isUsable;

    SerializedProperty destroyOnUse;
    SerializedProperty canHitEnemies;
    SerializedProperty canHitAllies;
    SerializedProperty cooldown;
    SerializedProperty minRange;
    SerializedProperty maxRange;
    SerializedProperty surroundingAffectedUnits;
    SerializedProperty consumeEffects;
    */
    #endregion

    #endregion

    #region VariableLinks

    private void OnEnable()
    {
        VariableLinksMonoBehaviour();
        //VariableLinksDataContainer();
    }

    private void VariableLinksMonoBehaviour()
    {

        inventory = serializedObject.FindProperty("inventory");
        itemContainer = serializedObject.FindProperty("container");
        itemSize = serializedObject.FindProperty("itemSize");
        currentlyStacked = serializedObject.FindProperty("currentlyStacked");

    }

    #region Currently Not Used
    /*
    private void VariableLinksDataContainer()
    {
        itemName = serializedObject.FindProperty("ItemName");
        itemIcon = serializedObject.FindProperty("ItemIcon");
        itemDescription = serializedObject.FindProperty("ItemDescription");

        encumbranceValue = serializedObject.FindProperty("EncumbranceValue");
        levelRequirement = serializedObject.FindProperty("LevelRequirement");

        isIndestructible = serializedObject.FindProperty("IsIndestructible");
        isQuestItem = serializedObject.FindProperty("IsQuestItem");
        isUnique = serializedObject.FindProperty("IsUnique");

        stackingLimit = serializedObject.FindProperty("StackingLimit");
    }

    private void VariableLinksEquipmentContainer()
    {
        matchingClasses = serializedObject.FindProperty("MatchingClasses");
        equipmentType = serializedObject.FindProperty("EquipmentType");
    }

    private void VariableLinksWeaponContainer()
    {
        attackBonus = serializedObject.FindProperty("AttackBonus");
    }

    private void VariableLinksArmorContainer()
    {
        defenseBonus = serializedObject.FindProperty("DefenseBonus");
    }

    private void VariableLinksMiscContainer()
    {
        isUsable = serializedObject.FindProperty("IsUsable");

        destroyOnUse = serializedObject.FindProperty("DestroyOnUse");
        canHitEnemies = serializedObject.FindProperty("CanHitEnemies");
        canHitAllies = serializedObject.FindProperty("CanHitAllies");
        cooldown = serializedObject.FindProperty("Cooldown");
        minRange = serializedObject.FindProperty("MinRange");
        maxRange = serializedObject.FindProperty("MaxRange");
        surroundingAffectedUnits = serializedObject.FindProperty("SurroundingAffectedUnits");
        consumeEffects = serializedObject.FindProperty("ConsumeEffects");
    }
    */
    #endregion

    #endregion

    #region InspectorGUI
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        //base.OnInspectorGUI();

        PropertyHandling();

        if (EditorGUI.EndChangeCheck())
            Undo.RecordObject(target, "");
        serializedObject.ApplyModifiedProperties();
    }

    private void PropertyHandling()
    {
        MonoBehaviourHandling();
        //DataContainerHandling();
    }

    private void MonoBehaviourHandling()
    {
        EditorGUILayout.PropertyField(inventory);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("MonoBehaviour", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(itemContainer);
        EditorGUILayout.Slider(itemSize, 2f, 4f);
        EditorGUILayout.PropertyField(currentlyStacked);
    }

    #region Currently Not Used
    /*
    private void DataContainerHandling()
    {
        if (itemContainer != null)
        {
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("BaseContainer", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(itemName);
            EditorGUILayout.PropertyField(itemIcon);
            EditorGUILayout.PropertyField(itemDescription); // <........
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(encumbranceValue);
            EditorGUILayout.PropertyField(levelRequirement);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isIndestructible);
            EditorGUILayout.PropertyField(isQuestItem);
            EditorGUILayout.PropertyField(isUnique);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(stackingLimit);
        }
    }
    */
    #endregion

    #endregion
}
