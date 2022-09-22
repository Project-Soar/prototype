using System.Numerics;
using Cysharp.Threading.Tasks;
using MoralisUnity;
using Nethereum.Hex.HexTypes;
using Pixelplacement;
using TMPro;
using UnityEngine;

    // Class which takes care to transfer some of the already minted supply of an ERC-20 token to the player address
    public class PickingUpRune : State
    {
        [Header("Components")]
        [SerializeField] private Player player;
        
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI statusText;

        [Header("Rune Contract Data")]
        public static string RuneContractAddress = "";
        public static string RuneContractAbi = "";

        #region UNITY_LIFECYCLE

        private void OnEnable()
        {


        }

        private void OnDisable()
        {

        }

        #endregion
        

        #region PRIVATE_METHODS

        public async void PickUpRune(int amount)
        {
            statusText.text = "Please confirm transaction in your wallet";
        
            var result = await GetExperience(amount);

            if (result is null)
            {
                statusText.text = "Transaction failed";
                return;
            }
        
            statusText.text = "Transaction completed!";

        }
        
        private async UniTask<string> GetExperience(int amount)
        {
            BigInteger amountValue = new BigInteger(amount);
            
            object[] parameters = {
                amountValue.ToString("x")
            };

            // Set gas estimate
            HexBigInteger value = new HexBigInteger(0);
            HexBigInteger gas = new HexBigInteger(0);
            HexBigInteger gasPrice = new HexBigInteger(0);

            string resp = await Moralis.ExecuteContractFunction(RuneContractAddress, RuneContractAbi, "getToken", parameters, value, gas, gasPrice);

            return resp;
        }

        #endregion
        
        
    
    }   
