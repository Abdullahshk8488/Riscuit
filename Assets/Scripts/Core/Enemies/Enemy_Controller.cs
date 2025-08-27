using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [field: SerializeField] public float CurrentHealth { get; private set; }

    [field: SerializeField] public Node currentNode { get; private set; }
    [field: SerializeField] public List<Node> path { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private float speed = 3;

    public enum StateMachine
    {
        Pursue,
        Attack,
        Evade
    }

    public StateMachine currentState;

    private void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }

        if (player == null)
        {
            Debug.LogError("Player component not found on the GameObject tagged as 'Player'.");
        }

        currentState = StateMachine.Pursue;
    }

    private void Update()
    {
        switch(currentState)
        {
            case StateMachine.Pursue:
                Pursue();
                break;
            case StateMachine.Attack:
                Attack();
                break;
            case StateMachine.Evade:
                Evade();
                break;
        }

        if (currentState != StateMachine.Pursue)
        {
            currentState = StateMachine.Pursue;
            path.Clear();
        }
        //else if (currentState != StateMachine.Attack)
        //{
        //    currentState = StateMachine.Attack;
        //    path.Clear();
        //}
        //else if (currentState != StateMachine.Evade)
        //{
        //    currentState = StateMachine.Evade;
        //    path.Clear();
        //}

        CreatePath();
    }

    private void Pursue()
    {
        if (path.Count == 0)
        {
            path = AStarManager.Instance.GeneratePath(currentNode, AStarManager.Instance.FindNearestNode(player.transform.position));
        }
    }

    private void Attack()
    {

    }

    private void Evade()
    {

    }

    private void CreatePath()
    {
        if(path.Count > 0)
        {
            int x = 0;

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[x].transform.position.x, path[x].transform.position.y, -1),
                speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, path[x].transform.position) < 1.0f)
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }
    }
}
