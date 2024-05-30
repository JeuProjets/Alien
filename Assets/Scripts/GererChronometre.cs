using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GererChronometre : MonoBehaviour
{
    public GameObject chronometre;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Si le chronometre est a 0, le joueur a perdu
        if (chronometre.GetComponent<Chronometre>().tempsRestant <= 0)
        {
            //On met la scene de fin perdu
            SceneManager.LoadScene("FinPerdu");
        }
    }
}
