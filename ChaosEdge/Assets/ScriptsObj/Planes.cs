using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planes : MonoBehaviour
{
    GameObject Plane0, Plane1, Plane2, Plane3, Plane4, Plane5, Plane6,Plane7, Plane8, Plane9, Plane10, Plane11;
    // Start is called before the first frame update
    void Start()
    {
        Plane0 = GameObject.Find("Plane0").gameObject;
        Plane1 = GameObject.Find("Plane1").gameObject;
        Plane2 = GameObject.Find("Plane2").gameObject;
        Plane3 = GameObject.Find("Plane3").gameObject;
        Plane4 = GameObject.Find("Plane4").gameObject;
        Plane5 = GameObject.Find("Plane5").gameObject;
        Plane6 = GameObject.Find("Plane6").gameObject;
        Plane7 = GameObject.Find("Plane7").gameObject;
        Plane8 = GameObject.Find("Plane8").gameObject;
        Plane9 = GameObject.Find("Plane9").gameObject;
        Plane10 = GameObject.Find("Plane10").gameObject;
        Plane11 = GameObject.Find("Plane11").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
