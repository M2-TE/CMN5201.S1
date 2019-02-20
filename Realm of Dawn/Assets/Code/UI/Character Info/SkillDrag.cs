using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillDrag : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SkillSelectionInfo skillSelectionInfo;
    public SkillHighlight Highlight;
    public Transform Row;

    private void Start()
    {
        if (skillSelectionInfo == null)
            enabled = false;
        else
        {
            skillSelectionInfo.LocalPosition = transform.localPosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Row.SetSiblingIndex(Row.transform.parent.childCount - 1);
        skillSelectionInfo.transform.SetSiblingIndex(skillSelectionInfo.transform.parent.childCount - 1);
        Highlight.Show(true);

        skillSelectionInfo.skillSelectionPanel.DraggedSkill = skillSelectionInfo.skill;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y +30f, 0f);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Highlight.Show(false);
        transform.localPosition = skillSelectionInfo.LocalPosition;
    }

}
