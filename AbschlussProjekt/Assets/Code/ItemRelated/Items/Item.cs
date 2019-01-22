using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{

    private ItemContainer container;
    [Range(2, 4)][SerializeField]private float itemSize = 3;
    private int itemID;

    private int currentlyStacked;
    [SerializeField] public int CurrentlyStacked {  get { return currentlyStacked; } set { currentlyStacked = (value > Container.StackingLimit) || (value < 1) ? currentlyStacked : value; } }
    public ItemContainer Container { get { return container; } set { container = value; Setup(); } }
    
    public  void Setup()
    {
        SetSprite();
        SetID();
        SetCorrectSize();
        SetEditorName();
    }

    private void SetSprite()
    {
        if(Container !=null)
            GetComponent<SpriteRenderer>().sprite = Container.ItemIcon;
    }

    private void SetID()
    {
        itemID = GetInstanceID();
    }

    private void SetCorrectSize()
    {
        transform.localScale = Vector3.one * itemSize;
    }

    private void SetEditorName()
    {
        if (Container != null)
            name = Container.name + " ID[" + itemID + "]";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
