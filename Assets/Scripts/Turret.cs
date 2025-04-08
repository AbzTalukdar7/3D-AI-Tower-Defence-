using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    /*  
    private Transform target;

    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity setup fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    

    void Start()
    {
        InvokeRepeating("UpdateTarget",0f,0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance){
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range){
            target = nearestEnemy.transform;
        }else{
            target = null;
        }
    }

    */

     private Transform target;

    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    // New variable to track targeting type
    public enum TargetingType { Closest, First, Strongest }
    public TargetingType targetingType = TargetingType.Closest;

    [Header("Unity setup fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        int strongestEnemyHealth = 0; // Variable to track the strongest enemy health
        GameObject nearestEnemy = null;
        GameObject firstEnemy = null; // Variable to track the first enemy
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && targetingType == TargetingType.Closest)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
            else if (targetingType == TargetingType.First && firstEnemy == null)
            {
                firstEnemy = enemy;
            }
            else if (targetingType == TargetingType.Strongest)
            {
                int enemyHealth = enemy.GetComponent<Enemy>().health;
                if (enemyHealth != 0 && enemyHealth > strongestEnemyHealth)
                {
                    strongestEnemyHealth = enemyHealth;
                    nearestEnemy = enemy;
                }
            }
        }

        // Update target based on targeting type
        if (targetingType == TargetingType.First && firstEnemy != null)
        {
            target = firstEnemy.transform;
        }
        else
        {
            target = nearestEnemy != null && shortestDistance <= range ? nearestEnemy.transform : null;
        }
    }
    

    void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown<= 0f){
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot(){
        GameObject bulletGO  = (GameObject)Instantiate (bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null){
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void ChangeTargetingType(string newTargetingType)
    {
        if (newTargetingType == "Closest")
            targetingType = TargetingType.Closest;
        else if (newTargetingType == "Strongest")
            targetingType = TargetingType.Strongest;
        else if (newTargetingType == "First")
            targetingType = TargetingType.First;
        else
            return;
    }
}
