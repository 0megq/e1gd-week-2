using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public PlatformerController player;
    public TextMeshProUGUI scoreText;

    [SerializeField] AudioMixer mixer;

    void Update()
    {
        scoreText.text = "Score: " + player.coinsCollected;
    }   

    public void OnMusicChangeVolume(float value)
    {
        float volumeDb = Mathf.Log10(Mathf.Max(value, 0.00001f)) * 20f;
        mixer.SetFloat("MusicVolume", volumeDb);
    }
}
