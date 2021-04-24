using Hyperdimension;
using UnityEngine;

public class BarrelInstantiator : MonoBehaviour
{
    public GameObject barrelPrefab;

    public int barrelCount = 100;
    void Start()
    {
        for (int i = 0; i < barrelCount; i++)
        {
            Instantiate(barrelPrefab).GetComponent<HyperBaseTransform>().Position =
                new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
        }
    }
}
