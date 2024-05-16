using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collisionsProjectile)
    {
        //Collision de la balle avec l'ennemi
        if (collisionsProjectile.gameObject.name == "Ennemi")
        {
            //On detruie l'ennemi
            Destroy(collisionsProjectile.gameObject, 0.4f);
        }
        //On détruie le projectile
        Destroy(gameObject, 3f);
    }
}
