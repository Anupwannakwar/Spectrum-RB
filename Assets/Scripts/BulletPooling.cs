using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private GameObject ChargedProjectile = null;
    [SerializeField] private GameObject SpecialProjectile = null;

    List<GameObject> bulletList;
    List<GameObject> ChargedBulletList;
    List<GameObject> SpecialBulletList;

	// Use this for initialization
	void Start ()
    {
        bulletList = new List<GameObject>(10);

        for (int i = 0; i < bulletList.Capacity; i++) 
        {
            GameObject bullet = Instantiate(bulletPrefab,transform);
            bulletList.Add(bullet);
            bullet.SetActive(false);
        }

        ChargedBulletList = new List<GameObject>(5);

        for (int i = 0; i < ChargedBulletList.Capacity; i++)
        {
            GameObject bullet = Instantiate(ChargedProjectile, transform);
            ChargedBulletList.Add(bullet);
            bullet.SetActive(false);
        }

        SpecialBulletList = new List<GameObject>(1);

        for (int i = 0; i < SpecialBulletList.Capacity; i++)
        {
            GameObject bullet = Instantiate(SpecialProjectile, transform);
            SpecialBulletList.Add(bullet);
            bullet.SetActive(false);
        }
    }
	
    public GameObject GetBullet()
    {
        foreach(GameObject bulletInstance in bulletList)
        {
            if(!bulletInstance.activeInHierarchy)
            {
                return bulletInstance;
            }
        }

        GameObject extraBullet = Instantiate(bulletPrefab, transform);
        bulletList.Add(extraBullet);
        extraBullet.SetActive(false);
        return extraBullet;
    }

    public GameObject GetChargedBullet()
    {
        foreach (GameObject bulletInstance in ChargedBulletList)
        {
            if (!bulletInstance.activeInHierarchy)
            {
                return bulletInstance;
            }
        }

        GameObject extraBullet = Instantiate(ChargedProjectile, transform);
        ChargedBulletList.Add(extraBullet);
        extraBullet.SetActive(false);
        return extraBullet;
    }

    public GameObject GetSpecialBullet()
    {
        foreach (GameObject bulletInstance in SpecialBulletList)
        {
            if (!bulletInstance.activeInHierarchy)
            {
                return bulletInstance;
            }
        }

        GameObject extraBullet = Instantiate(SpecialProjectile, transform);
        SpecialBulletList.Add(extraBullet);
        extraBullet.SetActive(false);
        return extraBullet;
    }
}
