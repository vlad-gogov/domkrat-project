using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    PageBuilder pg;

    void Start()
    {
        //string filepath = @"Assets\\Texts\\flatTutorial.txt";
        //string tutorial = File.ReadAllText(filepath);
        //string[] tutorialSteps = tutorial.Split(new string[] { "<br>" }, StringSplitOptions.None);
        //for (int i = 0; i < tutorialSteps.Length; i++)
        //{
        //    tutorials[(State)i] = tutorialSteps[i];
        //}

        pg = new PageBuilder(gameObject);
        pg.ParseAndAdd("Alo alo asdsjit ehjrtikeritbhjrdt erhuer yr\n" +
            "ajferjkhtbhjkehjtyrbthryhkbjjrhytb" +
            "tierbtrtybjtrybjk <img>C:\\Users\\rp-re\\Videos\\test.jpg</img>" +
            "asdsdteitv <button>suka blyat</button><button>suka blyat suka blyat suka blyat suka blyat suka blyat suka blyat suka blyat suka blyat suka blyat suka blyat</button>");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
