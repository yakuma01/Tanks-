using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject target;
    public float projectileSpeed = 15;
    public Transform spawnPoint;

    private Transform parent;

    float shootTime = 0.5f;
    float lastUpdateTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget();

        float angle;

        if(CalculateAngle(out angle))
        {
            RotateTurret(angle);
            if (Time.time >= lastUpdateTime + shootTime)
            {
                Fire();
                lastUpdateTime = Time.time;
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Fire();
        //}
    }

    bool CalculateAngle(out float angle, bool low = true)
    {
        Vector3 direction = target.transform.position - spawnPoint.position;
        float g = Physics.gravity.magnitude;
        float y = direction.y;
        direction.y = 0;
        float x = direction.magnitude;

        float v2 = projectileSpeed * projectileSpeed;
        float insideSqrt = (v2 * v2) - g * (g * x * x + 2 * y * v2);

        if(insideSqrt >= 0)
        {
            float root = Mathf.Sqrt(insideSqrt);
            float highAngle = v2 + root;
            float lowAngle = v2 - root;

            if (low)
            {
                angle = Mathf.Atan2(lowAngle, g * x) * Mathf.Rad2Deg;
            }
            else
            {
                angle = Mathf.Atan2(highAngle, g * x) * Mathf.Rad2Deg;
            }
            return true;
        }
        else
        {
            angle = 0;
            return false;
        }
    }


    void RotateTurret(float angle)
    {
        if(angle != 0)
        {
            transform.localEulerAngles = new Vector3(-angle, 0, 0);
        }
    }

    void FaceTarget()
    {
        Vector3 dir = target.transform.position - parent.position;

        var lookDir = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        parent.transform.rotation = lookDir;
    }

    void Fire()
    {
        GameObject shell = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        shell.transform.position = spawnPoint.position;
        Rigidbody rb = shell.AddComponent<Rigidbody>();
        rb.velocity = this.transform.forward * projectileSpeed;
    }
}