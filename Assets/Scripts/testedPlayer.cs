using System.Collections;
using UnityEngine;
using SWNetwork;

public class TestedPlayer : MonoBehaviour
{
    public int routePosition;//玩家所在格子位置

    //玩家属性
    string name;
    public int energy;
    int hp;
    int atk;
    int def;
    int evo;
    int bag;

    // Start is called before the first frame update
    void Start()
    {
        energy = 1000;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("battle incomming");
        }
    }
}
