using System;
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
    SET_DOMKRATS = 3,
    CHECK_TURING_MACHANISM = 4,
    UP_TPK = 5,
    CHECK_BREAK_MECHANISM = 6,
    // Flat
    MOVE_TPK_FLAT = 7,
    // Up
    MOVE_TPK_UP = 10,
    // Down
    MOVE_TPK_DOWN = 11,
    DISABLE_TORMOZ = 13,
    // Finish
    DOWN_TPK = 12
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
    CRITICAL = 100
}

public struct Error
{
    public string ErrorText;
    public ErrorWeight Weight;
}

//Singleton.Instance.StateManager.onError(new Error() { ErrorText = "WORK", Weight = 0 });


public class StateManager : MonoBehaviour
{
    [SerializeField] ErrorMessage errorMessage;

    public GameMode gameMode;
    public TypeArea typeArea;
    public int counterMistakes = 0;
    private int maxMistakes = 20;
    public GameObject player;

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

    void Awake()
    {
        // кто изменит 60 на какое-то другое число будет расстрелян Дороничевым из говномета
        Application.targetFrameRate = 60;
        GameObject[] ObjDomkrats = GameObject.FindGameObjectsWithTag("Domkrat");

        foreach (GameObject obj in ObjDomkrats)
        {
            domkrats.Add(obj.GetComponent<Domkrat>());
            obj.SetActive(false);
        }
    }

    void Start()
    {
        gameMode = CrossScenesStorage.gameMode;
        typeArea = CrossScenesStorage.typeArea;

        states.Add(new State() { state = NameState.DEFAULT, disctiption = "" });
        states.Add(new State() { state = NameState.SET_PEREHODNICK, disctiption = "Установите переходники на пакет" });
        states.Add(new State() { state = NameState.CHECK_DOMKRATS, disctiption = "Выполнить проверку подъема и опускание домкарата в разных режимах" });
        states.Add(new State() { state = NameState.SET_DOMKRATS, disctiption = "Подкатите и установите домкраты" });
        states.Add(new State() { state = NameState.CHECK_TURING_MACHANISM, disctiption = "Проверьте работу механизма поворота домкрата и верните ручку домкрата в исходное положение" });
        states.Add(new State() { state = NameState.UP_TPK, disctiption = "Поднимите ТПК" });
        states.Add(new State() { state = NameState.CHECK_BREAK_MECHANISM, disctiption = "Проверьте работу тормозного механизма и отключите тормоз" });

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

        if (states[indexCurState].state == NameState.SET_DOMKRATS)
        {
            NotifyAllDomkrats(states[indexCurState].state);
        }
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

    public void onError(Error error)
    {
        counterMistakes += (int)error.Weight;
        Debug.Log(counterMistakes);
        if (gameMode == GameMode.EXAM && counterMistakes >= maxMistakes)
        {
            Singleton.Instance.UIManager.OpenTutorial("Не сдал!!!!", /*finished=*/true);
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

    void Update()
    {
        if (countPerehodnick == 4)
        {
            NextState();
            countPerehodnick++;
            NotifyAllDomkrats(states[indexCurState].state);
        }
        if (countDomkrats == 4)
        {
            NextState();
            countDomkrats++;
        }
        if (Input.GetKey(KeyCode.F1) && gameMode == GameMode.TRAIN)
        {
            Singleton.Instance.UIManager.OpenTutorial(string.Copy(SafeGetFromDict(tutorials)));
        }
        if (Input.GetKey(KeyCode.F1) && gameMode == GameMode.EXAM)
        {
            Singleton.Instance.UIManager.OpenTutorial(string.Copy(SafeGetFromDict(tutorials, NameState.DEFAULT)));
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

    public void Pause()
    {
        OnPause = true;
        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<PlayerRay>().enabled = false;
    }

    public void Resume()
    {
        OnPause = false;
        player.GetComponent<PlayerMove>().enabled = true;
        player.GetComponent<PlayerRay>().enabled = true;
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
