using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SignPostVictory : MonoBehaviour
{
    //Declares an Animator variable;
    private Animator signPostAnimator;

    //Bool used to only let the onTrigger event happen once.
    private bool alreadyActivated = false;

    void Start()
    {
        //Assign the animator component to the variable.
        signPostAnimator = GetComponent<Animator>();
        //Initializes the signpost to have Dr. Robotnik's picture.
        signPostAnimator.Play("Signpost_Robotnik");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks for a collision with Sonic and if the if statement has already been run through once.
        if (collision.gameObject.tag == "Player" && !alreadyActivated)
        {
            //Sets to true so this if statement may only run once.
            alreadyActivated = true;

            //Twirls the signpost.
            signPostAnimator.Play("Signpost_Twirl");
            //Sets the signpost to the picture of Sonic and activates the end of level UI.
            Invoke("SonicSignpost", 3f);
            //Loads next scene.
            Invoke("LevelEnd", 5f);
        }
    }

    private void SonicSignpost()
    {
        signPostAnimator.Play("Signpost_Sonic");
    }

    private void LevelEnd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
