using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSteamer : MonoBehaviour
{
    int maxFailures = 3;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayVideo());
        // PlayVideo();
    }

    IEnumerator PlayVideo()
    {
        var videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        var waiter = new WaitForSeconds(1);
        int failures = 0;
        while (!videoPlayer.isPrepared)
        {
            yield return waiter;
            failures++;
            if (failures > maxFailures)
            {
                throw new System.Exception();
            }
        }
        var img = GetComponent<RawImage>();
        img.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
}
