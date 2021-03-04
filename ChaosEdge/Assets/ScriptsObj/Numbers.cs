using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numbers : MonoBehaviour
{
    GameObject Num1, Num2, Num3, Num4, Num5, Num6;
    GameObject DiceCopy;
    public int DiceFaceUpNum;
    // Start is called before the first frame update
    void Start()
    {
        Num1 = GameObject.Find("Num1").gameObject;
        Num2 = GameObject.Find("Num2").gameObject;
        Num3 = GameObject.Find("Num3").gameObject;
        Num4 = GameObject.Find("Num4").gameObject;
        Num5 = GameObject.Find("Num5").gameObject;
        Num6 = GameObject.Find("Num6").gameObject;
        DiceCopy = GameObject.Find("dice/Dice").gameObject;
        setAllFalse();
    }

    // Update is called once per frame
    void Update()
    {
        DiceFaceUpNum = DiceCopy.GetComponent<Dice>().DiceFaceUpNum;
        setAllFalse();
        if (DiceFaceUpNum == 1)
        {
            Num1.SetActive(true);
        }
        if (DiceFaceUpNum == 2)
        {
            Num2.SetActive(true);
        }
        if (DiceFaceUpNum == 3)
        {
            Num3.SetActive(true);
        }
        if (DiceFaceUpNum == 4)
        {
            Num4.SetActive(true);
        }
        if (DiceFaceUpNum == 5)
        {
            Num5.SetActive(true);
        }
        if (DiceFaceUpNum == 6)
        {
            Num6.SetActive(true);
        }
    }

    void setAllFalse()
    {
        Num1.SetActive(false);
        Num2.SetActive(false);
        Num3.SetActive(false);
        Num4.SetActive(false);
        Num5.SetActive(false);
        Num6.SetActive(false);
    }
}
