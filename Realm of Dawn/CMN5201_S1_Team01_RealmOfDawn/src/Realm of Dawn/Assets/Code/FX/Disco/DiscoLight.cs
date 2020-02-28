using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class DiscoLight : MonoBehaviour
{
    public float changeColorTime;

    private float currentTime;

    private Light m_Light;

    private void OnEnable()
    {
        m_Light = GetComponent<Light>();
    }

    void Update()
    {
        m_Light.color = new Vector4(Random.value, Random.value, Random.value, 1);
    }
}
