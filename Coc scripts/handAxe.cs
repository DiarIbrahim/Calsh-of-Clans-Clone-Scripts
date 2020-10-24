using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handAxe : MonoBehaviour
{
    public bool isconn = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "building" && !isconn)
        {
            other.transform.GetComponent<Building>().Attacked();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "attacker" && isconn)
        {
            other.transform.GetComponent<Attacker>().attackerAttacked(24);
            Destroy(this.gameObject);
        }
    }

}
