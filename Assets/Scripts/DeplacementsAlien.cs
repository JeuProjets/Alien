using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeplacementsAlien : MonoBehaviour
{
    //VARIABLES
    float vitesseX;
    float vitesseY;

    public float vitesseXMax;
    public float vitesseSaut;

    //public float reculBlessee;

    //Sons
    public AudioClip sonDiamants;

    //si Alien touche l'ennemi une fois
    public bool alienBlesser = false;

    //si Alien touche l'ennemi deux fois
    public bool alienMort = false;

    bool peutAttaquer;
    //si Alien a une collision avec un objet
    public bool alienCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //Déplacement vers la gauche
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            vitesseX = -vitesseXMax;
            GetComponent<SpriteRenderer>().flipX = true;

        }
        //Déplacement vers la droite
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            vitesseX = vitesseXMax;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            vitesseX = GetComponent<Rigidbody2D>().velocity.x;
        }

        Physics2D.OverlapCircle(transform.position, 0.2f);

        // Saut
        if (Input.GetKeyDown("up") && Physics2D.OverlapCircle(transform.position, 0.2f) && alienCollision == true)
        {
            vitesseY = vitesseSaut;
            alienCollision = false;
            GetComponent<Animator>().SetBool("saut", true);

        }
        else
        {   //vitesse actuelle verticale
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;  

        }

        //Applique les vitesses en X et Y
        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);

        //****Gestion des animaitons de course, de repos et de saut****
        if (vitesseX > 0.1f || vitesseX < -0.1f)
        {
            GetComponent<Animator>().SetBool("marche", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("marche", false);

        }
        if (vitesseX < 0.9f)
        {
            GetComponent<Animator>().SetBool("repos", true);
        }

        if (alienCollision == true)
        {
            GetComponent<Animator>().SetBool("saut", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collisionsAlien)
    {

        alienCollision = true;

        if (Physics2D.OverlapCircle(transform.position, 0.2f))
        {
            //On empêche l'option de sauter
            GetComponent<Animator>().SetBool("saut", false);
        }

        //Collision Gems
        if (collisionsAlien.gameObject.name == "bouleBleu")
        { 
            //On detruie le gem
            Destroy(collisionsAlien.gameObject);
            //On joue le son
            GetComponent<AudioSource>().PlayOneShot(sonDiamants);

            peutAttaquer = true;
            if (Input.GetKeyDown(KeyCode.Space) && peutAttaquer)
            {
                GetComponent<Animator>().SetBool("attaque", true);
            }
            
        }
        //Collision Ennemi
        if (collisionsAlien.gameObject.name == "Ennemi")
        {
            //Si l'alien est pas blessé
            if (!alienBlesser)
            {
                //Animation blessé
                GetComponent<Animator>().SetBool("mal", true);

                //Effet de saut recul
                GetComponent<Rigidbody2D>().velocity = new Vector2(10,4);

                //Appel de la fonction qui le remet a repos
                Invoke("AlienBlessee", 1f);

            }
            else
            {
                //Animation mort
                GetComponent<Animator>().SetBool("mort", true);
                //Variable mort est à vrai
                alienMort = true;
                //Appel de la fonction qui relance le jeu après un délai
                Invoke("RelancerJeu", 2f);
            }

        }
    }
    //fonction qui relance le jeu après un délai
    private void RelancerJeu()
    {
        SceneManager.LoadScene("Alien1");
    }
    // fonction de l'alien blessé
    private void AlienBlessee()
    {
        alienBlesser = true;
        GetComponent<Animator>().SetBool("mal", false);
        GetComponent<Animator>().SetBool("repos", true);
    }
}
