using System.Collections;
using UnityEngine;
using SWNetwork;

public class TestedPlayer : MonoBehaviour
{
    public Route currentRoute;
    public Dice dice;
    public GameManager gm;

    public int routePosition;//玩家所在格子位置
    int steps; //玩家需要移动的格子数
    bool isMoving; //判断玩家是否移动中

    public float moveSpeed = 5.0f;//移动速度

    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (dice.moveAllowed&&!isMoving)
        {
            dice.moveAllowed = false;
            steps = dice.diceNumber;
            Debug.Log("dice number: "+steps);
            StartCoroutine(PlayerMove());
        }
    }

    IEnumerator PlayerMove()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while (steps > 0)
        {
            routePosition++;
            routePosition %= currentRoute.childNodeList.Count;

            Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            while (MoveToNextNode(nextPos))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            steps--;
        }

        isMoving = false;

        //处理格子事件
        DealWithRoute();
    }

    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, moveSpeed * Time.deltaTime));
    }

    //格子交互
    public void DealWithRoute()
    {
        CanvasManager.Instance.ToCanvasStation();
    }
}
