using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeplacementsAlien : MonoBehaviour
{
    //VARIABLES
    float vitesseX;
    float vitesseY;

    public float vitesseXMax;
    public float vitesseSaut;

    public GameObject texteAttaque;
    public GameObject projectileOriginal;
    public GameObject projectileClone;

    public GameObject penteNeige;


    //public float reculBlessee;

    //Sons
    public AudioClip sonDiamants;

    //si Alien touche l'ennemi une fois
    public bool alienBlesser = false;

    //si Alien touche l'ennemi deux fois
    public bool alienMort = false;

    public bool peutAttaquer;
    //si Alien a une collision avec un objet
    public bool alienCollision = false;

  


    // Start is called before the first frame update
    void Start()
    {
        texteAttaque.SetActive(false);
        
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
            //Il peut plus attaquer
            peutAttaquer = false;
        }
        else
        {   //vitesse actuelle verticale
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;  

        }

        //Applique les vitesses en X et Y
        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);

        //Attaque
        if (Input.GetKeyDown(KeyCode.Space) && peutAttaquer)
        {
            GetComponent<Animator>().SetBool("attaque", true);
            Invoke("TireBalle", 0.8f);
            Invoke("ArretAttaque", 1f);
            texteAttaque.SetActive(false);  
        }

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
            peutAttaquer = true;
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
        if (collisionsAlien.gameObject.tag == "boule")
        {
            if (collisionsAlien.gameObject.name == "bouleRouge")
            {
                //Couleur de l'alien en rouge
                GetComponent<SpriteRenderer>().color = new Color(0.990566f, 0.6273868f, 0.6120951f, 1f);

                //Mettre la variable qui lui permet d'attaquer à true
                peutAttaquer = true;

                //Activer le texte dans l'inspector
                texteAttaque.SetActive(true);
            }
            
            if (collisionsAlien.gameObject.name == "bouleBleu")
            {
                GetComponent<SpriteRenderer>().color = new Color(0.6839622f, 0.7172294f, 1f, 1f);
                // Augmenter la vitesse
                vitesseXMax *= 1.3f;
                vitesseSaut *= 1.4f;

                // Il peut monter la pente
                SurfaceEffector2D effector = penteNeige.GetComponent<SurfaceEffector2D>();
                effector.speed *= -1f;

                //Il peut plus attaquer
                peutAttaquer = false;


            }
                
            //On detruie le gem
            Destroy(collisionsAlien.gameObject);
            //On joue le son
            GetComponent<AudioSource>().PlayOneShot(sonDiamants);

 
        }
        //Plateforme: lorsque le personnage est sur la plateforme, il est enfant de celle-ci
        //Tutoriel regardé pour l'information: https://www.youtube.com/watch?v=DQYj8Wgw3O0
        if (collisionsAlien.gameObject.name == "plateformeBouge")
        {
            this.transform.parent = collisionsAlien.transform;
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
                Invoke("finPerdu", 2f);
            }

        }

        //Si il se rend au vaisseau, on met la scene de victoire
        if (collisionsAlien.gameObject.name == "Vaisseau")
        {
            Invoke("finGagne", 2f);
            //On le place en dessous du vaisseau
            GetComponent<SpriteRenderer>().sortingOrder = 4;

        }



    }

    //Plateforme: lorsque le personnage sort de la plateforme, il n'est plus enfant de celle-ci
    void OnCollisionExit2D(Collision2D collisionsAlien)
    {
        if (collisionsAlien.gameObject.name == "plateformeBouge")
        {
            this.transform.parent = null;
        }
    }
    //Gestion Scenes
    private void finGagne()
    {
        SceneManager.LoadScene("finGagne");
    }
    private void finPerdu()
    {
        SceneManager.LoadScene("finPerdu");
    }
    // fonction de l'alien blessé
    private void AlienBlessee()
    {
        alienBlesser = true;
        GetComponent<Animator>().SetBool("mal", false);
        GetComponent<Animator>().SetBool("repos", true);
    }
    private void TireBalle()
    {

        GetComponent<Animator>().SetBool("projectile", true);


        //On instancie une balle
        projectileClone = Instantiate(projectileOriginal);

        //Rend active
        projectileClone.SetActive(true);


        if (GetComponent<SpriteRenderer>().flipX)
        {
            projectileClone.transform.position = transform.position + new Vector3(-1f, 0);
            projectileClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 3);
        }
        else
        {
            projectileClone.transform.position = transform.position + new Vector3(1f, 0);
            projectileClone.GetComponent<Rigidbody2D>().velocity = new Vector2(10, 0);
        }
    }
    //Gestion Attaque
    private void ArretAttaque()
    {
        GetComponent<Animator>().SetBool("attaque", false);
    }
}
