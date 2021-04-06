using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItemDetail : MonoBehaviour
{
    public GameObject detailPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDetailPanel()
    {
        detailPanel.SetActive(true);
    }

    public void HideDetailPanel()
    {
        detailPanel.SetActive(false);

    }
}
