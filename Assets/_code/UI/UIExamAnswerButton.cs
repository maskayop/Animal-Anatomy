using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace AnimalAnatomy
{
    public class UIExamAnswerButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] GameObject selectionImage;

        public bool isCorrectAnswer;

        Button button;

        public void Init(BodyPartInfo info, bool isCorrect)
        {
            button = GetComponent<Button>();
            SetInteractable(true);

            if (ExaminationController.Instance.examDifficulty == 0)
                nameText.text = info.partName;
            else if (ExaminationController.Instance.examDifficulty == 1)
                nameText.text = info.partScientificName;
            
            isCorrectAnswer = isCorrect;
            ShowSelection(false);
        }

        public void CheckAnswer()
        {
            button.interactable = false;
            SetInteractable(false);
            ShowSelection(true);

            ExaminationController.Instance.FinishQuestion(isCorrectAnswer);
        }

        public void SetInteractable(bool state)
        {
            button.interactable = state;
        }

        public void ShowSelection(bool state)
        {
            selectionImage.SetActive(state);
        }
    }
}
