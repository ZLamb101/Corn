using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelingManager : MonoBehaviour
{
    public static LevelingManager Instance;

    public bool isUpdatingXp;

    public int level;
    public float currentXp;
    public float requiredXp;
    private float lerpTimer;

    [Header("Multipliers")]
    [Range(1f,300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;


    [Header("UI")]
    public Slider XpBar;
    public TMP_Text ExperienceBarText;

    private void Awake()
    {
        requiredXp = CalculateRequiredXp();
        XpBar.value = currentXp / requiredXp;
        ExperienceBarText.text = "EXP " + currentXp.ToString() + " / " + requiredXp.ToString();
        Debug.Log("Xpbar Value : " + XpBar.value + " , Curent XP : " + currentXp + " , Required Xp : " + requiredXp);

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdatingXp)
        UpdateXpUI();
        if (Input.GetKeyDown(KeyCode.Equals)) {
            GainExperienceFlatRate(20);
           
        }
    }

    public void UpdateXpUI()
    {
        float xpFraction = currentXp / requiredXp;
        float FXP = XpBar.value;
        if(FXP < xpFraction)
        {
           // Debug.Log("AddingXP");
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / 4;
            XpBar.value = Mathf.Lerp(FXP, xpFraction, percentComplete);
            ExperienceBarText.text = "EXP "+ currentXp.ToString() + " / " + requiredXp.ToString();
            if (lerpTimer >= 0.6f)
            {
                Debug.Log("Xpbar Value : " + XpBar.value + " , Curent XP : " + currentXp + " , Required Xp : " + requiredXp);
                if (currentXp >= requiredXp)
                {
                    LevelUp();
                }
                else
                {
                    isUpdatingXp = false;
                }
                 
            }
        }
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        currentXp += xpGained;
        lerpTimer = 0f;
        isUpdatingXp = true;
        UpdateXpUI();
        Debug.Log("Xpbar Value : " + XpBar.value + " , Curent XP : " + currentXp + " , Required Xp : " + requiredXp);
    }

    public void LevelUp()
    {
        
        level++;
        XpBar.value = 0;
        currentXp = Mathf.RoundToInt(currentXp - requiredXp);
        if(currentXp != 0)
        {
            Debug.Log("LEVEL UP : Xpbar Value : " + XpBar.value + " , Curent XP : " + currentXp + " , Required Xp : " + requiredXp);
            isUpdatingXp = true;
            lerpTimer = 0f;
            requiredXp = CalculateRequiredXp();
            UpdateXpUI();
        }
        
        // add health

    }

    private int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;
        for(int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }
}
