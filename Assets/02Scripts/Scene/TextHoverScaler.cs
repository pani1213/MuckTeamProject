using TMPro;
using UnityEngine;

public class TextHoverScaler : MonoBehaviour
{
    public TextMeshProUGUI textMesh; // 텍스트 메시
    public float scaleFactor = 1.2f; // 커지는 비율

    private Vector3 originalScale; // 텍스트의 원래 크기

    void Start()
    {
        // 텍스트의 원래 크기 저장
        originalScale = textMesh.transform.localScale;
    }

    void Update()
    {
        // 마우스 커서 위치를 화면 좌표로 가져옴
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = 10f; // 화면과 커서 사이의 거리 설정

        // 화면 좌표로부터 Ray를 쏴서 텍스트 위에 있는지 확인
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == textMesh.gameObject)
            {
                // 텍스트 위에 마우스 커서가 있을 때 텍스트 크기를 키움
                textMesh.transform.localScale = originalScale * scaleFactor;
            }
            else
            {
                // 텍스트 위에서 벗어나면 텍스트 크기를 원래대로 되돌림
                textMesh.transform.localScale = originalScale;
            }
        }
        else
        {
            // 텍스트 위에서 벗어나면 텍스트 크기를 원래대로 되돌림
            textMesh.transform.localScale = originalScale;
        }
    }
}