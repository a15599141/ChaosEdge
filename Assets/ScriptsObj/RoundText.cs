using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundText : MonoBehaviour
{
    GameObject Roll;
    // Start is called before the first frame update
    void Start()
    {
        Roll = GameObject.Find("Roll").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TMP_Text>().text = "Round "+ Roll.GetComponent<Roll>().roundCount;
    }
}
