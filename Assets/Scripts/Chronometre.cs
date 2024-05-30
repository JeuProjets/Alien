using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chronometre : MonoBehaviour
{
    public float tempsRestant;
    public bool chronoActive;

    public TMP_Text textChrono;
    // Start is called before the first frame update

    //TUTORIEL REGARDE: https://www.google.com/search?q=unity+countdown+with+textmeshpro&oq=unity+countdown+with+textmeshpro&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIJCAEQIRgKGKABMgkIAhAhGAoYoAHSAQg3NTM3ajBqMagCALACAA&sourceid=chrome&ie=UTF-8#fpstate=ive&vld=cid:e71fb93b,vid:bGePRqD-SNE,st:0
    void Start()
    {
        chronoActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(chronoActive)
        {
            if (tempsRestant > 0)
            {
                tempsRestant -= Time.deltaTime;
                mettreAJourChrono(tempsRestant);
            }
            else
            {
                tempsRestant = 0;
                chronoActive = false;
            }
        }
    }

    void mettreAJourChrono(float tempsReel)
    {
        tempsReel += 1;

        float minutes = Mathf.FloorToInt(tempsReel / 60);
        float secondes = Mathf.FloorToInt(tempsReel % 60);

        textChrono.text = string.Format("{0:00} : {1:00}", minutes, secondes);
    }
}
