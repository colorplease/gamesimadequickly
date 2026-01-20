using UnityEngine;

public class Eyechecker : MonoBehaviour
{
    public static Eyechecker instance;
    public bool lookingAtPastPortal = false;
    public bool lookingAtFuturePortal = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if(hit.collider.gameObject.tag == "pastPortal")
            {
                lookingAtPastPortal = true;
            }
            else
            {
                lookingAtPastPortal = false;
            }
            if(hit.collider.gameObject.tag == "futurePortal")
            {
                lookingAtFuturePortal = true;
            }
            else
            {
                lookingAtFuturePortal = false;
            }
        }
    }
}
