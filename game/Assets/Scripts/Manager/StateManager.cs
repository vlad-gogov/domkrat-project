﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum NameState
{
    DEFAULT = 0,
    SET_PEREHODNICK = 1,
    CHECK_DOMKRATS = 2,
    GET_DOMKRATS = 3,
    SET_DOMKRATS = 4,
    CHECK_TURING_MACHANISM = 5,
    UP_TPK = 6,
    CHECK_BREAK_MECHANISM = 7,
    // Flat
    MOVE_TPK_FLAT = 8,
    // Up
    MOVE_TPK_UP = 9,
    // Down
    MOVE_TPK_DOWN = 10,
    DISABLE_TORMOZ = 11,
    // Finish
    DOWN_TPK = 12,
    RETURN_DOMKRATS=13,
}

public struct State
{
    public NameState state;
    public string disctiption;
}

public enum GameMode
{
    TRAIN = 0,
    EXAM = 1,
    FREEPLAY = 2,
}

public enum TypeArea
{
    FLAT = 0,
    UP = 1,
    DOWN = 2
}

public enum ErrorWeight
{
    MINOR = 0,
    LOW = 1,
    MEDIUM = 2,
    HIGH = 3,
    UBIL = 9,
    CRITICAL = 100
}

public struct Error
{
    public string ErrorText;
    public ErrorWeight Weight;
}

public class StateManager : MonoBehaviour
{
    [SerializeField] ErrorMessage errorMessage;

    public GameMode gameMode;
    public TypeArea typeArea;
    public int counterMistakes = 0;
    private int maxMistakes = 20;
    public GameObject player;
    public List<Error> errors = new List<Error>();

    private List<Domkrat> domkrats = new List<Domkrat>();

    public bool OnPause { get; private set; }
    private List<State> states = new List<State>();
    Dictionary<NameState, string> tutorials = new Dictionary<NameState, string>();
    private int indexCurState;
    // Переменная, проверяющая завершили ли мы сценарий
    bool finished = false;

    public int countPerehodnick = 0;
    public int countDomkrats = 0;

    public bool isTuringMech = false;
    public int ruchkaIsUp = 0;
    public int isTormozConnected = 0;
    public bool isErrorOpened = false;
    string controlsTutorial;

    void Awake()
    {
        // кто изменит 60 на какое-то другое число будет расстрелян Дороничевым из говномета
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        GameObject[] ObjDomkrats = GameObject.FindGameObjectsWithTag("Domkrat");

        foreach (GameObject obj in ObjDomkrats)
        {
            domkrats.Add(obj.GetComponent<Domkrat>());
            //obj.SetActive(false);
        }
    }

    void Start()
    {
        gameMode = CrossScenesStorage.gameMode;
        typeArea = CrossScenesStorage.typeArea;

        states.Add(new State() { state = NameState.DEFAULT, disctiption = "" });
        states.Add(new State() { state = NameState.SET_PEREHODNICK, disctiption = "Установите переходники на пакет" });
        states.Add(new State() { state = NameState.CHECK_DOMKRATS, disctiption = "Выполнить проверку подъема и опускание домкарата в разных режимах" });
        states.Add(new State() { state = NameState.GET_DOMKRATS, disctiption = "Снимите домкраты со стойки" });
        states.Add(new State() { state = NameState.SET_DOMKRATS, disctiption = "Отсоедените оставшиеся домкраты, подкатите и установите их на ТПК" });
        states.Add(new State() { state = NameState.CHECK_TURING_MACHANISM, disctiption = "Проверьте работу механизма поворота домкрата и верните ручку домкрата в исходное положение" });
        states.Add(new State() { state = NameState.UP_TPK, disctiption = "Поднимите ТПК" });
        states.Add(new State() { state = NameState.CHECK_BREAK_MECHANISM, disctiption = "Проверьте работу тормозного механизма, отключите тормоз и выключите тормозной механизм" });

        LoadTutorial();

        if (typeArea == TypeArea.FLAT)
        {
            states.Add(new State() { state = NameState.MOVE_TPK_FLAT, disctiption = "Переместите ТПК в красную точку" });
        }
        else if (typeArea == TypeArea.UP)
        {
            states.Add(new State() { state = NameState.MOVE_TPK_UP, disctiption = "Закатите ТПК по наклонной поверхности до красной точки" });
            states.Add(new State() { state = NameState.DISABLE_TORMOZ, disctiption = "Отключите внешний тормоз от домкратов" });
        }
        else if (typeArea == TypeArea.DOWN)
        {
            states.Add(new State() { state = NameState.MOVE_TPK_DOWN, disctiption = "Скатите ТПК по наклонной поверхности до красной точки" });
            states.Add(new State() { state = NameState.DISABLE_TORMOZ, disctiption = "Отключите внешний тормоз от домкратов" });
        }

        states.Add(new State() { state = NameState.DOWN_TPK, disctiption = "Опустите ТПК на землю" });
        states.Add(new State() { state = NameState.RETURN_DOMKRATS, disctiption = "Отсоедените домкраты от ТПК и верните их на обратно на стойку" });

        indexCurState = 0;

        if (gameMode == GameMode.FREEPLAY)
        {
            NotifyAllDomkrats(NameState.CHECK_DOMKRATS);
            NotifyAllDomkrats(NameState.SET_DOMKRATS);
            // Переходим в самое последнее состояние, чтобы открыть все части для свободной игры
            for (int i = 0; i < states.Count - 1; i++)
            {
                NextState();
            }
            finished = true;
        }
        ChangeTextHelper();
    }

    public void NextState()
    {
        indexCurState++;
        if (indexCurState >= states.Count)
        {
            finished = true;
            return;
        }

        if (typeArea != TypeArea.FLAT && states[indexCurState].state > NameState.CHECK_BREAK_MECHANISM)
        {
            TPK.TPKObj.SwitchMovingThings(true);
        }
        if (typeArea != TypeArea.FLAT && states[indexCurState].state > NameState.DOWN_TPK)
        {
            TPK.TPKObj.SwitchMovingThings(false);
        }
        NotifyAllDomkrats(states[indexCurState].state);
        ChangeTextHelper();
    }

    public void ChangeTextHelper()
    {
        Singleton.Instance.UIManager.SetHelperText(string.Copy(states[indexCurState].disctiption));
        if (gameMode == GameMode.TRAIN)
        {
            Singleton.Instance.UIManager.OpenTutorial(string.Copy(SafeGetFromDict(tutorials)));
        } else if (gameMode == GameMode.EXAM && states[indexCurState].state == NameState.DEFAULT)
        {
            Singleton.Instance.UIManager.OpenTutorial(string.Copy(SafeGetFromDict(tutorials)));
        }
    }

    public void DomkratStoikaDisconnect()
    {
        if (GetState() == NameState.GET_DOMKRATS)
        {
            NextState();
        }
    }

    public void onError(Error error)
    {
        if (isErrorOpened)
        {
            return;
        }
        if (error.Weight != ErrorWeight.MINOR)
        {
            errors.Add(error);
        }
        isErrorOpened = true;
        counterMistakes += (int)error.Weight;
        if (gameMode == GameMode.EXAM && counterMistakes >= maxMistakes)
        {
            Singleton.Instance.UIManager.OpenTutorial("<size=50><b>Неудача</b></size>\n" +
                "\n" +
                "Вы набрали слишком много штрафных баллов, экзамен не сдан! " +
                "Максимальное число штрафных баллов для сдачи: " + maxMistakes.ToString() + "\n" +
                "Набрано: " + counterMistakes.ToString(), /*finished=*/true);
        }
        errorMessage.OnShow(error);
    }

    void NotifyAllDomkrats(NameState state)
    {
        foreach(Domkrat domkrat in domkrats)
        {
            domkrat.Notify(state);
        }
    }

    public void Finish()
    {
        string finishString = "<size=50><b>Сценарий успешно завершен!</b></size>" +
           "\n\n" +
           "Сумма набранных штрафных баллов: ";
        Singleton.Instance.UIManager.OpenTutorial(finishString + Singleton.Instance.StateManager.counterMistakes.ToString(), /*finished=*/true);
    }
        

void Update()
    {
        if (countPerehodnick == 4)
        {
            NextState();
            countPerehodnick++;
            NotifyAllDomkrats(states[indexCurState].state);
        }
        if (GetState() == NameState.SET_DOMKRATS && countDomkrats == 4)
        {
            NextState();
            // countDomkrats++;
        }
        if (Input.GetKey(KeyCode.F1) && gameMode == GameMode.TRAIN)
        {
            Singleton.Instance.UIManager.OpenTutorial(string.Copy(SafeGetFromDict(tutorials)), /*finished=*/false, /*resetScrollBar=*/false);
        }
        if (Input.GetKey(KeyCode.F1) && gameMode == GameMode.EXAM)
        {
            Singleton.Instance.UIManager.OpenTutorial(string.Copy(SafeGetFromDict(tutorials, NameState.DEFAULT)));
        }
        if (Input.GetKey(KeyCode.F2))
        {
            Singleton.Instance.UIManager.OpenTutorial(controlsTutorial);
        }
        if (GetState() == NameState.DISABLE_TORMOZ && isTormozConnected == 0)
        {
            NextState();
        }
    }

    public NameState GetState()
    {
        return states[indexCurState].state;
    }

    public void Pause(bool stopTime = false)
    {
        OnPause = true;
        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<PlayerRay>().enabled = false;
        if (stopTime)
        {
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        OnPause = false;
        player.GetComponent<PlayerMove>().enabled = true;
        player.GetComponent<PlayerRay>().enabled = true;
        Time.timeScale = 1;
    }

    void LoadTutorial()
    {
        string filepath = @"Texts\\flatTutorial.txt";
        filepath = Path.Combine(Application.streamingAssetsPath, filepath);
        string tutorial = File.ReadAllText(filepath);
        string[] tutorialSteps = tutorial.Split(new string[] { "<br>" }, StringSplitOptions.None);
        for (int i=0; i<tutorialSteps.Length; i++)
        {
            var tutorLine = tutorialSteps[i].Trim();
            string modeStr = GetFirstLine(tutorLine);
            var match = Regex.Match(modeStr, "(.*):(.*)");
            if (!match.Success)
            {
                // Debug.LogError($"No state for tuturial page was specified! {tutorLine}");
                continue;
            }
            string state = match.Groups[1].Value;
            string mode = match.Groups[2].Value;
            if (mode != "")
            {
                var tp = (TypeArea)Enum.Parse(typeof(TypeArea), mode, /*ignore_case=*/true);
                if (tp != typeArea)
                {
                    continue;
                }
            }
            var st = (NameState)Enum.Parse(typeof(NameState), state, /*ignore_case=*/true);
            tutorials[st] = RemoveFirstLine(tutorLine);
        }

        // load constrol tutorial
        filepath = @"Texts\\controlsTutorial.txt";
        filepath = Path.Combine(Application.streamingAssetsPath, filepath);
        controlsTutorial = File.ReadAllText(filepath);
    }

    string RemoveFirstLine(string str, int nlines=1)
    {
        return string.Join(Environment.NewLine, Regex.Split(str, "\r\n|\r|\n").Skip(nlines));
    }

    string GetFirstLine(string str)
    {
        var reader = new StringReader(str);
        return reader.ReadLine();
    }

    public void InitialStateHack()
    {
        if (states[indexCurState].state == NameState.DEFAULT)
        {
            NextState();
        }
    }

    public string SafeGetFromDict(Dictionary<NameState, string> dict, NameState? state = null)
    {
        string value;
        if (state == null)
        {
            state = states[indexCurState].state;
        }
        if (!dict.TryGetValue(state == null? NameState.DEFAULT : (NameState)state, out value))
        {
            value = $"Для состояния {states[indexCurState].state} ещё не написаны подсказки...";
        }
        return value;
    }
}
