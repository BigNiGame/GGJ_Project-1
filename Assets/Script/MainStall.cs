using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject patientPF, questionBlock, dialogueBlock,dayFinishNoti,settingGO,settingBGO, documentGO,documentBGO;
    public TextAsset firstname, lastname;
    string[] firstNames;
    string[] lastNames; 
    public bool paperOn;
    public TextRenderer dialogue;
    List<string> availableNames = new List<string>();
    public Button feelB, dotoB;
    public PlayerState playerState = PlayerState.Idle;
    public float idleCoolTime, waitTimer;
    GameManager gameManager;
    public List<Patient> patients = new List<Patient>();
    public TextMeshProUGUI currentDayTxt, quotaTxt;
    public MaskType selectedMask;
    private bool maskSelected;
    public GameObject sadI,angerI,confuseI,anxietyI;
    public Sprite red,green;
    public SpriteRenderer gateLight1,gateLight2;
    public GameObject gate1,gate2;
    public bool gateOpen;
    public MaskType randomMask;
    public GameObject summaryGO;
    Tween gate1Ani, gate2Ani;
    private bool fail;
    public TextMeshProUGUI tpT, cmT, plT,quotaT;
    public Image judgeI;
    public Sprite passSp, failSp;
    public GameObject homeB, restartB, nextB;
    public ParticleSystem german1, german2, german3;
    public AudioSource music;
    void Awake()
    {
        
        instance = this;
        firstNames = firstname.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        lastNames = lastname.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        GenerateAllNames();
    }

    void Start()
    {
       
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        todayPatient = Random.Range(3, 5);
     
        currentDayTxt.text = gameManager.currentDay.ToString();
        quotaTxt.text =gameManager.currentPatientCuredThisQuota.ToString();
    }

    public void GameStart()
    {
        SpawnPatient();
        GateOn();
        
    }
    // Update is called once per frame
    void Update()
    {
        indexT.text = i.ToString();
        music.volume = Audio.instance.musicVolume * 0.1f;
        if (currentPatient != null)
        {
            info.text = currentPatient.correctMask.ToString();
            feelB.interactable = !currentPatient.feelAsked;
            dotoB.interactable = !currentPatient.todoAsked;
            if (currentPatient.patientState == PatientState.Idle || currentPatient.patientState == PatientState.Leaving)
            {
                
            }
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
     
        sadI.SetActive(false);
        angerI.SetActive(false);
        confuseI.SetActive(false);
        anxietyI.SetActive(false);
        if (maskSelected)
        {
            switch (selectedMask)
            {
                
                case MaskType.Sad:
                    sadI.SetActive(true);
                    break;
                case MaskType.Anger:
                    angerI.SetActive(true);
                    break;
                case MaskType.Confuse:
                    confuseI.SetActive(true);
                    break;
                case MaskType.Anxiety:
                    anxietyI.SetActive(true);
                    break;
            }
        }
       
       
    }
    public void SettingOn()
    {
        Audio.instance.PlaySound("Button");
        settingGO.transform.DOMoveX(-7.25f, 0.5f).SetEase(Ease.OutQuad);
        settingBGO.transform.DOMoveX(-11f, 0.5f).SetEase(Ease.OutQuad);
    }
    public void SettingOff()
    {
        Audio.instance.PlaySound("Button");
        settingGO.transform.DOMoveX(-12.25f, 0.5f).SetEase(Ease.InQuad);
        settingBGO.transform.DOMoveX(-9.2f, 0.5f).SetEase(Ease.InQuad);
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
        GateOn();
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
        Audio.instance.PlaySound("Button");
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
        int loss = 0;
        foreach (Patient p in patients)
        {
           p.patientSprite.targetPosition = new Vector3(30, 0);
           p.patientState = PatientState.WalkIn;
         
           yield return new WaitForSeconds(1.5f);
           german1.Play();
           german2.Play();
           german3.Play();
           Color c = Color.white;
           switch (p.maskGet)
           {
               case MaskType.Sad:
                   ColorUtility.TryParseHtmlString("#26C5FF", out c);
                   break;

               case MaskType.Anger:
                   ColorUtility.TryParseHtmlString("#FF2626", out c);
                   break;

               case MaskType.Confuse:
                   ColorUtility.TryParseHtmlString("#939393", out c);
                   break;
               case MaskType.Anxiety:
                   ColorUtility.TryParseHtmlString("#DBC93A", out c);
                   break;
               
                   
               
           }
           
           german1.startColor = c;
           german2.startColor = c;
           german3.startColor = c;
           yield return new WaitForSeconds(2);
           german1.Stop();
           german2.Stop();
           german3.Stop();
           yield return new WaitForSeconds(1);
           if (p.maskGet == p.correctMask)
           {
               yield return new WaitForSeconds(1.5f);
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
                       loss++ ;
                   }
               }
           }
           quotaTxt.text =gameManager.currentPatientCuredThisQuota.ToString();
           yield return new WaitForSeconds(1);
           
        }

        summaryGO.transform.DOMoveY(0, 0.5f).SetEase(Ease.InQuad);
        tpT.text = "Total Patient Today: " + i.ToString();
        cmT.text = "Correct Mask: " + c.ToString();
        plT.text = "Patient Loss: " + loss.ToString();
        if (gameManager.quotaDay == gameManager.currentDay)
        {
            quotaT.gameObject.SetActive(true);
            judgeI.gameObject.SetActive(true);
            if (gameManager.currentPatientCuredThisQuota >= 4)
            {
                judgeI.sprite = passSp;
                homeB.SetActive(false);
                restartB.SetActive(false);
                nextB.SetActive(true);
            }
            else
            {
                fail = true;
                judgeI.sprite = failSp;
                homeB.SetActive(true);
                restartB.SetActive(true);
                nextB.SetActive(false);
            }
        }
        else
        {
            homeB.SetActive(false);
            restartB.SetActive(false);
            nextB.SetActive(true);
        }
        
    }

    public void SelectingMask(int maskInt)
    {
        if (currentPatient == null)
            return;
        if (currentPatient.patientState == PatientState.Leaving || currentPatient.patientState == PatientState.WalkIn)
            return;
        Audio.instance.PlaySound("Button");
        maskSelected = true;
        selectedMask = (MaskType)maskInt;
        

  
    }
    public void GiveMask()
    {
        if (currentPatient == null)
            return;
        if (currentPatient.patientState == PatientState.Leaving || currentPatient.patientState == PatientState.WalkIn)
            return;
        if (!maskSelected)
            return;
        Audio.instance.PlaySound("Mask");
        idleCoolTime = 5;
        waitTimer = 0;
        questionBlock.SetActive(false);
        DocumentOff();
        dialogue.SkipTyping();
        dialogue.StopAllCoroutines();
        dialogue.canClose = true;
        dialogueBlock.SetActive(false);
        MaskType mask = selectedMask;
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
maskSelected = false;
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
        GateOff();
    }
    void DayFinish()
    {
        dayFinishNoti.transform.DOMoveY(0f, 0.5f).SetEase(Ease.InQuad);
        
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
    public void ToggleGate()
    {
        gateOpen = !gateOpen;
        if (gateOpen)
        {
            GateOn();
           
        }
        else
        {
            GateOff();
          
        }
    }
    public void GateOn()
    {
        Audio.instance.PlaySound("GOpen");
        gate1Ani  = gate1.transform.DOMoveY(8f, 0.5f).SetEase(Ease.OutQuad);
        gate2Ani  = gate2.transform.DOMoveY(8f, 0.5f).SetEase(Ease.OutQuad);
        gateOpen = true;
        gateLight1.sprite = green;
        gateLight2.sprite = green;
    }
    public void GateOff()
    {
        Audio.instance.PlaySound("GClose");
        gate1Ani  = gate1.transform.DOMoveY(1.94f, 0.5f).SetEase(Ease.InQuad);
        gate2Ani  = gate2.transform.DOMoveY(1.94f, 0.5f).SetEase(Ease.InQuad);
        gateOpen = false;
        gateLight1.sprite = red;
        gateLight2.sprite = red;
    }

    public void ToggleDocument()
    {
        if(currentPatient == null)
            return;
        if(currentPatient.patientState == PatientState.Leaving|| currentPatient.patientState == PatientState.WalkIn)
            return;
        Audio.instance.PlaySound("Doc");
        paperOn = !paperOn;
        if (paperOn)
        {
            DocumentOn();
        }
        else
        {
            DocumentOff();
        }
    }
    public void DocumentOn()
    {
        
        documentGO.transform.DOMoveX(6f, 0.5f).SetEase(Ease.OutQuad);
        paperOn = true;
       // documentBGO.transform.DOMoveX(-11f, 0.5f).SetEase(Ease.OutQuad);
    }
    public void DocumentOff()
    {
        documentGO.transform.DOMoveX(14f, 0.5f).SetEase(Ease.InQuad);
        paperOn = false;
       // documentBGO.transform.DOMoveX(-9.2f, 0.5f).SetEase(Ease.InQuad);
    }

    public void GoMainMenu()
    {
        Audio.instance.PlaySound("Button");
        StopAllCoroutines();
        SceneTransitionData.entryType = SceneEntryType.FromGame;
       
        gameManager.ResetGame();
        gate1Ani.Kill();
        gate2Ani.Kill();
        SceneFader.instance.FadeToScene("MainMenu");
    }

    public void NextDay()
    {
        Audio.instance.PlaySound("Button");
        StopAllCoroutines();
        SceneTransitionData.entryType = SceneEntryType.FromGame;
      
        gameManager.NextDay();
        gate1Ani.Kill();
        gate2Ani.Kill();
        SceneFader.instance.FadeToScene("InGame");
    }

    public void RestartGame()
    {
        Audio.instance.PlaySound("Button");
        StopAllCoroutines();
        SceneTransitionData.entryType = SceneEntryType.FromGame;
      
        gate1Ani.Kill();
        gate2Ani.Kill();
        gameManager.ResetGame();
        SceneFader.instance.FadeToScene("InGame");
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
              
                DocumentOff();
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