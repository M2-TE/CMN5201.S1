using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EquipmentSlot { PRIMARY, SECONDARY, HEAD, CHEST, WAIST, HANDS, FEET, FINGERONE, FINGERTWO, NECK }

public class CharacterInfoPanel : UIPanel
{
    #region UIConnectors
    [SerializeField]
    private TextMeshProUGUI characterName, characterStats, characterSkillInfo;
    [SerializeField]
    private Image characterPortrait;
    [SerializeField]
    private Image[] characterBuffs;
    [SerializeField]
    private SkillInfo[] characterSkills;
    [SerializeField]
    private GameObject InspectorPanel;
    [SerializeField]
    private SkillSelectionPanel skillSelectionPanel;

    private bool inspectorPanel;

    public ItemInfo itemInfo;
    #endregion

    [SerializeField] private GameObject renderCameraPrefab;

    private GameObject RenderCamera;

    private GameObject characterPreviewProxy;

    private Dictionary<EquipmentSlot, UIEquipmentHandler> equipmentSlots  = new Dictionary<EquipmentSlot, UIEquipmentHandler>();

    private Entity currentCharacter;

    public Entity CurrentCharacter => currentCharacter;

    public bool Open => visibilityToggleNode.activeInHierarchy;

    #region Setup
    protected override void Awake()
    {
        (AssetManager.Instance.GetManager<CharacterInfoManager>() ?? new CharacterInfoManager()).CharacterInfoPanel = this;
        base.Awake();
    }
    private void Start()
    {
        InputManager manager = AssetManager.Instance.GetManager<InputManager>();
        manager.AddListener(manager.Input.UI.CharacterInfoClose, ctx => ToggleVisibility(false));

        RenderCamera = Instantiate(renderCameraPrefab, new Vector3(0f, 0f, 1f), Quaternion.identity);

        AssetManager.Instance.GetManager<CharacterInfoManager>().InventoryManager = AssetManager.Instance.GetManager<InventoryManager>();

        skillSelectionPanel.EventSystem = this.EventSystem;
    }
    public void ConnectUIHandler(UIEquipmentHandler handler, EquipmentSlot key)
    {
        equipmentSlots.Add(key, handler);
    }
    public override void ToggleVisibility(bool visibleState)
    {
        base.ToggleVisibility(visibleState);
        if(AssetManager.Instance.GetManager<CharacterInfoManager>().InventoryManager != null)
        {
            AssetManager.Instance.GetManager<CharacterInfoManager>().InventoryManager.InventoryPanel.itemInfo.UpdateAction(true);
        }
        if (visibleState)
            inspectorPanel = true;
    }
    #endregion

    public void OpenCharacterInfo(Entity character)
    {
        currentCharacter = character;
        skillSelectionPanel.CurrentEntity = character;
        DisplayCharacter();
    }

    public void CloseCharacterInfo()
    {
        ToggleVisibility(false);
    }

    #region Displaying Character
    public void DisplayCharacter()
    {
        ToggleVisibility(true);
        characterName.text = currentCharacter.Name;
        characterStats.text = currentCharacter.Stats();

        characterPortrait.sprite = currentCharacter.CharDataContainer.Portrait;

        SwitchInspectionPanel();
        DisplayCharacterPreview();
        skillSelectionPanel.DisplayEquippedSkills();
        DisplayCombatEffects();

        /*EquipmentSlot slot in (EquipmentSlot[]) Enum.GetValues(typeof(EquipmentSlot))*/
        foreach (EquipmentSlot slot in equipmentSlots.Keys)
        {
            DisplayEquipmentSlot(slot);
        }
    }

    public void SwitchInspectionPanel()
    {
        SwitchInspectionPanel(inspectorPanel);
    }

    public void SwitchInspectionPanel(bool inspector)
    {
        skillSelectionPanel.ToggleVisibility(!inspector, this);
        InspectorPanel.SetActive(inspector);
        inspectorPanel = !inspector;
    }

    public void DisplayCharacterPreview()
    {
        if (characterPreviewProxy != null)
            GameObject.Destroy(characterPreviewProxy);

        characterPreviewProxy = GameObject.Instantiate(currentCharacter.CharDataContainer.Prefab, RenderCamera.transform);
		characterPreviewProxy.transform.localPosition = new Vector3(0f, 0f, 1f);
        for (int child = 0; child < characterPreviewProxy.transform.childCount; child++)
        {
            GameObject.Destroy(characterPreviewProxy.transform.GetChild(child).gameObject);
        }
    }

    public void DisplayEquipmentSlot(EquipmentSlot slot)
    {
        if (equipmentSlots.ContainsKey(slot))
        {
            if (currentCharacter.GetEquippedItem(slot) != null)
            {
                equipmentSlots[slot].Icon.sprite = currentCharacter.GetEquippedItem(slot).ItemIcon;
                equipmentSlots[slot].SetEmpty(false);
            }
            else
            {
                equipmentSlots[slot].SetEmpty();
            }
        }
        else
            Debug.Log("An error has occurred.");
    }

    public void DisplayEquipmentInfo(EquipmentSlot slot, bool show)
    {
        EquipmentContainer item = currentCharacter.GetEquippedItem(slot);
        if (show && item != null)
        {
            itemInfo.OpenItemInfo(item,false);
        }
        else
            itemInfo.CloseItemInfo();
    }

    public void DisplaySkills()
    {
        for (int i = 0; i < characterSkills.Length; i++)
        {
            if (currentCharacter.EquippedCombatSkills[i] != null)
                characterSkills[i].SetUI(currentCharacter.EquippedCombatSkills[i].SkillIcon, currentCharacter.EquippedCombatSkills[i].SkillDescription, this);
            else
                characterSkills[i].SetUI(null, "", this);
        }
    }

    public void DisplayCombatEffects()
    {
        SetCombatEffectVisibility();
        if (currentCharacter.currentCombatEffects != null)
        {
            for (int i = 0; i < currentCharacter.currentCombatEffects.activeCombatEffectElements.Count; i++)
            {
                characterBuffs[i].sprite = currentCharacter.currentCombatEffects.activeCombatEffectElements[i].CombatEffect.EffectSprite;
                characterBuffs[i].color = Color.white;
            }
        }
    }

    private void SetCombatEffectVisibility()
    {
        for (int i = 0; i < characterBuffs.Length; i++)
        {
            characterBuffs[i].color = Vector4.zero;
        }
    }

    public void DisplaySkillInfo(string description)
    {
        if(description != "" && description != null)
            characterSkillInfo.text = "Skill Description:\n"+ description;
        skillSelectionPanel.DisplaySkillInfo(description);

    }

    public void ClearSkillInfo()
    {
        characterSkillInfo.text = "";
        skillSelectionPanel.ClearSkillInfo();
    }
    #endregion
}
