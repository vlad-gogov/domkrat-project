using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

// тут такие блять костыли, лучше не лезь сюда

public class PageBuilder : MonoBehaviour
{
    class Drawable
    {
        public Drawable(string content) { this.content = content; }
        public virtual void Execute(PageBuilder owner) { throw new NotImplementedException(); }
        public string content;
    }
    class DText : Drawable
    {
        public DText(string content) : base(content) { }
        public override void Execute(PageBuilder owner)
        {
            string prettyContent = content.Trim().Trim(new char[] {'\r', '\n'});
            owner.addText(prettyContent);
        }
    }
    class DVideo : Drawable
    {
        public DVideo(string content) : base(content) { }
        public override void Execute(PageBuilder owner)
        {
            owner.AddVideo(content);
        }
    }
    class DImage : Drawable
    {
        public DImage(string content) : base(content) { }
        public override void Execute(PageBuilder owner)
        {
            owner.AddImage(content);
        }
    }

    GameObject parent;
    float topPadding = 10f;
    float offset = 40f;
    List<float> offsets = new List<float>();
    float viewWidth = 1000;

    string videoReg, imgReg;

    public PageBuilder(GameObject parent) : base()
    {
        this.parent = parent;
        videoReg = @"<video>(.*?)</video>";
        imgReg = @"<img>(.*?)</img>";
        offsets.Add(topPadding);
    }

    public void Clear()
    {
        offsets.Clear();
        offsets.Add(topPadding);
    }

    public float Heigth { get => offsets[offsets.Count - 1]; }

    public void addText(string text)
    {
        var go = new GameObject($"abcd{offsets.Count}");
        Text myText = go.AddComponent<Text>();
        myText.text = text;
        myText.font = UnityEngine.Font.CreateDynamicFontFromOSFont("Arial", 14);
        myText.fontSize = 36;
        AddObject(go, true);
    }

    public void AddObject(GameObject go, bool autoFit=false)
    {
        go.transform.SetParent(parent.transform);
        go.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
        go.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
        go.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        go.transform.localScale = new Vector3(1f, 1f, 1f);
        if (autoFit)
        {
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(viewWidth, 0);
            var cnt = go.AddComponent<ContentSizeFitter>();
            cnt.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            Canvas.ForceUpdateCanvases();
            Debug.Log(go.GetComponent<RectTransform>().rect.height);
        }

        go.transform.localPosition = new Vector3(791.5f, -go.GetComponent<RectTransform>().rect.height / 2 - offsets[offsets.Count - 1], 0f);
        go.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        offsets.Add(offsets[offsets.Count - 1] + go.GetComponent<RectTransform>().rect.height + offset);
    }

    public void AddImage(string imgPath)
    {
        var go = new GameObject($"img{offsets.Count}");
        var img = go.AddComponent<RawImage>();
        // img.color = Color.red;
        // 16:9 480p
        img.rectTransform.sizeDelta = new Vector2(853f, 480f);
        var txtt = new Texture2D(853, 480);
        imgPath = Path.Combine(Application.streamingAssetsPath, imgPath);
        try
        {
            txtt.LoadImage(File.ReadAllBytes(imgPath));
            img.texture = txtt;
        } catch (Exception)
        {
            img.color = Color.red;
        }

        AddObject(go);
    }

    public void AddVideo(string vidPath)
    {
        var go = new GameObject($"vid{offsets.Count}");
        var vid = go.AddComponent<VideoPlayer>();
        vid.isLooping = true;
        var img = go.AddComponent<RawImage>();
        // 16:9 480p
        img.rectTransform.sizeDelta = new Vector2(853f, 480f);
        // Videos\VivesitDomkrat.mp4
        vidPath = Path.Combine(Application.streamingAssetsPath, vidPath);
        if (File.Exists(vidPath))
        {
            vid.url = vidPath;
            vid.errorReceived += (VideoPlayer src, string msg) => { Debug.Log($"Video player error! {msg}"); };
            go.AddComponent<VideoSteamer>();
        }

        AddObject(go);
    }

    public void ParseAndAdd(string text)
    {
        List<Drawable> toDraw = new List<Drawable>() { new DText(text) };
        toDraw = subdivide<DVideo>(toDraw, videoReg);
        toDraw = subdivide<DImage>(toDraw, imgReg);
        // Debug.Log(toDraw);
        foreach (var d in toDraw)
        {
            Debug.Log($"{d} : {d.content}");
            d.Execute(this);
        }
    }

    List<Drawable> subdivide<T>(List<Drawable> ls, string regex) where T : Drawable
    {
        List<Drawable> result = new List<Drawable>();
        foreach (Drawable d in ls)
        {
            RegexOptions options = RegexOptions.Multiline;
            int prevIdx = 0;
            if (d as DText == null)
            {
                result.Add(d);
                continue;
            }
            foreach (Match m in Regex.Matches(d.content, regex, options))
            {
                result.Add(new DText(d.content.Substring(prevIdx, m.Index - prevIdx)));
                result.Add(Activator.CreateInstance(typeof(T), new object[] { m.Groups[1].Value }) as T);
                prevIdx = m.Index + m.Length;
            }
            if (prevIdx != d.content.Length)
            {
                result.Add(new DText(d.content.Substring(prevIdx, d.content.Length - prevIdx)));
            }
        }
        return result;
    }
}
