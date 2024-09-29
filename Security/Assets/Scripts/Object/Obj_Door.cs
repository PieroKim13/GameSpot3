using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Door : MonoBehaviour
{
    enum DoorState
    {
        Idle,
        Open
    }

    DoorState state = DoorState.Idle;
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
                    case DoorState.Idle:
                        animator.SetInteger(Hash_Door, 0);

                        break;
                    case DoorState.Open:
                        animator.SetInteger(Hash_Door, 1);

                        break;
                }
            }
        }
    }

    Transform door;
    Animator animator;

    readonly int Hash_Door = Animator.StringToHash("isOpen");

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    public void Action_Door()
    {
        if(State == DoorState.Idle)
        {
            State = DoorState.Open;
        }
        else
        {
            State = DoorState.Idle;
        }
    }
}
