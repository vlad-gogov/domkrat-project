using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    DEFAULT = 0,
    SET_PEREHODNICK = 1,
    CHECK_DOMKRATS = 2,
    SET_DOMKRATS = 3,
    UP_TPK = 4,
    CONFIG_DOMKRAT_TO_FORWARD = 5,
    CONFIG_DOMKRAT_TO_RIGHT = 6
}

public enum GameMode
{
    TRAIN = 0,
    EXAM = 1
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
    public int counterMistaks = 0;

    public List<Domkrat> domkrats = new List<Domkrat>(); 

    private Dictionary<State, string> states = new Dictionary<State, string>();
    private State curState;

    public int countPerehodnick = 0;
    public int countDomkrats = 0;


    void Awake()
    {
        gameMode = GameMode.TRAIN;
        typeArea = TypeArea.FLAT;
        states.Add(State.DEFAULT, "");
        states.Add(State.SET_PEREHODNICK, "Установите переходники на пакет");
        states.Add(State.CHECK_DOMKRATS, "Выполнить проверку подъема и опускание домкарата в разных режимах");
        states.Add(State.SET_DOMKRATS, "Подкатите и установите домкраты");
        states.Add(State.UP_TPK, "Поднимите ТПК");

        if (typeArea == TypeArea.FLAT)
        {
            states.Add(State.CONFIG_DOMKRAT_TO_FORWARD, "Установите домкарты для перемещения вперед и нажмите стрелку вверх");
            states.Add(State.CONFIG_DOMKRAT_TO_RIGHT, "Установите домкарты для перемещения вправо и нажмите стрелку вправо");
        }

        else if (typeArea == TypeArea.UP)
        {

        }

        else if (typeArea == TypeArea.DOWN)
        {

        }

        curState = State.DEFAULT;
        NextState();
    }

    public void NextState()
    {
        curState = (State)((int)curState + 1);
        Debug.Log(curState);
        if (curState == State.SET_DOMKRATS)
        {
            NotifyAllDomkrats(curState);
        }
        if (gameMode == GameMode.TRAIN)
        {
            ChangeTextHelper();
        }
    }

    public void ChangeTextHelper()
    {
        int index = (int)curState;
        Singleton.Instance.UIManager.SetHelperText(string.Copy(states[curState]));
    }

    public void onError(Error error)
    {
        if (gameMode == GameMode.TRAIN)
            errorMessage.OnShow(string.Copy(error.ErrorText));
        counterMistaks += (int)error.Weight;
    }

    void NotifyAllDomkrats(State state)
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
            NotifyAllDomkrats(curState);
        }
        if (countDomkrats == 4)
        {
            NextState();
            countDomkrats++;
        }
    }
}
