using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    [SerializeField] float totalMinutes;
    public float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimePassing());
    }

    IEnumerator TimePassing()
    {
        for (int i = 0; i < totalMinutes * 60; i++)
        {
            yield return new WaitForSeconds(1);
            currentTime++;
        }
        //end level
    }
}
