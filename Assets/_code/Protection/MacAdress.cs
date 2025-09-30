using UnityEngine;
using System.Net.NetworkInformation;

namespace Vopere.Protection
{
	public class MacAdress : MonoBehaviour
	{
		public string MyMacAdress;
		public string ControlMacAdress = "B4-2E-99-D1-2B-D1";

		public bool closeAppOnFailed = true;

		void Awake()
		{
			CheckForMacAdress();
		}

		public void CheckForMacAdress()
		{
			ShowNetworkInterfaces();

			if (MyMacAdress != ControlMacAdress && closeAppOnFailed)
				Quit();
		}

		string GetMacAddress()
		{
			string macAddresses = "";
			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (nic.OperationalStatus == OperationalStatus.Up)
				{
					macAddresses += nic.GetPhysicalAddress().ToString();
					break;
				}
			}

			return macAddresses;
		}

		public void ShowNetworkInterfaces()
		{
			NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

			foreach (NetworkInterface adapter in nics)
			{
				PhysicalAddress address = adapter.GetPhysicalAddress();
				byte[] bytes = address.GetAddressBytes();
				string mac = null;
				for (int i = 0; i < bytes.Length; i++)
				{
					mac = string.Concat(mac + (string.Format("{0}", bytes[i].ToString("X2"))));
					if (i != bytes.Length - 1)
					{
						mac = string.Concat(mac + "-");
					}
				}
				MyMacAdress += mac;
			}

			//Debug.Log(MyMacAdress);
		}

		public void Quit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		}
	}
}
