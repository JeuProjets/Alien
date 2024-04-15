using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeplacementsAlien : MonoBehaviour
{
    float vitesseX;
    float vitesseY;

    public float vitesseXMax;
    public float vitesseSaut;

    public bool alienCollision;
    //bool pouvoirSaut = false;

    public AudioClip sonDiamants;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //D�placement vers la gauche
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            vitesseX = -vitesseXMax;
            GetComponent<SpriteRenderer>().flipX = true;

        }
        //D�placement vers la droite
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
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && Physics2D.OverlapCircle(transform.position, 0.2f))
        {
            vitesseY = vitesseSaut;
            GetComponent<Animator>().SetBool("saut", true);
            alienCollision = false;
        }
        else
        {
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;  //vitesse actuelle verticale
            GetComponent<Animator>().SetBool("saut", false);
        }

        //Applique les vitesses en X et Y
        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);

        //****Gestion des animaitons de course, de repos et de saut****
        if (vitesseX > 0.2f || vitesseX < -0.5f)
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
        //Variable de collision est � vrai
        alienCollision = true;


        if (Physics2D.OverlapCircle(transform.position, 0.2f))
        {
            //On emp�che l'option de sauter
            GetComponent<Animator>().SetBool("saut", false);
        }

        //Gems
        if (collisionsAlien.gameObject.name == "bouleBleu")
        { 
            //pouvoirSaut = true;
            Destroy(collisionsAlien.gameObject);
            GetComponent<AudioSource>().PlayOneShot(sonDiamants);
        }
    }
}
