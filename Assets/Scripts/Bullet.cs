using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;

    public int damage = 50;
    public float expolosionRadius = 0f;
    public GameObject impactEffect;

    public void Seek(Transform _target){
        target = _target;
    }

    void Update()
    {
        if (target == null){
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame){
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

    }

    void HitTarget(){
        //GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        //Destroy(effectIns, 6f);

        if (expolosionRadius > 0f){
            Explode();
        }else{
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null){
            e.TakeDamage(damage);
        }
    }

    void Explode(){
       Collider[] colliders = Physics.OverlapSphere(transform.position, expolosionRadius);
       foreach (Collider collider in colliders)
       {
            if (collider.tag == "Enemy"){
                Damage(collider.transform);
            }
       }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, expolosionRadius);
    }
}
