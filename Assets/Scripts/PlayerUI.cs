using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI interactText;
    public Image healtBarFill;
    public Image xpBarFill;

    private Player player;

    void Awake(){
        //get the player

        player = FindObjectOfType<Player>();
    }

    public void UpdateLevelText(){
        levelText.text = "Lvl\n" + player.curLevel;
    }

    public void UpdateHealtBar(){
        healtBarFill.fillAmount = (float)player.curHp / (float)player.maxHp;
    }

    public void UpdateXpBar(){
        xpBarFill.fillAmount = (float)player.curXp / (float)player.xpToNextLevel;
    }

    public void SetInteractText(Vector3 pos, string text){
        interactText.gameObject.SetActive(true);
        interactText.text = text;

        interactText.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.up);
    }
    
    public void DisableInteractText(){
        if(interactText.gameObject.activeInHierarchy)
            interactText.gameObject.SetActive(false);
    }

    public void UpdateInventoryText(){
        inventoryText.text = "";
        foreach (string item in player.inventory)
        {
        inventoryText.text += item + "\n";
        }
    }
}
