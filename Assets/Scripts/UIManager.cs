using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // TODO: move to gameManager or smth
    public int deckNum;
    public int discardNum;

    [SerializeField] private TextMeshProUGUI _deckText;
    [SerializeField] private TextMeshProUGUI _discardText;

    [SerializeField] private Transform _testSphere;
    [SerializeField] private LineRenderer _lineRenderer;

    void Start()
    {
        _deckText.text = $"Deck\n({deckNum})";
        _discardText.text = $"Discard\n({discardNum})";
    }

    private void Update()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.transform.name);
            _testSphere.position = hit.point;

            _lineRenderer.SetPosition(0, Camera.main.transform.position);
            _lineRenderer.SetPosition(1, hit.point);
        }
    }
}
