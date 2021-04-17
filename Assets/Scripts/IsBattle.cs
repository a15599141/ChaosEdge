using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBattle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BattleConfirm()
    {

    }

    public void BattleCancel()
    {
        PlayerManager.Instance.BattleCancel();
        OnExit();
    }

    public void OnExit()
    {
        CanvasManager.Instance.CloseCanvasEngagement();
    }
}
