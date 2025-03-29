using System;
using TMPro;
using UnityEngine;

public class BankUIManager : MonoBehaviour
{
    [Header("UI Elements")] 
    [SerializeField] private TextMeshProUGUI balanceText;

    [SerializeField] private TextMeshProUGUI transactionsText;

    private string transactions = "AUTH PURCHASE - REGGIE	       +10000.00       2030.00\n"
                                  + "AUTH PURCHASE - CASINO	        -9000.00        -8640.00\n"
                                  + "AUTH PURCHASE - RENT	        -650.00           360.00\n"
                                  + "AUTH PURCHASE - FOOD	        -20.00             1010.00\n"
                                  + "AUTH PURCHASE - CASINO	        -1000.00          1030.00";

    public void GainMoney()
    {
        balanceText.text = "10000.00";
        balanceText.color = Color.black;

        transactionsText.text = transactions;
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //     GainMoney();
        // }
    }
}
