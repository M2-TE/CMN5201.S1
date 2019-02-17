using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillSelectionPanel : UIPanel
{
    #region UIConnectors
    public TextMeshProUGUI characterSkillInfo;
    public SkillInfo[] characterSkills;
    #endregion


    protected override void Awake()
    {
        (AssetManager.Instance.GetManager<SkillSelectionManager>() ?? new SkillSelectionManager()).SkillSelectionPanel = this;
        base.Awake();
    }

    void Start()
    {
        
    }

}
