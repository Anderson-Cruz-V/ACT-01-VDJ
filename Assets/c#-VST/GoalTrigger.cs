using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private GameBootstrap manager;

    private void Start()
    {
        manager = FindAnyObjectByType<GameBootstrap>();

        Debug.Log("GOAL TRIGGER INICIADO");

        if (manager == null)
        {
            Debug.LogError("NO SE ENCONTRO GAMEBOOTSTRAP");
        }
        else
        {
            Debug.Log("GAMEBOOTSTRAP ENCONTRADO");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ALGO ENTRO A LA META: " + other.gameObject.name);
        Debug.Log("TAG: " + other.gameObject.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("NARUTO LLEGO A LA META");

            if (manager != null)
            {
                manager.ReachGoal();
            }
        }
    }
}