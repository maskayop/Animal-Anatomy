using System;
using System.Text;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Vopere.Protection
{
    public class Protection : MonoBehaviour
    {
        [Header("Canvases")]
        public GameObject clientCanvas;
        public GameObject generatorCanvas;

        [Header("Check and Activation")]
        public string checkKey;
        public TMP_InputField checkInputField;
        public string key;
        public TMP_InputField activationInputField;

        [Header("Generating")]
        public bool useForGenerator = false;
        public TMP_InputField generatingInputField;
        public TMP_InputField activatingInputField;

        [Space(20)]
        public UnityEvent onFirstTime;
        public UnityEvent onSucceed;
        public UnityEvent onFailed;

        string loadedKey;

        void Start()
        {
            if (useForGenerator)
                Destroy(clientCanvas);
            else
                Destroy(generatorCanvas);
            
            GenerateKey();
            LoadKey();
        }

        void GenerateKey()
        {
            checkKey = ComputeHash(GetUserName() + GetGPUInfo() + GetCPUInfo());
            checkInputField.text = checkKey;
            key = GetEditedKey(checkKey);

            Debug.Log(checkKey);
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

                ExitGame();
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
                stringBuilder.Append(hash[i].ToString("X2"));

            return stringBuilder.ToString();
        }

        public string GetEditedKey(string INkey)
        {
            string value = ComputeHash(INkey);
            char[] chars = value.ToCharArray();
            value = "";

            //  A B   C D   E F   G H   I J   K L   M N   O P   Q R   S T   U V   W X   Y Z
            if (char.IsNumber(chars[0]))
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    if (chars[i].ToString() == "1")
                        value += "A";
                    else if (chars[i].ToString() == "2")
                        value += "B";
                    else if (chars[i].ToString() == "3")
                        value += "C";
                    else if (chars[i].ToString() == "4")
                        value += "D";
                    else if (chars[i].ToString() == "5")
                        value += "E";
                    else if (chars[i].ToString() == "6")
                        value += "F";
                    else if (chars[i].ToString() == "7")
                        value += "G";
                    else if (chars[i].ToString() == "8")
                        value += "H";
                    else if (chars[i].ToString() == "9")
                        value += "I";
                    else if (chars[i].ToString() == "0")
                        value += "J";
                    else if (chars[i].ToString() == "A")
                        value += "1";
                    else if (chars[i].ToString() == "B")
                        value += "2";
                    else if (chars[i].ToString() == "C")
                        value += "3";
                    else if (chars[i].ToString() == "D")
                        value += "4";
                    else if (chars[i].ToString() == "E")
                        value += "5";
                    else if (chars[i].ToString() == "F")
                        value += "6";
                    else if (chars[i].ToString() == "G")
                        value += "7";
                    else if (chars[i].ToString() == "H")
                        value += "8";
                    else if (chars[i].ToString() == "I")
                        value += "9";
                    else
                        value += "0";
                }
            }
            //  A B   C D   E F   G H   I J   K L   M N   O P   Q R   S T   U V   W X   Y Z
            else
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    if (chars[i].ToString() == "1")
                        value += "Z";
                    else if (chars[i].ToString() == "2")
                        value += "Y";
                    else if (chars[i].ToString() == "3")
                        value += "X";
                    else if (chars[i].ToString() == "4")
                        value += "W";
                    else if (chars[i].ToString() == "5")
                        value += "V";
                    else if (chars[i].ToString() == "6")
                        value += "U";
                    else if (chars[i].ToString() == "7")
                        value += "T";
                    else if (chars[i].ToString() == "8")
                        value += "S";
                    else if (chars[i].ToString() == "9")
                        value += "R";
                    else if (chars[i].ToString() == "0")
                        value += "Q";
                    else if (chars[i].ToString() == "A")
                        value += "9";
                    else if (chars[i].ToString() == "B")
                        value += "8";
                    else if (chars[i].ToString() == "C")
                        value += "7";
                    else if (chars[i].ToString() == "D")
                        value += "6";
                    else if (chars[i].ToString() == "E")
                        value += "5";
                    else if (chars[i].ToString() == "F")
                        value += "4";
                    else if (chars[i].ToString() == "G")
                        value += "3";
                    else if (chars[i].ToString() == "H")
                        value += "2";
                    else if (chars[i].ToString() == "I")
                        value += "1";
                    else
                        value += "0";
                }
            }

            return value;
        }

        public void CopyActivationKeyToClipboard()
        {
            GUIUtility.systemCopyBuffer = activatingInputField.text;
        }

        public void CopyCheckKeyToClipboard()
        {
            GUIUtility.systemCopyBuffer = checkInputField.text;
        }

        public void PasteActivationKeyFromClipboard()
        {
            activationInputField.text = GUIUtility.systemCopyBuffer;
        }

        public void ShowGeneratedKey()
        {
            activatingInputField.text = GetEditedKey(generatingInputField.text);
        }

        string GetUserName()
        {
            Debug.Log("------");
            
            string userName = Environment.UserName;

#if UNITY_STANDALONE_WIN
            Debug.Log($"Windows пользователь: {userName}");
#elif UNITY_STANDALONE_OSX
            Debug.Log($"macOS пользователь: {userName}");
#elif UNITY_STANDALONE_LINUX
            Debug.Log($"Linux пользователь: {userName}");
#else
            Debug.Log($"Пользователь ({Application.platform}): {userName}");
#endif

            return userName;
        }

        string GetGPUInfo()
        {
            string gpuName = SystemInfo.graphicsDeviceName;
            string gpuVendor = SystemInfo.graphicsDeviceVendor;
            int gpuMemory = SystemInfo.graphicsMemorySize;
            string gpuVersion = SystemInfo.graphicsDeviceVersion;

            Debug.Log($"Видеокарта: {gpuName}");
            Debug.Log($"Производитель: {gpuVendor}");
            Debug.Log($"Видеопамять: {gpuMemory} MB");
            Debug.Log($"Драйвер: {gpuVersion}");

            return gpuName + "_" + gpuVendor + "_" + gpuMemory;
        }

        string GetCPUInfo()
        {
            string processorName = SystemInfo.processorType;
            int processorCount = SystemInfo.processorCount;
            int processorFrequency = SystemInfo.processorFrequency;

            Debug.Log($"Процессор: {processorName}");
            Debug.Log($"Количество ядер: {processorCount}");
            Debug.Log($"Частота: {processorFrequency} MHz");

            return processorName;
        }

        public void ExitGame()
        {
            Debug.Log("Выход из программы" + "\n");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }
    }
}
