using UnityEngine;
using UnityEngine.SceneManagement;

namespace Store
{
    public class CategorySortUI : MonoBehaviour
    {
        [Header("Store Panels")]
        [SerializeField] private GameObject cardsStore;
        [SerializeField] private GameObject cardBackStore;
        [SerializeField] private GameObject playerSkinStore;

        private void Start() => ShowCardsStore(); 

        public void OnGoBackButtonClicked() => SceneManager.LoadScene("MainMenu");
        public void ShowCardsStore() => ShowOnly(cardsStore);
        public void ShowCardBackStore() => ShowOnly(cardBackStore);
        public void ShowPlayerSkinStore() => ShowOnly(playerSkinStore);

        private void ShowOnly(GameObject target)
        {
            cardsStore.SetActive(cardsStore == target);
            cardBackStore.SetActive(cardBackStore == target);
            playerSkinStore.SetActive(playerSkinStore == target);
        }
    }
}