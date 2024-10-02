using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleDoor : MonoBehaviour
{
    enum DoorState
    {
        Close = 0,
        Open
    }
    DoorState state = DoorState.Close;
    DoorState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                switch (state)
                {
                    case DoorState.Close:
                        animator.SetInteger(Hash_Door, 0);
                        audioSources[1].Play();
                        break;

                    case DoorState.Open:
                        animator.SetInteger(Hash_Door, 1);
                        audioSources[0].Play();
                        break;
                }
            }
        }
    }

    Animator animator;
    AudioSource[] audioSources;

    readonly int Hash_Door = Animator.StringToHash("isOpen");

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
        audioSources = GetComponents<AudioSource>();
    }

    public void Interact_Door()
    {
        if(State == DoorState.Close)
        {
            State = DoorState.Open;
        }
        else
        {
            State = DoorState.Close;
        }
    }
}
