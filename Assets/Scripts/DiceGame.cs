using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DiceGame : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField, Min(1)] private int diceCount = 5;
    [SerializeField] private float forceMin = 4f;
    [SerializeField] private float forceMax = 6f;
    [SerializeField] private float spreadMin = 0.15f;
    [SerializeField] private float spreadMax = 0.5f;
    [SerializeField] private float torqueMin = 3f;
    [SerializeField] private float torqueMax = 7f;
    [SerializeField] private float restTime = 0.5f;
    [SerializeField] private InputActionReference throwAction;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text keyLabel;
    [SerializeField] private Button rebindButton;

    private readonly List<Dice> dices = new List<Dice>();
    private float restTimer;
    private bool rolling;

    private void Awake()
    {
        if (dicePrefab == null)
        {
            Debug.LogError("DiceGame: не назначен префаб кубика!", this);
            return;
        }

        for (int i = 0; i < diceCount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-1.5f, 1.5f), 0.25f, Random.Range(-1.5f, 1.5f));
            Dice dice = Instantiate(dicePrefab, pos, Quaternion.identity, transform).GetComponent<Dice>();
            dices.Add(dice);
        }

        rebindButton.onClick.AddListener(StartRebind);
        RefreshKeyLabel();
        if (scoreLabel != null) scoreLabel.text = "Очки: —";
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
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        if (rolling) return;
        rolling = true;
        restTimer = 0f;

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
        if (restTimer >= restTime)
        {
            rolling = false;
            CountScore();
        }
    }

    private void CountScore()
    {
        int sum = 0;
        for (int i = 0; i < dices.Count; i++)
            sum += dices[i].TopFace();
        if (scoreLabel != null) scoreLabel.text = "Очки: " + sum;
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