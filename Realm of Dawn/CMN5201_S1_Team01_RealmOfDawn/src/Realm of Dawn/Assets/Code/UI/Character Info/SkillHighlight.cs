using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillHighlight : MonoBehaviour, IPointerEnterHandler
{
    public SkillSelectionPanel SkillSelectionPanel;

    public Image[] image;

    public void Start()
    {
        Show(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillSelectionPanel.DraggedSkill = null;
    }

    public void Show(bool hightlight)
    {
        for (int i = 0; i < image.Length; i++)
        {
            image[i].color = !hightlight ? Vector4.zero : new Vector4(1,1,0,1);
        }
    }
}
