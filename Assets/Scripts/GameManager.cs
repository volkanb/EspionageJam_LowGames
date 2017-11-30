using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Sprite[] cardFace;
    public Sprite cardBack;
    public GameObject[] cards;
    public Text matchText;
    public Text timerText;
    public string[] clues;
    public Text guideText;
    public GameObject[] clueTexts;
    public float[] loopTimes;

    private bool _init = false;
    private int _matches = 10;
    private float remainingTime = 65f;

    private int totalNumberOfMatches = 1;
    private int noOfShownClues = 0;
    private int puzzleLoopCount = 0;

    // Update is called once per frame
    void Update() {
        if (!_init)
            initializeCards();

        if (Input.GetMouseButtonUp(0) && !Card.DO_NOT)
            checkCards();

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            SceneManager.LoadScene("EndingLose");
        }
        timerText.text = Mathf.FloorToInt(remainingTime).ToString();
        
        

    }

    void initializeCards()
    {
        for (int id = 0; id < 2; id++)
        {
            for(int i = 1; i < 11; i++)
            {
                bool test = false;
                int choice = 0;
                while (!test)
                {
                    choice = Random.Range(0, cards.Length);
                    test = !(cards[choice].GetComponent<Card>().initialized);
                }
                cards[choice].GetComponent<Card>().carddValue = i;
                cards[choice].GetComponent<Card>().initialized = true;
                
            }
        }

        foreach(GameObject c in cards)        
            c.GetComponent<Card>().setupGraphics();

        if (!_init)
            _init = true;        
    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        return cardFace[i - 1];
    }

    void checkCards() 
    {
        List<int> c = new List<int>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<Card>().state == 1)
                c.Add(i);
        }

        if (c.Count == 2)
            cardComparison(c);
    }

    void cardComparison(List<int> c)
    {
        Card.DO_NOT = true;

        int x = 0;

        if(cards[c[0]].GetComponent<Card>().carddValue == cards[c[1]].GetComponent<Card>().carddValue)
        {
            totalNumberOfMatches++;
            
            x = 2;
            _matches--;
            matchText.text = "Number of Matches: " + _matches;
            if (_matches == 0)                
                guideText.gameObject.SetActive(true);

            if (totalNumberOfMatches % 7 == 0)
                showClue();             
                                    
        }
        for(int i = 0; i < c.Count; i++ )
        {
            cards[c [i]].GetComponent<Card> ().state = x;
            cards[c [i]].GetComponent<Card> ().falseCheck();

        }
    }

    void showClue()
    {
        if (noOfShownClues < 3)
        {
            clueTexts[noOfShownClues].SetActive(true);
            noOfShownClues++;
        }
        
    }

     public int getMatches()
    {
        return _matches;
    }

    public void choseTarget(int i)
    {
        if (i == 7)
            SceneManager.LoadScene("EndingWin");
        else
        {
            foreach (GameObject card in cards)
                card.GetComponent<Card>().rewindCard();
            if (puzzleLoopCount < 3)            
                remainingTime = 40f; 
            else
                remainingTime = loopTimes[puzzleLoopCount];
            puzzleLoopCount++;
            _matches = 10;
            Card.DO_NOT = false;
            StartCoroutine(hideGuide());
        }
    }

    IEnumerator hideGuide()
    {
        guideText.text = "WRONG TARGET!";
        yield return new WaitForSeconds(5);
        guideText.gameObject.SetActive(false);
        guideText.text = "ELIMINATE!";

    }

}
