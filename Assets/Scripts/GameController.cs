using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Rock - Paper - Scissors : starter script
// Follow the slides and complete every TODO.
// 1 = Rock, 2 = Paper, 3 = Scissors, 4 = Unknown ("?")
public class GameController : MonoBehaviour
{
    Image imgYou;       // your selected image (rock, paper, scissor)
    Image imgCom;       // computer selected image (rock, paper, scissor)

    Text txtYou;        // the score you win
    Text txtCom;        // the score computer win
    Text txtResult;     // the result

    int cntYou = 0;     // number you win
    int cntCom = 0;     // number computer win
    bool gameReady;


    private bool InitGame()
    {
        // TODO (slides 37-40): find the UI objects by name and get their components.
        // Example:
        // imgYou = GameObject.Find("ImgYou").GetComponent<Image>();
        // Also find: ImgCom, the score Text children, and TxtResult_.
        imgYou = FindUIComponent<Image>("ImgYou", "imgYou");
        imgCom = FindUIComponent<Image>("ImgCom", "imgCom");
        txtYou = FindScoreText("TxtYou");
        txtCom = FindScoreText("TxtCom");
        txtResult = FindUIComponent<Text>("TxtResult_", "TxtResult", "TextResult");

        if (imgYou == null || imgCom == null || txtYou == null || txtCom == null || txtResult == null)
        {
            return false;
        }

        // TODO: set the text of txtResult to "Select the button below"
        txtResult.text = "Select the button below";
        return true;
    }

    T FindUIComponent<T>(params string[] objectNames) where T : Component
    {
        GameObject uiObject = null;
        foreach (string objectName in objectNames)
        {
            uiObject = GameObject.Find(objectName);
            if (uiObject != null)
            {
                break;
            }
        }

        if (uiObject == null)
        {
            Debug.LogError("Game UI is missing an active object named " + string.Join(" or ", objectNames) + ".");
            return null;
        }

        T component = uiObject.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("Game UI object " + uiObject.name + " is missing a " + typeof(T).Name + " component.");
        }
        return component;
    }

    Text FindScoreText(string labelName)
    {
        GameObject label = GameObject.Find(labelName);
        if (label == null)
        {
            Debug.LogError("Game UI is missing an active score label named " + labelName + ".");
            return null;
        }

        Transform scoreObject = label.transform.Find("Text");
        if (scoreObject == null && label.transform.parent != null)
        {
            scoreObject = label.transform.parent.Find("Text");
        }

        if (scoreObject == null)
        {
            Debug.LogError("Game UI object " + labelName + " and its parent are missing a child named Text.");
            return null;
        }

        Text text = scoreObject.GetComponent<Text>();
        if (text == null)
        {
            Debug.LogError("Score object " + scoreObject.name + " is missing a Legacy Text component.");
        }
        return text;
    }

    void Start()
    {
        // init the game
        gameReady = InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        // exit if press escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Slides 24-31: link this function to the On Click () event of every button.
    // Pass the button itself as the GameObject parameter.
    public void OnButtonClick(GameObject buttonObject)
    {
        if (buttonObject == null)
        {
            Debug.LogError("OnButtonClick received no button.");
            return;
        }

        if (!gameReady)
        {
            return;
        }

        Debug.Log(buttonObject.name + " clicked ");

        int you;
        if (!TryGetChoice(buttonObject.name, out you))
        {
            Debug.LogError("Unsupported choice button name: " + buttonObject.name);
            return;
        }

        CheckResult(you);
    }

    bool TryGetChoice(string buttonName, out int choice)
    {
        choice = 0;
        if (string.IsNullOrEmpty(buttonName))
        {
            return false;
        }

        char firstCharacter = buttonName[0];
        if (firstCharacter >= '1' && firstCharacter <= '3')
        {
            choice = firstCharacter - '0';
            return true;
        }

        string normalizedName = buttonName.Replace("_", "").Replace(" ", "").ToLowerInvariant();
        switch (normalizedName)
        {
            case "rock":
            case "rockbutton":
                choice = 1;
                return true;
            case "paper":
            case "paperbutton":
                choice = 2;
                return true;
            case "scissor":
            case "scissors":
            case "scissorbutton":
            case "scissorsbutton":
                choice = 3;
                return true;
            default:
                return false;
        }
    }

    void CheckResult(int yourResult)
    {
        // TODO (slide 33): game logic
        // 1. com = random number 1..3  ->  UnityEngine.Random.Range(1, 4)
        int com = UnityEngine.Random.Range(1, 4);
        // 2. k = you - com
        int k = yourResult - com;
        // 3. k == 0              -> "Draw."
        //    k == 1 or k == -2   -> "You win."       (cntYou++)
        //    otherwise           -> "Computer win."  (cntCom++)
        if (k == 0)
        {
            txtResult.text = "Draw.";
        }
        else if (k == 1 || k == -2)
        {
            txtResult.text = "You win.";
            cntYou++;
        }
        else
        {
            txtResult.text = "Computer win.";
            cntCom++;
        }
        // 4. call SetResult(yourResult, comResult)
        SetResult(yourResult, com);

        Debug.Log("Your result is: " + yourResult);
    }

    void SetResult(int you, int com)
    {
        // TODO (slide 41):
        // 1. change imgYou.sprite and imgCom.sprite
        //    Resources.Load("img_" + you, typeof(Sprite)) as Sprite
        imgYou.sprite = Resources.Load("img_" + you, typeof(Sprite)) as Sprite;
        imgCom.sprite = Resources.Load("img_" + com, typeof(Sprite)) as Sprite;

        // 2. flip the computer image on the x axis: localScale = (-1, 1, 1)
        imgCom.transform.localScale = new Vector3(-1, 1, 1);

        // 3. update txtYou.text and txtCom.text with the scores
        txtYou.text = cntYou.ToString();
        txtCom.text = cntCom.ToString();
    }
}
