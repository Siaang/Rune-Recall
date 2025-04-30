using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CardController : MonoBehaviour
{
    [SerializeField] private Cards cardPrefab;
    [SerializeField] private Transform gridTransform;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private TMP_Text countdownText; 
    [SerializeField] private TMP_Text timerText;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject Grid;

    private List<Sprite> spritePairs = new List<Sprite>();
    private List<Cards> allCards = new List<Cards>();

    private Cards firstSelected;
    private Cards secondSelected;

    private bool canSelect = false;
    private float timeElapsed = 0f; 
    private bool isTimerRunning = false;

    

    private void Start()
    {
        PrepareSprites();
        CreateCards();
        StartCoroutine(ShowAllCardsBriefly());
    }

    private void PrepareSprites()
    {
        spritePairs.Clear();
        for (int i = 0; i < sprites.Length; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]);
        }

        ShuffleSprites(spritePairs);
    }

    private void CreateCards()
    {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Cards card = Instantiate(cardPrefab, gridTransform);
            card.SetRuneSprite(spritePairs[i]);
            card.controller = this;
            allCards.Add(card);
        }
    }

    private IEnumerator ShowAllCardsBriefly()
    {
      
        foreach (Cards card in allCards)
        {
            card.Show();
        }

        int countdown = 3; 

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "";

        foreach (Cards card in allCards)
        {
            card.Hide();
        }

        canSelect = true;

        isTimerRunning = true;
        StartCoroutine(UpdateTimer());
    }

    public void SetSelected(Cards card)
    {
        if (!canSelect || card.isSelected || secondSelected != null)
        {
            return;
        }

        card.Show();
        card.isSelected = true;

        if (firstSelected == null)
        {
            firstSelected = card;
        }
        else
        {
            secondSelected = card;
            StartCoroutine(CheckMatching(firstSelected, secondSelected));
        }
    }

    private IEnumerator CheckMatching(Cards a, Cards b)
    {
        yield return new WaitForSeconds(0.5f);

        if (a.RuneSprite == b.RuneSprite)
        {
            Debug.Log("The cards matched!");
            a.isSelected = true;
            b.isSelected = true;
        }
        else
        {
            a.Hide();
            b.Hide();
        }

        firstSelected = null;
        secondSelected = null;

        if (CheckAllCardsMatched())
        {
            StopTimer();
            Debug.Log("🎉 You matched all cards!");
            endScreen.SetActive(true);
            endScreen.GetComponent<EndScreen>().Setup(timeElapsed); 
            Grid.SetActive(false);
        }

    }

    private void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = spriteList[i];
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (isTimerRunning)
        {
            timeElapsed += Time.deltaTime; 
            UpdateTimerDisplay(timeElapsed);
            yield return null;
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        time = Mathf.Max(0, time); 
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private bool CheckAllCardsMatched()
    {
        foreach (Cards card in allCards)
        {
            if (!card.isSelected) return false; 
        }
        return true; 
    }

    private void StopTimer()
    {
        isTimerRunning = false; 
    }
}
