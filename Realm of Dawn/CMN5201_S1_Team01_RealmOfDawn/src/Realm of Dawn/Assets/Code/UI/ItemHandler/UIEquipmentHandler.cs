using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipmentHandler : UIElementHandler
{
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private EquipmentSlot slotType;

    private CharacterInfoPanel characterInfoPanel;

    private void Start()
    {
        characterInfoPanel = AssetManager.Instance.GetManager<CharacterInfoManager>().CharacterInfoPanel;
        characterInfoPanel.ConnectUIHandler(this, slotType);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (!empty)
        {
            characterInfoPanel.DisplayEquipmentInfo(slotType, true);
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        characterInfoPanel.DisplayEquipmentInfo(slotType, false);
    }

    protected override void OnPrimaryAction()
    {
        AssetManager.Instance.GetManager<CharacterInfoManager>().PrimaryActionOnItem(slotType);
    }

    public override void SetEmpty()
    {
        SetEmpty(true);
    }

    public void SetEmpty(bool state)
    {
        empty = state;
        if(empty)
            Icon.sprite = emptySprite;
    }
}
