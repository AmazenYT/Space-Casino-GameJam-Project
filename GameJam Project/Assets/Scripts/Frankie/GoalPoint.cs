using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPoint : MonoBehaviour
{
    public bool goNextLevel;
    public string levelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        

            if (goNextLevel)
            {
              
                SceneController.instance.NextLevel();
            }
            else
            {
               
                SceneController.instance.LoadScene(levelName);
            }
        }
    }
}
