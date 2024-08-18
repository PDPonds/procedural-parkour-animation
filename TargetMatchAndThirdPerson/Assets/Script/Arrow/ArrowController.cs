using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Vector3 movePoint;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint, PlayerManager.Instance.arrowSpeed * Time.deltaTime);

        transform.LookAt( movePoint);

        if (Vector3.Distance(transform.position, movePoint) < .01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}
