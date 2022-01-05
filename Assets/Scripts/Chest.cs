using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public int xpToGive;

    public void OpenChest(){
        FindObjectOfType<Player>().AddXp(xpToGive);
        Destroy(gameObject);
    }
}
