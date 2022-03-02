using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_anim : MonoBehaviour {

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        while (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("isWalking", true);
        }
        anim.SetBool("isWalking", false);
    }

    //public enum CharacterState
    //{
    //    idle,
    //    walking
    //}

    //Animator player_animation;
    //public CharacterState _state;

    //private void Start()
    //{
    //    player_animation = gameObject.GetComponentInChildren<Animator>();
    //}

    //void Update()
    //{
    //    CheckKey();
    //}

    //void CheckKey()
    //{
    //    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
    //    {
    //        _state = CharacterState.walking;
    //    }
    //    else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
    //    {
    //        _state = CharacterState.idle;
    //    }
    //    PlayAnimation();
    //}

    //void PlayAnimation()
    //{
    //    switch (_state)
    //    {
    //        case CharacterState.idle:
    //            player_animation.Play("Idle");
    //            break;

    //        case CharacterState.walking:
    //            player_animation.Play("Walking");
    //            break;
    //    }
    //}
}
