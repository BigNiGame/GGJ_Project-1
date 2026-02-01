using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentDay = 1;
    public int currentPatientCuredThisQuota;
    public int quotaDay = 2;
    public int quotaPatient = 4;
    
    public void NextDay()
    { 
        if (currentDay == quotaDay)
        {
            quotaDay +=2;
            currentPatientCuredThisQuota = 0;
        }
        currentDay++;
       
    }

    public void ResetGame()
    {
        currentDay = 1;
        quotaDay = 2;
        currentPatientCuredThisQuota = 0;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
