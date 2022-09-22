using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoralisUnity.Platform.Objects;
using Pixelplacement;
using System;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Web3Api.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using TMPro;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

namespace ERC20Mint{
    public class GameManager : StateMachine
    {

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI statusText;

        [Header("ERC@) Contract Data")]
        public static string ERC20ContractAddress = "0x6Dd857B580bcb7324f3Cc824EEec5D4d70C5403f";
        public static string ERC20ContractAbi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"getToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"removeFromBalance\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"origin\",\"type\":\"address\"}],\"name\":\"getBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"balance\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    
        [SerializeField] private ERC20 ercmint;
        [SerializeField] private TextMeshProUGUI statusLabel;
 
        #region UNITY_LIFECYCLE
        private void OnEnable()
        {
            ercmint.MintPressed += GetToken ;
        }

        private void OnDisable()
        {
            ercmint.MintPressed -= GetToken;
        
        }
        #endregion

        
        #region PUBLIC_METHODS

        public void ToMainState()
        {
            ChangeState("State");
        }


        #endregion
        
        public void token(int z)
        {
            GetToken(z);
        }
        #region PRIVATE_METHODS

         private async void GetToken(int amount)
        {
            statusText.text = "Please confirm transaction in your wallet";
        
            var result = await TransferToken(amount);

            if (result is null)
            {
                statusText.text = "Transaction failed";
                ercmint.EnableMintButton();
                return;
            }
        
            statusText.text = "Transaction completed!";
            ercmint.EnableMintButton();
        }
        
        private async UniTask<string> TransferToken(int amount)
        {
            BigInteger amountValue = new BigInteger(amount);
            
            object[] parameters = {
                amountValue.ToString("x")
            };

            // Set gas estimate
            HexBigInteger value = new HexBigInteger(0);
            HexBigInteger gas = new HexBigInteger(0);
            HexBigInteger gasPrice = new HexBigInteger(0);

            string resp = await Moralis.ExecuteContractFunction(ERC20ContractAddress, ERC20ContractAbi, "gettoken", parameters, value, gas, gasPrice);

            return resp;
        }

        #endregion
        
        
        // #region INPUT_SYSTEM_HANDLERS

        // private async void CancelTransaction(InputAction.CallbackContext obj)
        // {
        //     // Check out what we're doing to "cancel" the transaction:
        //     // https://ethereum.stackexchange.com/questions/31298/is-it-possible-to-cancel-a-transaction
            
        //     BigInteger amountValue = new BigInteger(0);
            
        //     object[] parameters = {
        //         amountValue.ToString("x")
        //     };
            
        //     HexBigInteger value = new HexBigInteger(0);
        //     HexBigInteger gas = new HexBigInteger(0);
        //     HexBigInteger gasPrice = new HexBigInteger(1); //Higher than "GetItem" transaction
            
        //     string resp = await Moralis.ExecuteContractFunction(GameManager.RuneContractAddress, GameManager.RuneContractAbi, "getExperience", parameters, value, gas, gasPrice);
            
        //     LastActiveState();
        // }
    
    public void onExit()
    {
        
    } 

    }  

    
}
