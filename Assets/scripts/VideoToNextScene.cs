using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoToNextScene : MonoBehaviour
{
    public string nextSceneName; // Name der nächsten Szene
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Wenn das Video fertig ist, rufe OnVideoEnd auf
        videoPlayer.loopPointReached += OnVideoEnd;

        // Optional: Video automatisch abspielen
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Szene wechseln
        SceneManager.LoadScene(nextSceneName);
    }
}
