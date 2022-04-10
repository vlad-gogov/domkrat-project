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
        // GetComponent<RawImage>().color = Color.green;

        var videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.SetDirectAudioMute(0, true);
        videoPlayer.Prepare();
        // refresh rate
        var waiter = new WaitForSeconds(0.1f);
        int failures = 0;
        while (!videoPlayer.isPrepared)
        {
            yield return waiter;
            // failures++;
            //if (failures > maxFailures)
            //{
            //    throw new System.Exception();
            //}
        }
        var img = GetComponent<RawImage>();
        img.texture = videoPlayer.texture;
        videoPlayer.Play();
        yield return null;
    }
}
