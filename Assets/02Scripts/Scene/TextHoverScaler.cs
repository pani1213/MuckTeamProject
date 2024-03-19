using TMPro;
using UnityEngine;

public class TextHoverScaler : MonoBehaviour
{
    public TextMeshProUGUI textMesh; // �ؽ�Ʈ �޽�
    public float scaleFactor = 1.2f; // Ŀ���� ����

    private Vector3 originalScale; // �ؽ�Ʈ�� ���� ũ��

    void Start()
    {
        // �ؽ�Ʈ�� ���� ũ�� ����
        originalScale = textMesh.transform.localScale;
    }

    void Update()
    {
        // ���콺 Ŀ�� ��ġ�� ȭ�� ��ǥ�� ������
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = 10f; // ȭ��� Ŀ�� ������ �Ÿ� ����

        // ȭ�� ��ǥ�κ��� Ray�� ���� �ؽ�Ʈ ���� �ִ��� Ȯ��
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == textMesh.gameObject)
            {
                // �ؽ�Ʈ ���� ���콺 Ŀ���� ���� �� �ؽ�Ʈ ũ�⸦ Ű��
                textMesh.transform.localScale = originalScale * scaleFactor;
            }
            else
            {
                // �ؽ�Ʈ ������ ����� �ؽ�Ʈ ũ�⸦ ������� �ǵ���
                textMesh.transform.localScale = originalScale;
            }
        }
        else
        {
            // �ؽ�Ʈ ������ ����� �ؽ�Ʈ ũ�⸦ ������� �ǵ���
            textMesh.transform.localScale = originalScale;
        }
    }
}