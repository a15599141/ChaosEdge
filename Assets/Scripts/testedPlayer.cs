using System.Collections;
using UnityEngine;
using SWNetwork;

public class testedPlayer : MonoBehaviour
{
    public Route currentRoute;
    public Dice dice;

    int routePosition;
    public int steps;
    bool isMoving;

    public float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        if (dice.moveAllowed&&!isMoving)
        {
            dice.moveAllowed = false;
            steps = dice.diceNumber;
            Debug.Log("dice number: "+steps);
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
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
        dice.rollButton.interactable = true;
    }

    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, moveSpeed * Time.deltaTime));
    }
}
