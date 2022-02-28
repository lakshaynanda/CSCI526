using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint1;
    public Transform firePoint2; 
    public GameObject bulletPrefab1;
    public GameObject bulletPrefab2;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot1();
        }
        
    }

    void Shoot1()
    {
        Instantiate(bulletPrefab1, firePoint1.position, firePoint1.rotation);
    }
    
}
