using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static int lives = 3;
    public static bool over = false;
    public static int score = 0;
}
public class PlayerHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Score: " + (GameManager.score).ToString();
        livesText.text = "Lives: " + (GameManager.lives).ToString();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "EnemyHead")
        {
            else if (collision.collider.gameObject.tag == "EnemyBody")
            {
                GameManager.lives -= 1;
                Debug.Log(GameManager.lives);
                if (GameManager.lives == 0)
                {
                    GameManager.over = true;
                    Time.timeScale = 0;
                }
                else
                {
                    SceneManager.LoadScene("Level 1");
                }

            }
            if (transform.position.y >= collision.collider.transform.position.y)
            {
                Destroy(collision.collider.transform.parent.gameObject);
                Destroy(collision.collider.gameObject);
                GameManager.score += 100;
            }
            else {
                SceneManager.LoadScene("Completed Game");
            }
        }
        void OnGUI()
        {
            if (GameManager.over == true)
            {
                Vector2 dimensions = new Vector2(Screen.width / 2, Screen.height / 2);
                Vector2 position = new Vector2((Screen.width - dimensions.x) / 2, (Screen.height - dimensions.y) / 2);
                GUI.Label(new Rect(position, dimensions), "Game Over");
                switch (GameManager.score)
                {
                    case 100:
                        GUI.Label(new Rect(position2, dimensions), "You’re better than this.");
                        break;
                    case 200:
                        GUI.Label(new Rect(position2, dimensions), "Well you beat someone.");
                        break;
                    case 300:
                        GUI.Label(new Rect(position2, dimensions), "Comin' along.");
                        break;
                    case 400:
                        GUI.Label(new Rect(position2, dimensions), "Well played.");
                        break;
                    case 500:
                        GUI.Label(new Rect(position2, dimensions), "Came to play, huh?");
                        break;
                    case 600:
                        GUI.Label(new Rect(position2, dimensions), "Not too shabby.");
                        break;
                    case 700:
                        GUI.Label(new Rect(position2, dimensions), "Quality.");
                        break;
                    case 800:
                        GUI.Label(new Rect(position2, dimensions), "Almost the perfect game!");
                        break;
                    case 900:
                        GUI.Label(new Rect(position2, dimensions), "Perfect!");
                        break;
                    default:
                        GUI.Label(new Rect(position2, dimensions), "No points? Seriously?");
                        break;
                }
                Vector2 position2 = new Vector2((Screen.width - dimensions.x) / 2, (Screen.height - dimensions.y) / 2 + 20);
            }
        }
    }
}
