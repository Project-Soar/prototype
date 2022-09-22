using System.Numerics;
using Cysharp.Threading.Tasks;
using MoralisUnity;
using Nethereum.Hex.HexTypes;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;

using System;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

namespace ERC20Mint
{
    public class ERC20 : State
    {  
        
        public Action<int> MintPressed;

        public int harvest = 10;

        private GameManager _gameManager;

        [SerializeField] private Button Mint;

        private void OnEnable()
        {
            EnableMintButton();
        }



        public void OnButtonPressed()
        {
            MintPressed?.Invoke(harvest);
            _gameManager.token(harvest);
            Mint.interactable = false;

        }


        public void EnableMintButton()
        {
            Mint.interactable = true;
        }

       
}
}