using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class People : MonoBehaviour
{

    [SerializeField] Transform _homePosition;
    [SerializeField] float _duration = .25f;

    public bool isHome = false;

    private void Awake()
    {
    }

    private void Start()
    {
    }


    private void OnMouseOver()
    {
        Scene2Manager.MouseOverPeople = this;
    }

    public void GoBackHome()
    {
        if (!isHome)
        {
            transform.DOMove(_homePosition.position, _duration);
            isHome = true;
        }
    }

}
