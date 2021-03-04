using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openTradeStation : MonoBehaviour
{
    bool stationStatus = false;
    public void ToggleTrade()
    {
        if (stationStatus) {
            this.transform.localPosition = new Vector3(1500, -650, 0);
        }
        else
        {
            this.transform.localPosition = new Vector3(0, -650, 0);
        }
        stationStatus = !stationStatus;
    }
}
