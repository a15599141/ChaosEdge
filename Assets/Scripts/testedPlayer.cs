using System.Collections;
using UnityEngine;

public class testedPlayer : MonoBehaviour
{
    public Route currentRoute;
    public GameManager gm;

    int routePosition;
    public int steps;
    bool isMoving;

    public float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (gm.moveAllowed&&!isMoving)
        {
            gm.moveAllowed = false;
            steps = gm.diceNumber;
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
        gm.rollButton.interactable = true;
    }

    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, moveSpeed * Time.deltaTime));
    }
}
