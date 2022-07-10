using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Player : MonoBehaviour
{
    private CharacterController _playerController;
    [SerializeField]
    private GameObject _cameraPlayer;
    [SerializeField]
    private float _camVeloc = 2f;
    [SerializeField]
    private float _velocidadePlayer = 1f;
    [SerializeField]
    private GameObject muzzle;
    [SerializeField]
    private GameObject HitPref;
    [SerializeField]
    private AudioSource ShotAudio;
    [SerializeField]
    private int muniAtual;
    [SerializeField]
    private int muniMax = 300;
    private bool recargr = false;
    [SerializeField]
    private TextMeshPro _textMesh;
    [SerializeField]
    private GameObject BoxDamaged;
    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        muniAtual = muniMax;
        
    }

    // Update is called once per frame
    void Update()
    {
        MovimentoPlayer();
        MoviemntoCamera();

        if(Input.GetMouseButton(0) && muniAtual > 0)
        {
            atirar();
        }
        else
        {
            muzzle.SetActive(false);
            ShotAudio.Stop();
        }

        if (Input.GetKeyDown(KeyCode.R) && recargr == false)
        {
            recargr = true;
            StartCoroutine(recarg());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }  
    }

    void MovimentoPlayer()
    {
        Vector3 direcao = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 mov = direcao * _velocidadePlayer;
        mov.y -= 9.81f;
        mov = transform.TransformDirection(mov);
        _playerController.Move(mov * Time.deltaTime);
    }
    void MoviemntoCamera()
    {
        float _camY = Input.GetAxis("Mouse X");
        float _camX = Input.GetAxis("Mouse Y");

        Vector3 _rotacaoPlayer = transform.localEulerAngles;
        _rotacaoPlayer.y += _camY * _camVeloc;
        transform.localEulerAngles = _rotacaoPlayer;

        Vector3 _rotacaoCamera = _cameraPlayer.transform.localEulerAngles;
        _rotacaoCamera.x -= _camX * _camVeloc;
        _cameraPlayer.transform.localEulerAngles = _rotacaoCamera;

    }

    void atirar()
    {
        if (Input.GetMouseButton(0))
        {
            Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit HitInfo;

            muzzle.SetActive(true);
            muniAtual--;
            _textMesh.SetText(muniAtual.ToString());
            if (ShotAudio.isPlaying == false)
            {
                ShotAudio.Play();
            }


            if (Physics.Raycast(rayOrigin, out HitInfo))
            {
                Debug.Log("Atingiu: " + HitInfo.transform.name);
                GameObject HitMarker = Instantiate(HitPref, HitInfo.point, Quaternion.LookRotation(HitInfo.normal)) as GameObject;
                Destroy(HitMarker, 1f);

                if(HitInfo.transform.tag == "Box")
                {
                    GameObject HitBox = HitInfo.transform.gameObject;
                    Instantiate(BoxDamaged, HitInfo.transform.position, HitInfo.transform.rotation);
                    Destroy(HitBox);
                }


            }
        }
    }

    IEnumerator recarg()
    {
        yield return new WaitForSeconds(1.5f);
        muniAtual = muniMax;
        _textMesh.SetText(muniAtual.ToString());
        recargr = false;
    }
}
