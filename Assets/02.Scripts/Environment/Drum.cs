using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    private int _hitCount = 0;
    public ParticleSystem BarrelExplodeEffect;
    public Rigidbody BarrelRigid;
    private float _burstingForce = 12f;
    private float _burstingTime = 3f;

    public float BurstingExplosionRadius = 3f;
    public int Damage = 70;

    public void Start()
    {
        BarrelRigid = GetComponent<Rigidbody>();
    }

    public void Hit(int damage)
    {
        _hitCount++;
        if (_hitCount >= 3)
        {
            BurstBarrel();
        }

    }
    public void BurstBarrel()
    {
        int findLayer = LayerMask.GetMask("Player") | LayerMask.GetMask("Monster") | LayerMask.GetMask("Environment");
        Collider[] colliders = Physics.OverlapSphere(transform.position, BurstingExplosionRadius, findLayer);


        foreach (Collider collider in colliders)
        {
            IHitable hitableObject = collider.GetComponent<IHitable>();
            if (hitableObject != null)
            {
            }
        }

        BarrelExplodeEffect.gameObject.transform.position = transform.position;
        BarrelExplodeEffect.Play();
        BarrelRigid.AddForce(Vector3.up * _burstingForce, ForceMode.Impulse);
        BarrelRigid.AddTorque(new Vector3(1,0,1) * _burstingForce/2);
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(_burstingTime);
        Destroy(gameObject);
    }

}
