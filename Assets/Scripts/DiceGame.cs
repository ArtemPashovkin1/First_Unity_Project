using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DiceGame : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private float forceMin = 4f;
    [SerializeField] private float forceMax = 6f;
    [SerializeField] private float spreadMin = 0.15f;
    [SerializeField] private float spreadMax = 0.5f;
    [SerializeField] private float torqueMin = 3f;
    [SerializeField] private float torqueMax = 7f;
    [SerializeField] private float restTime = 0.5f;
    [SerializeField] private InputActionReference throwAction;
    [SerializeField] private Text keyLabel;
    [SerializeField] private Button rebindButton;
    [SerializeField] private DiceUi ui;
    [SerializeField] private TMP_InputField diceCountInput;
    [SerializeField] private TMP_InputField loseInput;
    [SerializeField] private TMP_InputField drawInput;
    [SerializeField] private TMP_InputField winInput;

    private readonly List<Dice> dices = new List<Dice>();
    private readonly RoundModel model = new RoundModel();
    private float restTimer;
    private bool rolling;

    private void Awake()
    {
        if (dicePrefab == null)
        {
            Debug.LogError("DiceGame: не назначен префаб кубика!", this);
            return;
        }

        model.RandomizeThresholds();
        SpawnDices();

        rebindButton.onClick.AddListener(StartRebind);
        diceCountInput.onEndEdit.AddListener(OnInputChanged);
        loseInput.onEndEdit.AddListener(OnInputChanged);
        drawInput.onEndEdit.AddListener(OnInputChanged);
        winInput.onEndEdit.AddListener(OnInputChanged);

        RefreshKeyLabel();
        ui.Apply(model);
    }

    private void OnEnable()
    {
        throwAction.action.performed += OnThrow;
        throwAction.action.Enable();
    }

    private void OnDisable()
    {
        throwAction.action.performed -= OnThrow;
        rebindButton.onClick.RemoveListener(StartRebind);
        diceCountInput.onEndEdit.RemoveListener(OnInputChanged);
        loseInput.onEndEdit.RemoveListener(OnInputChanged);
        drawInput.onEndEdit.RemoveListener(OnInputChanged);
        winInput.onEndEdit.RemoveListener(OnInputChanged);
    }

    public void Reroll()
    {
        if (rolling) return;
        model.RandomizeThresholds();
        ApplyInputs();
        model.ResetRoll();
        RespawnDices();
        Throw();
        ui.Apply(model);
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        Throw();
    }

    private void Throw()
    {
        if (rolling) return;
        rolling = true;
        restTimer = 0f;
        model.ResetRoll();
        ui.Apply(model);

        for (int i = 0; i < dices.Count; i++)
        {
            float force = Random.Range(forceMin, forceMax);
            float spread = Random.Range(spreadMin, spreadMax);
            Vector3 dir = new Vector3(Random.Range(-spread, spread), 1f, Random.Range(-spread, spread)).normalized;
            dices[i].Body.AddForce(dir * force, ForceMode.Impulse);
            dices[i].Body.AddTorque(Random.onUnitSphere * Random.Range(torqueMin, torqueMax), ForceMode.Impulse);
        }
    }

    private void Update()
    {
        if (!rolling) return;

        bool allSleep = true;
        for (int i = 0; i < dices.Count; i++)
        {
            if (!dices[i].IsSleeping())
            {
                allSleep = false;
                break;
            }
        }

        restTimer = allSleep ? restTimer + Time.deltaTime : 0f;
        if (restTimer < restTime) return;

        rolling = false;
        List<int> values = new List<int>();
        for (int i = 0; i < dices.Count; i++)
            values.Add(dices[i].TopFace());
        model.ApplyValues(values);
        ui.Apply(model);
    }

    private void OnInputChanged(string value)
    {
        ApplyInputs();
        ui.Apply(model);
    }

    private void ApplyInputs()
    {
        if (int.TryParse(diceCountInput.text, out int count))
            model.SetDiceCount(count);
        if (int.TryParse(loseInput.text, out int lose))
            model.SetLose(lose);
        if (int.TryParse(drawInput.text, out int draw))
            model.SetDraw(draw);
        if (int.TryParse(winInput.text, out int win))
            model.SetWin(win);
    }

    private void SpawnDices()
    {
        for (int i = 0; i < model.DiceCount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-1.5f, 1.5f), 0.25f, Random.Range(-1.5f, 1.5f));
            Dice dice = Instantiate(dicePrefab, pos, Quaternion.identity, transform).GetComponent<Dice>();
            dices.Add(dice);
        }
    }

    private void RespawnDices()
    {
        for (int i = 0; i < dices.Count; i++)
        {
            if (dices[i] != null)
                Destroy(dices[i].gameObject);
        }
        dices.Clear();
        SpawnDices();
    }

    private void StartRebind()
    {
        throwAction.action.Disable();
        throwAction.action.PerformInteractiveRebinding()
            .WithControlsHavingToMatchPath("<Keyboard>")
            .OnComplete(op =>
            {
                op.Dispose();
                throwAction.action.Enable();
                RefreshKeyLabel();
            })
            .Start();
    }

    private void RefreshKeyLabel()
    {
        if (keyLabel != null)
            keyLabel.text = "Кнопка: " + throwAction.action.GetBindingDisplayString();
    }
}