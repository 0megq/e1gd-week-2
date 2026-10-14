using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlatformerController>();
            player.coinsCollected++;
            Debug.Log("coins collected: " + player.coinsCollected);
            Destroy(gameObject);
        }
    }
}
