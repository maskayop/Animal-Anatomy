using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    public class UISystemActivatingButtonsPanel : MonoBehaviour
    {
        public List<UIButtonSystemActivating> systemActivatingButtons = new List<UIButtonSystemActivating>();

        void Start()
        {
            Init();
        }

        public void Init()
        {
            for (int i = 0; i < systemActivatingButtons.Count; i++)
                    systemActivatingButtons[i].Init();

            CollapseSystemActivatingButtons(true);
        }

        public void CollapseSystemActivatingButtons(bool state)
        {
            for (int i = 0; i < systemActivatingButtons.Count; i++)
            {
                if (state)
                    systemActivatingButtons[i].Collapse();
                else
                    systemActivatingButtons[i].Expand();
            }
        }

        public void ActivateAllSystems(bool state)
        {
            for (int i = 0; i < systemActivatingButtons.Count; i++)
            {
                systemActivatingButtons[i].SetActiveState(state);
            }
        }
    }
}
