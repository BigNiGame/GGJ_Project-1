using UnityEngine;

public class PatientSprite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float breatheTimer = 0f;
    public float breatheScaleY = 1.02f; // Maximum scale in Y
    public float breatheScaleX = 1.01f; // Optional horizontal stretch
    public float breatheSpeed = 3f; // Time for one inhale/exhale cycle
    private Vector3 originalScale;
    public Vector2 startPosition, targetPosition;
    float moveSpeed = 10f;
    float bounceHeight = 0.25f;
    float bounceSpeed = 10f;
    float distance, moveProgress;
    public Patient patient;
    Vector2 shakeOffset;
    float shakeTimer;
    float shakeDuration;
    float shakeStrength;
    Vector2 basePosition,bounceOffset;
    void Start()
    {
        originalScale = transform.localScale;
        startPosition = transform.position;
        distance = Vector3.Distance(startPosition, targetPosition);
    }
    public void StartShake(float duration, float strength)
    {
        shakeDuration = duration;
        shakeStrength = strength;
        shakeTimer = duration;
    }
    // Update is called once per frame
    void Update()
    {
        AnimateBreathing();
        if (patient.patientState == PatientState.WalkIn || patient.patientState == PatientState.Leaving)
        {
            MoveAndBounce();
        }

     
        if (patient.agressive)
        {
            StartShake(10f, 0.5f);
            UpdateShake();
            if(patient.patientState == PatientState.Idle)
                transform.position =  startPosition + shakeOffset;
            moveSpeed = 50;
        }
     
    }

    void AnimateBreathing()
    {
        // Increment timer
        breatheTimer += Time.deltaTime;

        float sine = Mathf.Sin((breatheTimer / breatheSpeed) * Mathf.PI * 2) * 0.5f + 0.5f; // 0 to 1

        transform.localScale = new Vector3(
            Mathf.Lerp(originalScale.x, originalScale.x * breatheScaleX, sine),
            Mathf.Lerp(originalScale.y, originalScale.y * breatheScaleY, sine),
            originalScale.z
        );
        
      
    }

    private void MoveAndBounce()
    {
        moveProgress += (moveSpeed * Time.deltaTime) / distance;
        moveProgress = Mathf.Clamp01(moveProgress);

      

        if (!patient.agressive)
        {
            float bounce = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
            bounceOffset = new Vector3(0, bounce, 0);
        }
        else
        {
            bounceOffset = Vector3.zero;
            
        }
        transform.position = Vector2.Lerp(startPosition, targetPosition, moveProgress) + shakeOffset+ bounceOffset;
        if (moveProgress >= 1f)
        {
            transform.position = targetPosition;
            if (patient.patientState == PatientState.Leaving && patient.currentZone == 1)
            {
                transform.position = new Vector3(45, 0, 0);
            }
            patient.patientState = PatientState.Idle;
            moveProgress = 0;
            startPosition = transform.position;
        }
    }
    void UpdateShake()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            float x = Random.Range(-1f, 1f) * shakeStrength;
            float y = Random.Range(-1f, 1f) * shakeStrength;
            shakeOffset = new Vector3(x, y, 0);
        }
        else
        {
            shakeOffset = Vector3.zero;
        }
    }
}