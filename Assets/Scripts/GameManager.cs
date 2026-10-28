using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlatformerController player;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = "Score: " + player.coinsCollected;
    }   
}
