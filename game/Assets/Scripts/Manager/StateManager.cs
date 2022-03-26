using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    DEFAULT = 0,
    SET_PEREHODNICK = 1,
    CHECK_DOMKRATS = 2,
    SET_DOMKRATS = 3,
    UP_TPK = 4
}

public enum GameMode
{
    TRAIN = 0,
    EXAM = 1
}

public class StateManager : MonoBehaviour
{
    public List<Domkrat> domkrats; 

    private Dictionary<State, string> states = new Dictionary<State, string>();
    private State curState;

    // Ñ÷åò÷èêè äëÿ êîë-âà
    public int countPerehodnick = 0;
    public int countDomkrats = 0;


    void Awake()
    {
        states.Add(State.DEFAULT, "");
        states.Add(State.SET_PEREHODNICK, "Óñòàíîâèòå ïåðåõîäíèêè íà ïàêåò");
        states.Add(State.CHECK_DOMKRATS, "Âûïîëíèòü ïðîâåðêó ïîäúåìà è îïóñêàíèå äîìêàðàòà â ðàçíûõ ðåæèìàõ");
        states.Add(State.SET_DOMKRATS, "Ïîäêàòèòå è óñòàíîâèòå äîìêðàòû");
        states.Add(State.UP_TPK, "Ïîäíèìèòå ÒÏÊ");
        
        curState = State.DEFAULT;
        NextState();
    }

    public void NextState()
    {
        Debug.Log(curState);
        curState = (State)((int)curState + 1);
        Debug.Log(curState);
        ChangeTextHelper();
    }

    public void ChangeTextHelper()
    {
        int index = (int)curState;
        Singleton.Instance.UIManager.SetHelperText(string.Copy(states[curState]));
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
            NotifyAllDomkrats(curState);
            countPerehodnick++;
        }
        if (countDomkrats == 4)
        {
            NextState();
            countDomkrats++;
        }
    }
}
