using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFactory : MonoBehaviour
{
    public static BloodFactory Instance { get; private set; }
    public GameObject BloodPrefab;
    private int _poolSize = 1;
    private List<GameObject> _bloodPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _bloodPool = new List<GameObject>();
    }
    private void Start()
    {
        AddinPool(_poolSize);
    }
    private void AddinPool(int poolsize)
    {
        for (int i = 0; i < poolsize; i++)
        {
            GameObject blood = Instantiate(BloodPrefab);
            _bloodPool.Add(blood);
            blood.gameObject.transform.SetParent(this.transform);
            blood.gameObject.SetActive(false);
        }
    }
    public void Make(Vector3 position, Vector3 normal)
    {
        GameObject selectedBlood = null;
        foreach (GameObject blood in _bloodPool)
        {
            if (!blood.activeSelf)
            {
                selectedBlood = blood;
                break;
            }
        }
        if (selectedBlood != null)
        {
            selectedBlood.SetActive(true);
            selectedBlood.transform.position = position;
            selectedBlood.transform.forward = normal;
        }
        else
        {
            AddinPool(1);
            Make(position, normal);
        }
    }
}
