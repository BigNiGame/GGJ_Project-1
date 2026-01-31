using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Male,
    Female
}

public enum PatientState
{
    WalkIn,
    Idle,
    Talking,
    Leaving
}

public class Patient : MonoBehaviour
{
    public Sprite photo;
    public int age;
    public Gender gender;
    public string patientName;
    public string patientFirstName;
    public string patientLastName;
    public MaskType correctMask;
    public MaskType maskGet;
    public bool history, randomDia, has2Rando;
    public int questionType;
    [TextArea] public string historytxt, randomtxt1, randomtxt2, feeltxt, todotxt;
    public bool feelAsked, todoAsked, randomText1Talked, randomText2Talked;
    public PatientState patientState;
    public float timeTilTalk;
    public PatientSprite patientSprite;
    public GameObject sad, anger, confuse, anxiety;
    public bool maskGiven;
    public int currentZone = 1;
    public bool agressive;
    void Start()
    {
        //   correctMask = (MaskType)Random.Range(0, 5);

        correctMask = (MaskType)0;
        patientName = MainStall.instance.GetRandomName();
        ConvertName(patientName);
        gender = (Gender)Random.Range(0, 2);
        age = Random.Range(21, 61);
        timeTilTalk = GenerateWaitTime();
        int possible = Random.Range(1, 8);
        switch (possible)
        {
            case 1:
                // his
                history = true;
                break;
            case 2:
                // rando 
                randomDia = true;
                break;
            case 3:
                // quest
                PickQuestion();
                break;
            case 4:
                // his rando
                history = true;
                randomDia = true;
                break;
            case 5:
                // his quest
                history = true;
                PickQuestion();
                break;
            case 6:
                // rando quest
                randomDia = true;
                PickQuestion();
                break;
            case 7:
                // all 3
                history = true;
                randomDia = true;
                PickQuestion();
                break;
        }

        PickTextData();
        
    }

    public void PickTextData()
    {
        historytxt = PickHistory();
        historytxt = FormatGenderText(historytxt, gender);
        historytxt = FormatTextSpace(historytxt);
        PickQuestionAnswer();
        PickRandomDia();
        randomtxt1 = FormatTextSpace(randomtxt1);
        randomtxt2 = FormatTextSpace(randomtxt2);
        feeltxt = FormatTextSpace(feeltxt);
        todotxt = FormatTextSpace(todotxt);
    }

    public float GenerateWaitTime()
    {
        float waitTime = Random.Range(2f, 4f);
        return waitTime;
    }

    public string PickHistory()
    {
        if (history)
        {
            string folder = correctMask.ToString();
            string path = $"Dialogues/{folder}/{folder}History";
            TextAsset file = Resources.Load<TextAsset>(path);
            if (file == null)
            {
                Debug.LogError("History file not found: " + path);
                return "";
            }

            string[] lines = file.text.Split(
                new[] { '\n' },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return "";

            int randomIndex = Random.Range(0, lines.Length);
            return lines[randomIndex].Trim();
        }
        else
        {
            string folder = "Blank";
            string path = $"Dialogues/{folder}/{folder}History";
            TextAsset file = Resources.Load<TextAsset>(path);
            if (file == null)
            {
                Debug.LogError("History file not found: " + path);
                return "";
            }

            string[] lines = file.text.Split(
                new[] { '\n' },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return "";

            int randomIndex = Random.Range(0, lines.Length);
            return lines[randomIndex].Trim();
        }
    }

    public void Maskgiven()
    {
        patientState = PatientState.Leaving;
        maskGiven = true;
        patientSprite.targetPosition = new Vector3(-15, 0, 0);
    }

    public void PickQuestionAnswer()
    {
        switch (questionType)
        {
            case 1:
            {
                string folderA = correctMask.ToString();
                string pathA = $"Dialogues/{folderA}/{folderA}Feel";
                TextAsset fileA = Resources.Load<TextAsset>(pathA);
                if (fileA == null)
                {
                    Debug.LogError("History file not found: " + pathA);
                }

                string[] linesA = fileA.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexA = Random.Range(0, linesA.Length);
                feeltxt = linesA[randomIndexA].Trim();

                string folderB = "Blank";
                string pathB = $"Dialogues/{folderB}/{folderB}ToDo";
                TextAsset fileB = Resources.Load<TextAsset>(pathB);
                if (fileB == null)
                {
                    Debug.LogError("History file not found: " + pathB);
                }

                string[] linesB = fileB.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexB = Random.Range(0, linesB.Length);
                todotxt = linesB[randomIndexB].Trim();
                break;
            }
            case 2:
            {
                string folderA = "Blank";
                string pathA = $"Dialogues/{folderA}/{folderA}Feel";
                TextAsset fileA = Resources.Load<TextAsset>(pathA);
                if (fileA == null)
                {
                    Debug.LogError("History file not found: " + pathA);
                }

                string[] linesA = fileA.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexA = Random.Range(0, linesA.Length);
                feeltxt = linesA[randomIndexA].Trim();

                string folderB = correctMask.ToString();
                string pathB = $"Dialogues/{folderB}/{folderB}ToDo";
                TextAsset fileB = Resources.Load<TextAsset>(pathB);
                if (fileB == null)
                {
                    Debug.LogError("History file not found: " + pathB);
                }

                string[] linesB = fileB.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexB = Random.Range(0, linesB.Length);
                todotxt = linesB[randomIndexB].Trim();
                break;
            }
            case 0:
            {
                string folderA = "Blank";
                string pathA = $"Dialogues/{folderA}/{folderA}Feel";
                TextAsset fileA = Resources.Load<TextAsset>(pathA);
                if (fileA == null)
                {
                    Debug.LogError("History file not found: " + pathA);
                }

                string[] linesA = fileA.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexA = Random.Range(0, linesA.Length);
                feeltxt = linesA[randomIndexA].Trim();

                string folderB = "Blank";
                string pathB = $"Dialogues/{folderB}/{folderB}ToDo";
                TextAsset fileB = Resources.Load<TextAsset>(pathB);
                if (fileB == null)
                {
                    Debug.LogError("History file not found: " + pathB);
                }

                string[] linesB = fileB.text.Split(
                    new[] { '\n' },
                    System.StringSplitOptions.RemoveEmptyEntries);


                int randomIndexB = Random.Range(0, linesB.Length);
                todotxt = linesB[randomIndexB].Trim();
                break;
            }
        }
    }

    public void PickRandomDia()
    {
        string folder = randomDia ? correctMask.ToString() : "Blank";
        string path = $"Dialogues/{folder}/{folder}Rando";
        TextAsset file = Resources.Load<TextAsset>(path);
        if (file == null)
        {
            Debug.LogError("History file not found: " + path);
        }

        string[] lines = file.text.Split(
            new[] { '\n' },
            System.StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0)
            return;

        int firstIndex = Random.Range(0, lines.Length);
        randomtxt1 = lines[firstIndex].Trim();

        // SOMETIMES pick second line (50% chance)
        bool hasSecond = Random.value > 0.5f && lines.Length > 1;

        if (hasSecond)
        {
            int secondIndex;
            do
            {
                secondIndex = Random.Range(0, lines.Length);
            } while (secondIndex == firstIndex);

            has2Rando = true;
            randomtxt2 = lines[secondIndex].Trim();
        }
    }

    public void ConvertName(string name)
    {
        string[] parts = name.Split(' ');

        patientFirstName = parts[0];
        patientLastName = parts.Length > 1 ? parts[1] : "";
    }

    public string FormatGenderText(string text, Gender gender)
    {
        if (gender == Gender.Male)
        {
            text = text.Replace("[person]", "man");
            text = text.Replace("[Person]", "Man");

            text = text.Replace("[they]", "he");
            text = text.Replace("[They]", "He");

            text = text.Replace("[their]", "his");
            text = text.Replace("[Their]", "His");

            text = text.Replace("[them]", "him");
            text = text.Replace("[Them]", "Him");
        }
        else
        {
            text = text.Replace("[person]", "woman");
            text = text.Replace("[Person]", "Woman");

            text = text.Replace("[they]", "she");
            text = text.Replace("[They]", "She");

            text = text.Replace("[their]", "her");
            text = text.Replace("[Their]", "Her");

            text = text.Replace("[them]", "her");
            text = text.Replace("[Them]", "Her");
        }

        return text;
    }

    public string FormatTextSpace(string text)
    {
        text = text.Replace("|", "\n");
        return text;
    }

    public void PickQuestion()
    {
        if (Random.value > 0.5f)
        {
            questionType = 1; //feel
        }
        else
        {
            questionType = 2; //do to
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (maskGiven)
            switch (maskGet)
            {
                case MaskType.Sad:
                    sad.SetActive(true);
                    break;
                case MaskType.Anger:
                    anger.SetActive(true);
                    break;
                case MaskType.Confuse:
                    confuse.SetActive(true);
                    break;
                case MaskType.Anxiety:
                    anxiety.SetActive(true);
                    break;
            }
    }
}