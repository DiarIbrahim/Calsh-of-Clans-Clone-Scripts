using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour 
{
    public float TimeSec = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyByTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(TimeSec);
        Destroy(this.gameObject);
    }
}
