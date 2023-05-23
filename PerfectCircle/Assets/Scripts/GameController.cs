using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button infoButton;
    public GameObject infoText;
    public Button infoCloseButton;

    public Button soundControl;
    private bool isSoundOn = true;
    private Image soundImage;
    public Sprite mutedSprite;
    public Sprite unmutedSprite;

    public Button goButton;
    public GameObject startText;
    public GameObject hintText;
    public bool isStarted = false;

    public GameObject linePrefab;
    Line activeLine;
    Line oldLine = null;

    private float bestScore = 0.0f;
    private float newScore = 0.0f;
    private Vector2 center = new Vector2(10.0f, 10.0f);


    // Start is called before the first frame update
    void Start()
    {
        infoButton.onClick.AddListener(ShowInfoText);
        infoCloseButton.onClick.AddListener(CloseInfoText);

        soundControl.onClick.AddListener(ChangeSound);
        soundImage = soundControl.gameObject.GetComponent<Image>();

        goButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isStarted)
        {
            if (oldLine != null)
            {
                oldLine.RemoveLine();
            }
            GameObject newLine = Instantiate(linePrefab);
            activeLine = newLine.GetComponent<Line>();
        }

        if (Input.GetMouseButtonUp(0))
        {
            oldLine = activeLine;
            activeLine = null;
        }

        if (activeLine != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(mousePosition);
            Vector2 v2 = mousePosition - center;
            float angle = Mathf.Atan2(v2.y, v2.x);
            Debug.Log(angle);
            // Debug.Log(Vector2.Distance(mousePosition, center));
        }
    }

    void ShowInfoText()
    {
        infoText.SetActive(true);
    }

    void CloseInfoText()
    {
        infoText.SetActive(false);
    }

    void ChangeSound()
    {
        if (isSoundOn)
        {
            isSoundOn = false;
            soundImage.sprite = mutedSprite;
        }
        else
        {
            isSoundOn = true;
            soundImage.sprite = unmutedSprite;
        }
    }

    void StartGame()
    {
        isStarted = true;
        goButton.enabled = false;
        StartCoroutine(TextDotMoves());
        goButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator TextDotMoves()
    {
        RectTransform goButtonTransform = goButton.gameObject.GetComponent<RectTransform>();
        RectTransform startTextTransform = startText.GetComponent<RectTransform>();
        Text text1 = startText.transform.GetChild(0).gameObject.GetComponent<Text>();
        Text text2 = startText.transform.GetChild(1).gameObject.GetComponent<Text>();
        Text hintText1 = hintText.transform.GetChild(0).gameObject.GetComponent<Text>();
        Text hintText2 = hintText.transform.GetChild(1).gameObject.GetComponent<Text>();
        hintText.SetActive(true);
        
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.0005f);
            goButtonTransform.sizeDelta *= 0.987f;
            startTextTransform.localPosition += Vector3.up;
            text2.color = (text1.color = new Color(1f, 1f, 1f, (100 - i) / 100f));
            hintText2.color = (hintText1.color = new Color(1f, 1f, 1f, i / 100f));
        }
        startText.SetActive(false);
    }
}
