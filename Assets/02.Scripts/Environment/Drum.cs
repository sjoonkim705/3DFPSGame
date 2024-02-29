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
    public bool IsExploding = false;
    public List<Material> BarrelMaterials;
    private MeshRenderer _barrelMesh;

    public void Start()
    {
        BarrelRigid = GetComponent<Rigidbody>();
        _barrelMesh = GetComponent<MeshRenderer>();
        int materialRandIndex = Random.Range(0, BarrelMaterials.Count);
        _barrelMesh.material = BarrelMaterials[materialRandIndex];

    }

    public void Hit(DamageInfo damage)
    {
        _hitCount++;
        if (_hitCount >= 3)
        {
            BurstBarrel();
        }

    }
    public void BurstBarrel()
    {
        IsExploding = true;
        int findLayer = LayerMask.GetMask("Player") | LayerMask.GetMask("Monster") | LayerMask.GetMask("Environment");
        Collider[] colliders = Physics.OverlapSphere(transform.position, BurstingExplosionRadius, findLayer);


        foreach (Collider collider in colliders)
        {
            IHitable hitableObject;
            if (collider.TryGetComponent<IHitable>(out hitableObject))
            {
                if (collider.CompareTag("Player") || collider.CompareTag("Monster"))
                {
                    DamageInfo damageInfo = new DamageInfo();
                    hitableObject.Hit(damageInfo);
                }
                else if (collider.CompareTag("Barrel"))
                {
                    Drum aBarrel = collider.GetComponent<Drum>();
                    if (aBarrel != null && !aBarrel.IsExploding)
                    {
                        aBarrel.BurstBarrel();
                    }
                }
            }
   
        }

        BarrelExplodeEffect.gameObject.transform.position = transform.position;
        BarrelExplodeEffect.Play();
        BarrelRigid.AddForce(Vector3.up * _burstingForce, ForceMode.Impulse);
        BarrelRigid.AddTorque(new Vector3(Random.Range(0f,3f),0, Random.Range(0f,4f)) * _burstingForce/Random.Range(1f,3f));
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(_burstingTime);
        Destroy(gameObject);
    }

}
