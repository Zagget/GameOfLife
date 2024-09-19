using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    public Slider speedSlider;
    public Slider spawnChanceSlider;
    public Slider totalCellsSlider;
    public Button newGameButton;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI spawnChanceText;
    public TextMeshProUGUI totalCellsText;

    void Start()
    {
        speedSlider.value = gameManager.speed;
        spawnChanceSlider.value = gameManager.spawnChancePercentage;
        totalCellsSlider.value = gameManager.totalCells;


        speedSlider.onValueChanged.AddListener(OnSpeedSlider);
        spawnChanceSlider.onValueChanged.AddListener(OnSpawnChance);
        totalCellsSlider.onValueChanged.AddListener(OnTotalCell);
        newGameButton.onClick.AddListener(OnNewGame);
    }


    void OnSpeedSlider(float value)
    {
        gameManager.SetSpeed(Mathf.RoundToInt(value));
        speedText.text = Mathf.RoundToInt(value).ToString();
    }

    void OnSpawnChance(float value)
    {
        gameManager.SetSpawnChance(Mathf.RoundToInt(value));
        spawnChanceText.text = Mathf.RoundToInt(value) + "%";
    }

    void OnTotalCell(float value)
    {
        gameManager.SetTotalCells(Mathf.RoundToInt(value));
        totalCellsText.text = Mathf.RoundToInt(value).ToString();
    }

    void OnNewGame()
    {
        gameManager.NewGame();
    }
}
