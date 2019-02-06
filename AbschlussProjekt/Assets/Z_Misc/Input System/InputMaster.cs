// GENERATED AUTOMATICALLY FROM 'Assets/Z_Misc/Input System/InputMaster.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class InputMaster : InputActionAssetReference
{
    public InputMaster()
    {
    }
    public InputMaster(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // UI
        m_UI = asset.GetActionMap("UI");
        m_UI_Back = m_UI.GetAction("Back");
        m_UI_InventoryOpen = m_UI.GetAction("InventoryOpen");
        m_UI_CharacterInfoClose = m_UI.GetAction("CharacterInfoClose");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_UI = null;
        m_UI_Back = null;
        m_UI_InventoryOpen = null;
        m_UI_CharacterInfoClose = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // UI
    private InputActionMap m_UI;
    private InputAction m_UI_Back;
    private InputAction m_UI_InventoryOpen;
    private InputAction m_UI_CharacterInfoClose;
    public struct UIActions
    {
        private InputMaster m_Wrapper;
        public UIActions(InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Back { get { return m_Wrapper.m_UI_Back; } }
        public InputAction @InventoryOpen { get { return m_Wrapper.m_UI_InventoryOpen; } }
        public InputAction @CharacterInfoClose { get { return m_Wrapper.m_UI_CharacterInfoClose; } }
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
    }
    public UIActions @UI
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new UIActions(this);
        }
    }
}
