using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    GameObject DiceCopy;
    public int DiceFaceUpNum;
    GameObject Planes;
    public int count;
    Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        DiceCopy = GameObject.Find("dice/Dice").gameObject;
        Planes = GameObject.Find("Planes").gameObject;
        count = 0;
        newPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        //Vector3 newPosition = new Vector3(1,2,3);
        DiceFaceUpNum = DiceCopy.GetComponent<Dice>().DiceFaceUpNum;
        if (Input.GetMouseButtonDown(0))
        {
            
            
        }
         count =  DiceFaceUpNum;
        newPosition = Planes.transform.GetChild(DiceFaceUpNum).position;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, 0.1f);



    }
}
