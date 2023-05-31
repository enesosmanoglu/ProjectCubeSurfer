using HmsPlugin;

using HuaweiMobileServices.IAP;
using HuaweiMobileServices.Utils;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    // Please insert your products via custom editor. You can find it in Huawei > Kit Settings > IAP tab.

    public static Action<string> IAPLog;

    List<InAppPurchaseData> consumablePurchaseRecord = new List<InAppPurchaseData>();
    List<InAppPurchaseData> activeNonConsumables = new List<InAppPurchaseData>();
    List<InAppPurchaseData> activeSubscriptions = new List<InAppPurchaseData>();

    #region Singleton Bool
    public static IAPManager Instance { get; private set; }
    private bool Singleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            Destroy(this);
            return false;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            return true;
        }
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        if (!Singleton()) return;
        Debug.Log("### AWAKE ### IAPManager");
    }
    void Start()
    {
        // Uncomment below if InitializeOnStart is not enabled in Huawei > Kit Settings > IAP tab.
        //HMSIAPManager.Instance.InitializeIAP();
    }
    private void OnEnable()
    {
        HMSIAPManager.Instance.OnBuyProductSuccess += OnBuyProductSuccess;
        HMSIAPManager.Instance.OnInitializeIAPSuccess += OnInitializeIAPSuccess;
        HMSIAPManager.Instance.OnInitializeIAPFailure += OnInitializeIAPFailure;
        HMSIAPManager.Instance.OnBuyProductFailure += OnBuyProductFailure;
    }
    private void OnDisable()
    {
        HMSIAPManager.Instance.OnBuyProductSuccess -= OnBuyProductSuccess;
        HMSIAPManager.Instance.OnInitializeIAPSuccess -= OnInitializeIAPSuccess;
        HMSIAPManager.Instance.OnInitializeIAPFailure -= OnInitializeIAPFailure;
        HMSIAPManager.Instance.OnBuyProductFailure -= OnBuyProductFailure;
    }
    #endregion

    public void InitializeIAP()
    {
        Debug.Log($"[IAPManager] InitializeIAP");
        HMSIAPManager.Instance.InitializeIAP();
    }

    public void RestoreProducts()
    {
        HMSIAPManager.Instance.RestorePurchaseRecords((restoredProducts) =>
        {
            foreach (var item in restoredProducts.InAppPurchaseDataList)
            {
                Debug.Log($"[IAPManager] [PURCHASE-RECORD] ProductId:{item.ProductId}, Kind:{item.Kind}, SubValid:{item.SubValid}, PurchaseToken:{item.PurchaseToken}, OrderID:{item.OrderID}");
                if ((IAPProductType)item.Kind == IAPProductType.Consumable)
                {
                    consumablePurchaseRecord.Add(item);

                    if (item.ProductId.StartsWith("cube_skin_"))
                    {
                        IAPLog?.Invoke(item.ProductId + " Purchased!");
                        PlayerPrefs.SetInt("purchased_" + item.ProductId, 1);
                        Managers.Reference.cubeSkinButtonsPriced.Find(x => x.name == item.ProductId)?.transform.GetChild(1)?.gameObject.SetActive(false);
                        Debug.Log($"[IAPManager] [RESTORED] ProductId:{item.ProductId}");
                    }
                }
            }
        });

        HMSIAPManager.Instance.RestoreOwnedPurchases((restoredProducts) =>
        {
            foreach (var item in restoredProducts.InAppPurchaseDataList)
            {
                Debug.Log($"[IAPManager] [OWNED-PURCHASE] ProductId:{item.ProductId}, Kind:{item.Kind}, PurchaseToken:{item.PurchaseToken}, OrderID:{item.OrderID}");

                if ((IAPProductType)item.Kind == IAPProductType.Subscription)
                {
                    Debug.Log($"[IAPManager] [SUBSCRIPTION] ProductId:{item.ProductId}, ExpirationDate:{item.ExpirationDate}, AutoRenewing {item.AutoRenewing}");
                    activeSubscriptions.Add(item);
                    Debug.Log($"[IAPManager] [RESTORED] ProductId:{item.ProductId}");
                }

                else if ((IAPProductType)item.Kind == IAPProductType.NonConsumable)
                {
                    Debug.Log($"[IAPManager] [NON-CONSUMABLE] ProductId:{item.ProductId}, DaysLasted:{item.DaysLasted}, SubValid:{item.SubValid}");
                    activeNonConsumables.Add(item);

                    if (item.ProductId == "removeads")
                    {
                        IAPLog?.Invoke("Ads Removed!");
                        Managers.Ad.RemoveAds();
                    }

                    Debug.Log($"[IAPManager] [RESTORED] ProductId:{item.ProductId}");
                }
            }
        });

    }

    public void PurchaseProduct(string productID)
    {
        Debug.Log($"[IAPManager] PurchaseProduct");

        HMSIAPManager.Instance.PurchaseProduct(productID);
    }

    #region Callbacks

    private void OnBuyProductSuccess(PurchaseResultInfo obj)
    {
        Debug.Log($"[IAPManager] OnBuyProductSuccess");
        Debug.Log($"[IAPManager] [PURCHASED] ProductId:{obj.InAppPurchaseData.ProductId}");

        if (obj.InAppPurchaseData.ProductId == "removeads")
        {
            IAPLog?.Invoke("Ads Removed!");
            Managers.Ad.RemoveAds();
        }
        else if (obj.InAppPurchaseData.ProductId.StartsWith("cube_skin_"))
        {
            IAPLog?.Invoke(obj.InAppPurchaseData.ProductId + " Purchased!");
            PlayerPrefs.SetInt("purchased_" + obj.InAppPurchaseData.ProductId, 1);
            Button button = Managers.Reference.cubeSkinButtonsPriced.Find(x => x.name == obj.InAppPurchaseData.ProductId);
            if (button != null)
            {
                button.transform.GetChild(1)?.gameObject.SetActive(false);
                Managers.Skin.SetCubeSkinPriced(button.transform.GetSiblingIndex() - Managers.Reference.cubeSkinButtons.Count);
                Managers.UI.SetCubeSkinButtonColors(button);
            }
        }
    }

    private void OnInitializeIAPFailure(HMSException obj)
    {
        IAPLog?.Invoke("IAP is not ready.");
        Debug.Log($"[IAPManager] OnInitializeIAPFailure");
    }

    private void OnInitializeIAPSuccess()
    {
        IAPLog?.Invoke("IAP is ready.");
        Debug.Log($"[IAPManager] OnInitializeIAPSuccess");

        RestoreProducts();
    }

    private void OnBuyProductFailure(int code)
    {
        IAPLog?.Invoke("Purchase Fail.");
        Debug.Log($"[IAPManager] OnBuyProductFailure");
    }

    #endregion
}