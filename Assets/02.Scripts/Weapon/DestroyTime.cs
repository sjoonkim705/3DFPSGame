using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeleteType
{
    Destory,
    Inactive

}

public class DestroyTime : MonoBehaviour
{
    public DeleteType deleteType;
    public float DestroyTimeSpan = 1.5f;
    private float _timer = 0;
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > DestroyTimeSpan)
        {
            if (deleteType == DeleteType.Destory)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                _timer = 0;
            }
        }
    }
}
