using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DiscoBall : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float SizeChange, ChangeSpeed, RotationSpeed;

    private float currentSize;
    private bool upwardsTrend = true;

    private float initSize;

    public void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initSize = transform.parent.localScale.x;
        currentSize = initSize;
    }

    public void Update()
    {
        transform.parent.transform.Rotate(0f, 0f, RotationSpeed);
        HandleScale();
        HandleColor();
    }

    private void HandleScale()
    {
        if (currentSize >= initSize + SizeChange || currentSize <= initSize - SizeChange)
            upwardsTrend = !upwardsTrend;

        currentSize += upwardsTrend ? (Time.deltaTime * RotationSpeed) : -(Time.deltaTime * RotationSpeed);
        transform.parent.transform.localScale = new Vector3(currentSize, currentSize, 0f);
    }

    private void HandleColor()
    {
        spriteRenderer.color = new Vector4(Random.value, Random.value, Random.value, 1);
    }
}
