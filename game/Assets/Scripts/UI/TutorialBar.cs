using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBar : MonoBehaviour
{
    GameObject scrollView;
    GameObject content;
    PageBuilder builder;

    public TutorialBar(GameObject scrollView)
    {
        this.scrollView = scrollView;
        content = scrollView.transform.GetChild(0).GetChild(0).gameObject;
        builder = new PageBuilder(content);
    }

    public void Show(string text)
    {
        // scroll to top
        scrollView.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
        builder.ParseAndAdd(text);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, builder.Heigth);
    }

    public void Hide()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        builder.Clear();
    }
}
