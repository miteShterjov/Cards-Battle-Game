using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndCreditsMenuUI : MonoBehaviour
    {
        [Header("EndCredits Menu Config")]
        [SerializeField] private TextMeshProUGUI endCreditsText;
        [SerializeField] private float scrollSpeed = 10f;
        [Header("Special Section Config")]
        [SerializeField] private TextMeshProUGUI specialSectionText;
        [SerializeField] private Button specialSectionButton;
        [SerializeField] private Image specialSectionImage;
        [SerializeField] private float specialSectionScrollSpeed = 9f;
        [SerializeField] private float specialSectionBreakPoint = -200f;

        private void Update()
        {
            ScrollingEndCreditsText();
            if (specialSectionText.rectTransform.position.y < specialSectionBreakPoint) ScrollingSpecialSection();
        }

        private void ScrollingEndCreditsText()
        {
            endCreditsText.transform.Translate(Vector3.up * (Time.deltaTime * scrollSpeed));
        }

        private void ScrollingSpecialSection()
        {
            specialSectionText.transform.Translate(Vector3.up * (Time.deltaTime * specialSectionScrollSpeed));
            specialSectionButton.transform.Translate(Vector3.up * (Time.deltaTime * specialSectionScrollSpeed));
            specialSectionImage.transform.Translate(Vector3.up * (Time.deltaTime * specialSectionScrollSpeed));
        }
    }
}
