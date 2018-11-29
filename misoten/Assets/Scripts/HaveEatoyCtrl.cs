﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Constants;

public class HaveEatoyCtrl : MonoBehaviour {

    private Player          _player;
    private PlayerMove      _playerMove;
    private PlayerAnimCtrl  _playerAnim;

    private Sprite[]        _eatoyMaps;
    private GameObject      _haveEatoy;
    private SpriteRenderer  _eatoyRenderer;

    public enum EATOY_NUM
    {
        E1_PASTA = 0,
        E2_PASTA,
        E3_PASTA,
        E4_PASTA,
        E5_USAGI,
        E6_USAGI,
        E7_USAGI,
        E8_USAGI,
        E9_LOCKET,
        E10_LOCKET,
        E11_LOCKET,
        E12_LOCKET
    }
    [SerializeField, Range(0, 5)] private int _eatoyNum = 0;

    private void Awake()
    {
        _eatoyMaps = Resources.LoadAll<Sprite>("Textures/Eatoy/eatoymap");
    }

    void Start()
    {
        _player = this.GetComponent<Player>();
        _playerMove = this.GetComponent<PlayerMove>();
        _playerAnim = this.GetComponent<PlayerAnimCtrl>();

        _haveEatoy = this.transform.GetChild(1).gameObject;
        if (!(_haveEatoy.name == "haveEatoy"))
        {
            Debug.Log("Error . broken hierarchy : Not haveEatoy");
        }
        _eatoyRenderer = _haveEatoy.GetComponent<SpriteRenderer>();
        _eatoyRenderer.sprite = _eatoyMaps[_eatoyNum * 2];

        foreach (Transform effect in _haveEatoy.transform) effect.gameObject.GetComponent<ParticleSystem>().Stop();
        
    }

    void Update() {
        HaveEatoy();
    }

    void HaveEatoy()
    {
        if (!(_playerAnim.IsServing()))
        {
            _eatoyRenderer.enabled = false;
            return;
        }

        _eatoyRenderer.enabled = true;

        if (_player.GetPlayerStatus() == Player.PlayerStatus.CateringIceEatoy)
        {
            foreach (Transform effect in _haveEatoy.transform)
            {
                if(!(effect.gameObject.GetComponent<ParticleSystem>().isPlaying))
                    effect.gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            foreach (Transform effect in _haveEatoy.transform)
            {
                if (effect.gameObject.GetComponent<ParticleSystem>().isPlaying)
                    effect.gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }

        SetEatoy(_eatoyNum * 2);
        OnTrun();

    }

    void OnTrun()
    {
        if (_playerMove.IsRightDirection())
        {
            _haveEatoy.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            _haveEatoy.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void UpHand() => _haveEatoy.transform.localPosition = new Vector3(0, 0.01f, -0.1f);

    void DownHand() => _haveEatoy.transform.localPosition = new Vector3(0, 0, -0.1f);

    void SetEatoy(int num) => _eatoyRenderer.sprite = _eatoyMaps[num];

    public void SetEatoyNum(int num) => _eatoyNum = num - 1;
    
}
