using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Vopere.Protection
{
    public class ProtectionKey : MonoBehaviour
    {
        public MacAdress macAdress;
        public string key;
        public TMP_InputField activationInputField;

        [Header("Generating")]
        public bool useForGenerator = false;
        public TMP_InputField generatingInputField;

        public UnityEvent onFirstTime;
        public UnityEvent onSucceed;
        public UnityEvent onFailed;

        string loadedKey;

        void Start()
        {
            GenerateKey();
            LoadKey();
        }

        void GenerateKey()
        {
            key = ComputeHash(macAdress.MyMacAdress);

            if (useForGenerator)
                generatingInputField.text = key;
        }

        public void CheckKey()
        {
            if (activationInputField.text == key)
            {
                onSucceed.Invoke();
                SaveKey();
                Debug.Log("Ключ подтверждён");
            }
            else
            {
                onFailed.Invoke();
                Debug.LogWarning("Неверный ключ защиты! Выход");
            }
        }

        void SaveKey()
        {
            PlayerPrefs.SetString("String", key);
            PlayerPrefs.Save();
            Debug.Log("Ключ сохранён");
        }

        void LoadKey()
        {
            if (PlayerPrefs.HasKey("String"))
            {
                loadedKey = PlayerPrefs.GetString("String");
            }
            else
            {
                onFirstTime.Invoke();
                Debug.LogWarning("Нет сохранённого ключа");
            }

            if (loadedKey == key)
                onSucceed.Invoke();
            else
                onFirstTime.Invoke();
        }

        string ComputeHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hash = MD5.Create().ComputeHash(inputBytes);
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        public void CopyTextToClipboard()
        {
            string textToCopy = key;

            GUIUtility.systemCopyBuffer = textToCopy;
        }
    }
}
