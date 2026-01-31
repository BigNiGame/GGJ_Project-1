using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum MaskType
{
    Sad,
    Anger,
    Confuse,
    Anxiety
}

public enum PlayerState
{
    Idle,
    InAction
}
public enum Station
{
    Investigate = 0,
    Curing = 1
}
public class MainStall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image blackScreen;
    public static MainStall instance;
    public int todayPatient;
    public int currentIndex = 0;
    public GameObject currentPatientGO;
    private bool isScreenChanging;
    public Patient currentPatient;
    public float blackScreenHoldTime = 0.5f;  
    public Station currentStation = Station.Investigate;
    public TextMeshProUGUI correct,
        wrong,
        indexT,
        info,
        paperFirstName,
        paperLastName,
        paperAge,
        paperGender,
        paperContent;
    
    private int c, w, i;
    public GameObject patientPF, questionBlock, dialogueBlock, closePaperBGO,dayFinishNoti;
    public TextAsset firstname, lastname;
    string[] firstNames;
    string[] lastNames;
    public RectTransform paperRect, paperBRect;
    public bool paperOn;
    private Vector2 paperTpos, paperBTpos;
    public TextRenderer dialogue;
    List<string> availableNames = new List<string>();
    public Button feelB, dotoB;
    public PlayerState playerState = PlayerState.Idle;
    public float idleCoolTime, waitTimer;
    GameManager gameManager;
    public List<Patient> patients = new List<Patient>();
    public TextMeshProUGUI currentDayTxt, quotaTxt;
    void Awake()
    {
        firstNames = firstname.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        lastNames = lastname.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        GenerateAllNames();
    }

    void Start()
    {
        instance = this;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        todayPatient = Random.Range(3, 5);
        SpawnPatient();
        currentDayTxt.text = "Day "+gameManager.currentDay.ToString();
        quotaTxt.text = "Quota "+gameManager.currentPatientCuredThisQuota.ToString() + "/" + gameManager.quotaPatient.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        indexT.text = i.ToString();
       
        if (currentPatient != null)
        {
            info.text = currentPatient.correctMask.ToString();
            feelB.interactable = !currentPatient.feelAsked;
            dotoB.interactable = !currentPatient.todoAsked;
            
            UpdatePaper();
          
            if (playerState == PlayerState.Idle && !dialogueBlock.activeSelf)
            {
                if (idleCoolTime > 0)
                {
                    idleCoolTime -= Time.deltaTime;
                }
                else
                {
                    idleCoolTime = 0;
                    if (!currentPatient.randomText1Talked ||
                        (currentPatient.has2Rando && !currentPatient.randomText2Talked))
                    {
                        waitTimer += Time.deltaTime;
                        if (waitTimer >= currentPatient.timeTilTalk)
                        {
                            RandomTalk();
                            waitTimer = 0;
                        }
                    }
                }
              
            }
            else
            {
                idleCoolTime = 1;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            MouseClick();
        }
        if (paperOn)
        {
            paperTpos = new Vector2(-400, 0);
            paperBTpos = new Vector2(-30, 0);
        }
        else
        {
            paperTpos = new Vector2(400, 0);
            paperBTpos = new Vector2(30, 0);
        }
        paperRect.anchoredPosition = Vector2.Lerp(paperRect.anchoredPosition, paperTpos, Time.deltaTime * 50);
        paperBRect.anchoredPosition = Vector2.Lerp(paperBRect.anchoredPosition, paperBTpos, Time.deltaTime * 50);
        
    }

    void RandomTalk()
    {
        dialogueBlock.SetActive(true);

        if (!currentPatient.randomText1Talked)
        {
            dialogue.StartTyping(currentPatient.randomtxt1);
            currentPatient.randomText1Talked = true;
        }
        else if (currentPatient.has2Rando && !currentPatient.randomText2Talked)
        {
            dialogue.StartTyping(currentPatient.randomtxt2);
            currentPatient.randomText2Talked = true;
        }
    }

    void MouseClick()
    {
        dialogue.SkipTyping();
        if (dialogue.canClose)
        {
            dialogueBlock.SetActive(false);
        }
    }

    public void UpdatePaper()
    {
        paperFirstName.text = "Name: " + currentPatient.patientFirstName;
        paperLastName.text = "Last Name: " + currentPatient.patientLastName;
        paperAge.text = "Age: " + currentPatient.age.ToString();
        paperGender.text = "Gender: " + currentPatient.gender.ToString();
  
      

        paperContent.text = currentPatient.historytxt;
    }

    void GenerateAllNames()
    {
        foreach (string first in firstNames)
        {
            string f = first.Trim();

            // First name only
            availableNames.Add(f);

            // First + Last
            foreach (string last in lastNames)
            {
                availableNames.Add(f + " " + last.Trim());
            }
        }

        ShuffleList(availableNames);
    }

    void ShuffleList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    public void Finish()
    {
        StartCoroutine(CureCutScene());
    }
    public string GetRandomName()
    {
        if (availableNames.Count == 0)
            return "Tewi Inaba :3";

        string name = availableNames[0];
        availableNames.RemoveAt(0);
        return name;
    }

    public void SpawnPatient()
    {
        i++;

        GameObject patientGO = Instantiate(patientPF, new Vector3(15, 0, 0), Quaternion.identity);
        Patient patient = patientGO.GetComponent<Patient>();
        currentPatient = patient;
        currentPatientGO = patientGO;
    }

    public void TalkAction(int i)
    {
        dialogueBlock.SetActive(true);
        switch (i)
        {
            case 0:

                break;
            case 1:
                dialogue.StartTyping(currentPatient.feeltxt);
                currentPatient.feelAsked = true;
                break;
            case 2:
                dialogue.StartTyping(currentPatient.todotxt);
                currentPatient.todoAsked = true;
                break;
        }

        ToggleQuestion();
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(1);
        SpawnPatient();
    }
    IEnumerator CureCutScene()
    {
        yield return new WaitForSeconds(1);
        foreach (Patient p in patients)
        {
           p.patientSprite.targetPosition = new Vector3(30, 0);
           p.patientState = PatientState.WalkIn;
           yield return new WaitForSeconds(4);
           if (p.maskGet == p.correctMask)
           {
               p.patientSprite.targetPosition = new Vector3(15, 0);
               p.patientState = PatientState.Leaving;
               
           }
           else
           {
               p.agressive = true;
               yield return new WaitForSeconds(3);
               p.patientSprite.targetPosition = new Vector3(15, 0);
               p.patientState = PatientState.Leaving;
           }
           yield return new WaitForSeconds(1);
           if (p.maskGet == p.correctMask)
           {
               gameManager.currentPatientCuredThisQuota++;
               
           }
           else
           {
               if (gameManager.currentPatientCuredThisQuota > 0)
               {
                   bool r = Random.value > 0.5f;
                   if (r)
                   {
                       gameManager.currentPatientCuredThisQuota--;
                   }
               }
           }
           quotaTxt.text = "Quota "+gameManager.currentPatientCuredThisQuota.ToString() + "/" + gameManager.quotaPatient.ToString();
           yield return new WaitForSeconds(1);
           
        }
    }
    public void GiveMask(int maskInt)
    {
        if (currentPatient == null)
            return;
        if (currentPatient.patientState == PatientState.Leaving || currentPatient.patientState == PatientState.WalkIn)
            return;
        idleCoolTime = 5;
        waitTimer = 0;
        questionBlock.SetActive(false);
        paperOn = false;
        dialogue.SkipTyping();
        dialogue.StopAllCoroutines();
        dialogue.canClose = true;
        dialogueBlock.SetActive(false);
        MaskType mask = (MaskType)maskInt;
        currentPatient.maskGet = mask;
        if (mask == currentPatient.correctMask)
        {
            c++;
            correct.text = c.ToString();
        }
        else
        {
            w++;
            wrong.text = w.ToString();
        }

        currentPatient.Maskgiven();
        patients.Add(currentPatient);
        currentPatient = null;
        if (i < todayPatient)
        {
            StartCoroutine(SpawnDelay());
        }
        else
        {
            StartCoroutine(DayFinishTimer());
        }
    }

    IEnumerator DayFinishTimer()
    {
        yield return new WaitForSeconds(2.5f);
        DayFinish();
    }
    void DayFinish()
    {
        dayFinishNoti.SetActive(true);
    }
    public void ToggleQuestion()
    {
        questionBlock.SetActive(!questionBlock.activeSelf);
        if (questionBlock.activeSelf)
        {
            playerState = PlayerState.InAction;
        }
        else
        {
            playerState = PlayerState.Idle;
        }
    }
    

    public void TogglePaper()
    {
        paperOn = !paperOn;
    }

    public void ClosePaper()
    {
        paperOn = false;
    }
    public void SetCamera(int station)
    {
        if (isScreenChanging) return;
        if((int)currentStation == station) return;
   
        float xPos = 0f;
        switch (station)
        {
            case 0: xPos = 0f;
            
                break;   
            case 1: xPos = 30f; 
                dayFinishNoti.SetActive(false);
                paperOn = false;
                break;  
            default: xPos = 0f; break;
        }
        StartCoroutine(CameraTransitionRoutine(xPos));

        // Update currentStation
        currentStation = (Station)station;
        
    }
    private IEnumerator CameraTransitionRoutine(float x)
    {
        isScreenChanging = true;
    
        

        // --- Fade to black ---
        yield return StartCoroutine(FadeScreen(1));

        Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        // --- Hold black screen for a short moment ---
        yield return new WaitForSeconds(blackScreenHoldTime);
        
        // --- Move camera instantly ---
      

        // --- Fade back in ---
        yield return StartCoroutine(FadeScreen(0));

        // --- Wait for button cooldown ---
        Finish();
        isScreenChanging = false;
       
    }

    private IEnumerator FadeScreen(float targetAlpha)
    {
        Color color = blackScreen.color;
        while (Mathf.Abs(color.a - targetAlpha) > 0.01f)
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, Time.deltaTime * 5);
            blackScreen.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        blackScreen.color = color;
    }
}