using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        _isIdle = false;
        instance = this;
        /* if (GameManager.instance != null)
         {
             Destroy(gameObject);
             Destroy(player.gameObject);
             Destroy(floatingTextManager.gameObject);
             Destroy(hud);
             Destroy(menu);
             return;
         }

         // Code to Remove all saved Data
         //PlayerPrefs.DeleteAll();
         //weaponPrices = new List<int> { 30, 60, 100, 140, 190, 220 };

         SceneManager.sceneLoaded += LoadState;
         SceneManager.sceneLoaded += OnSceneLoaded;*/
    }

    // Resources


    //References

    public bool IdleChanged = false;

    private bool _isIdle;

    public bool IsIdle {  
        get 
        { 
            return _isIdle; 
        } 
        set 
        {
            _isIdle = value;
        } 
    }

    public int _gold;
    private int _experience;

    public int Experience { 
        get
        {
            return _experience;
        } 
        set 
        { 
            _experience = value;
        }
    }

    public void GrantXp(int exp)
    {
        Experience += exp;

    }

    // Floating text

    // Upgrade weapon


    // Experience System


    //Save Game
    /*
     * Int prefferedSkin
     * Int gold
     * Int experience
     * Int weaponLevel
     */
    public void SaveState()
    {
       /* string s = "";

        s += player.spriteId + "|";
        s += gold.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("Save State");*/
    }

   

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        /*SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //player.SwapSprite(int.Parse(data[0]));
        gold = int.Parse(data[1]);

        // Experience
        experience = int.Parse(data[2]);
        player.SetLevel(GetCurrentLevel());


        //Weapon
        weapon.SetWeaponLevel(int.Parse(data[3]));*/
    }
}
