using UnityEngine;
using UnityEngine.Rendering;

public class VFXController : MonoBehaviour
{
    public GameObject buff, debuff;
    private float timeCounter;
    private GameObject currentEffectPrefab;

    private void Update()
    {
        if (buff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2)
            {
                timeCounter = 0f;
                buff.SetActive(false);

            }
        }

        if (debuff.activeInHierarchy)
        {
            timeCounter -= Time.deltaTime;
            if (timeCounter <= 0f)
            {
                timeCounter = 0f;
                debuff.SetActive(false);
            }
        }
    }

    public void SetEffectPrefab(GameObject prefab)
    {
        if (currentEffectPrefab != null)
        {
            Destroy(currentEffectPrefab);
        }
        currentEffectPrefab = Instantiate(prefab, transform);
        currentEffectPrefab.SetActive(true);
    }
}
