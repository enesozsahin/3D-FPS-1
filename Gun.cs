using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range=100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactParticleEffect;
    public float impactForce = 30f;
    public float fireRate = 15f;

    private float nextTimetoFire = 0f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Animator animator;
    // Update is called once per frame

    void Start()
    {

        currentAmmo = maxAmmo;
        
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <=0) {
            StartCoroutine(Reload());
            return;
        }

        //if you want to make the auto just remove the Down phrase in GetButtonDown
        if (Input.GetButtonDown("Fire1"))
        {
            
            Shoot();
        }


        IEnumerator Reload()
        {
            isReloading = true;
            Debug.Log("Reloading...");
            animator.SetBool("Reloading", true);  

            yield return new WaitForSeconds(reloadTime-.25f);

            animator.SetBool("Reloading", false);

            yield return new WaitForSeconds(.25f);


            currentAmmo = maxAmmo;
            isReloading=false;
        }

        void Shoot()
        {
            muzzleFlash.Play();

            currentAmmo --;

            RaycastHit hit;
            if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                Target target = hit.transform.GetComponent<Target>();
                if(target != null) {
                    target.TakeDamage(damage);
                }

                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce );
                }

                GameObject ImpactGO = Instantiate(impactParticleEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(ImpactGO, 2f);
            }
        }
    }
}
