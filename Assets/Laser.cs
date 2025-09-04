using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [Header("Timer Settings")]
    public float fireInterval = 3f;
    public float warningTime = 1f;
    private float fireTimer;

    [Header("Laser Prefab")]
    public GameObject laserPrefab;
    public Transform firePoint;

    [Header("Warning Indicator")]
    public GameObject indicatorPrefab;
    private GameObject activeIndicator;

    [Header("Lazer Animator")]
    private Animator animator;

    [Header("Laser attributes")]
    public float laserLength = 1.5f;  // how far the laser extends
    public float laserWidth = 0.5f;  // how fat the laser is (adjust hitbox if you change this value)
    private BoxCollider2D hitbox;
    public Light2D light;

    private void Start()
    {
        fireTimer = fireInterval;
        animator = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider2D>();
        animator.SetTrigger("Idle");
        hitbox.enabled = false;
        if (light != null)
            light.intensity = 0f;
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
        
        if (fireTimer <= warningTime && activeIndicator == null)
        {
            ShowIndicator();
        }
        if (fireTimer <= 0f)
        {
            FireLaser();
            fireTimer = fireInterval;
            
        }
    }

    private void FireLaser()
    {
        if (activeIndicator) Destroy(activeIndicator);

        

        if (laserPrefab && firePoint)
        {

            GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
            light.intensity = 3f;
            hitbox.enabled = true;
            Vector3 scale = laser.transform.localScale;
            scale.y = laserLength;
            scale.x = laserWidth;
            laser.transform.localScale = scale;
            StartCoroutine(DestroyAfterDelay(laser, 0.5f));
        }
        else
        {
            Debug.LogWarning("Laser Prefab or Fire Point not assigned.");
        }

        animator.SetTrigger("Fire");
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        hitbox.enabled = false;
        Destroy(obj);
        light.intensity = 0f;
    }


    private void ShowIndicator()
    {
        if (indicatorPrefab && firePoint)
        {
            activeIndicator = Instantiate(indicatorPrefab, firePoint.position, firePoint.rotation);
            light.intensity = 1f;
            Vector3 scale = activeIndicator.transform.localScale;
            scale.y = laserLength;
            scale.x = laserWidth;
            activeIndicator.transform.localScale = scale;
            activeIndicator.transform.SetParent(transform);
            animator.SetTrigger("Charging");
        }
        else
        {
            Debug.LogWarning("Indicator Prefab or Fire Point not assigned.");
        }
    }

    private void OnDrawGizmos()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.red;
        Vector3 offset = new Vector3(0.0f, 3.0f, 0.0f);
        Vector3 start = firePoint.position - offset;
        Vector3 end = start + transform.up * (laserLength * 4f);

        Gizmos.DrawLine(start, end);
    }
}
