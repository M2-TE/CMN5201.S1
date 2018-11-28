using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiveEffectManager : MonoBehaviour
{
    [SerializeField] private int framerate = 16;

    #region Static
    private static List<CollectiveEffect> effects = new List<CollectiveEffect>();

    public static void Register(CollectiveEffect collectiveEffect)
    {
        effects.Add(collectiveEffect);
    }

    public static void Unregister(CollectiveEffect collectiveEffect)
    {
        effects.Remove(collectiveEffect);
    }
    #endregion

    protected IEnumerator Start()
    {
        WaitForSeconds waiter = new WaitForSeconds(1f / framerate);

        while (true)
        {
            for (int index = 0; index < effects.Count; index++)
                effects[index].SetSprite();

            yield return waiter;
        }
    }
}
