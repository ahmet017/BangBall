using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core.Environments;
using System.Threading.Tasks;
public class IAPManager : MonoBehaviour, IStoreListener
{
    IStoreController controller;
    private async System.Threading.Tasks.Task InitializeServicesAsync()
    {
        // Initialize Unity Gaming Services
        await UnityServices.InitializeAsync();

        // Now initialize IAP
        FindObjectOfType<IAPManager>().IAPStart();
    }
    private async void Start()
    {
        await InitializeServicesAsync();
        Debug.Log("IAPManager Start");
        IAPStart();
    }

    private async Task InitializeUnityServicesAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Gaming Services initialized successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Gaming Services: {e.Message}");
        }
    }

    private void IAPStart()
    {
        Debug.Log("IAPStart called");
        var module = StandardPurchasingModule.Instance();
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
        builder.AddProduct(ServicesManager.instance.noAdsID, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Initialization Failed: {error} - {message}");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized called");
        this.controller = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Initialization Failed: {error}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase Failed: {failureReason}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (string.Equals(purchaseEvent.purchasedProduct.definition.id, ServicesManager.instance.noAdsID, StringComparison.Ordinal))
        {
            Debug.Log("Purchase Successful");
            return PurchaseProcessingResult.Complete;
        }
        else
        {
            Debug.LogWarning("Purchase Pending");
            return PurchaseProcessingResult.Pending;
        }
    }

    public void IAPButton(string id)
    {
        if (controller == null)
        {
            Debug.LogError("IAPManager controller is not initialized. Please try again later.");
            return;
        }

        Product proc = controller.products.WithID(id);
        if (proc != null && proc.availableToPurchase)
        {
            Debug.Log("Initiating Purchase");
            controller.InitiatePurchase(proc);
        }
        else
        {
            Debug.LogError("Product not available for purchase or null.");
        }
    }


}
