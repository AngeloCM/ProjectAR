using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{
    public static ARController Instance;

    public ARRaycastManager RaycastManager;

    public GameObject Webmodel;
    public GameObject current_ARGameobject;
    public GameObject selected_Gameobject;

    public GameObject crosshair;
    [SerializeField] GameObject btn_show;
    [SerializeField] GameObject btn_return;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip selectSound;
    [SerializeField] AudioClip deleteSound;
    [SerializeField] AudioClip holdSound;

    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    Pose pose;

    float touchStartTime = default;
    bool clipPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CrosshairCalculation();
        TouchGameObject();
    }

    public void TouchGameObject()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                touchStartTime = Time.time;
            }

            if(touch.phase == TouchPhase.Stationary && (Time.time - touchStartTime) >= 2f)
            {
                if (selected_Gameobject && !clipPlayed)
                {
                    audio.clip = holdSound;
                    audio.Play();
                    clipPlayed = true;
                }
            }

            // Perform actions for a single touch event
            RaycastHit hit;
            Ray ray = Server.Instance.Login.ARCamera.ScreenPointToRay(Input.GetTouch(0).position);

            if(touch.phase == TouchPhase.Ended)
            {
                float touchDuration = Time.time - touchStartTime;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("GameObject"))
                    {
                        if(touchDuration >= 2f)
                        {
                            DeleteModel(selected_Gameobject);
                            clipPlayed = false;
                        }
                        else
                        {
                            Debug.Log("Single touch detected on GameObject: " + hit.collider.gameObject);

                            selected_Gameobject = hit.collider.gameObject;
                            SelectModel(selected_Gameobject);
                        }
                    }
                    else
                    {
                        if (selected_Gameobject)
                        {
                            UnselectModel(selected_Gameobject);
                            selected_Gameobject = null;
                        }
                    }
                }
            }
        }
    }

    public void CrosshairCalculation()
    {
        Vector3 cameraOrigin = Server.Instance.Login.ARCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        Ray ray = Server.Instance.Login.ARCamera.ScreenPointToRay(cameraOrigin);

        //Checking if Crosshair is hitting a Plane to be able to Instantiate a Gameobject;
        if (RaycastManager.Raycast(ray, hitResults, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            pose = hitResults[0].pose;

            crosshair.transform.position = pose.position;
            crosshair.transform.rotation = pose.rotation;
            crosshair.GetComponent<SpriteRenderer>().color = Color.green;

            btn_show.GetComponent<Button>().interactable = true;
        }
        else
        {
            hitResults.Clear();
            crosshair.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void ShowModel()
    {
        btn_show.GetComponent<PlayAudio>().PlayButton();

        current_ARGameobject = Instantiate(Webmodel, pose.position, pose.rotation);
        current_ARGameobject.GetComponent<LeanDragTranslate>().enabled = false;
        current_ARGameobject.GetComponent<LeanPinchScale>().enabled = false;
        current_ARGameobject.GetComponent<LeanTwistRotateAxis>().enabled = false;
    }

    public void SelectModel(GameObject selected)
    {
        audio.clip = selectSound;
        audio.Play();
        StartCoroutine(HighlightGameObject(selected, 0.3f));

        //Enable LeanTouch of the Gameobject
        selected.GetComponent<LeanDragTranslate>().enabled = true;
        selected.GetComponent<LeanPinchScale>().enabled = true;
        selected.GetComponent<LeanTwistRotateAxis>().enabled = true;
    }

    public void UnselectModel(GameObject selected)
    {
        StopCoroutine(HighlightGameObject(selected, 0.3f));

        selected.GetComponent<LeanDragTranslate>().enabled = false;
        selected.GetComponent<LeanPinchScale>().enabled = false;
        selected.GetComponent<LeanTwistRotateAxis>().enabled = false;
    }

    public void DeleteModel(GameObject selected)
    {
        if (selected)
        {
            audio.clip = deleteSound;
            audio.Play();
            Destroy(selected);
        }
    }

    public void Return()
    {
        btn_return.GetComponent<PlayAudio>().PlayButton();

        //Turn On ModelManager Canvas;
        Server.Instance.ModelManager.gameObject.SetActive(true);
        Server.Instance.Login.CanvasCamera.gameObject.SetActive(true);

        Server.Instance.Login.ARPanel.gameObject.SetActive(false);
        Server.Instance.Login.ARCamera.gameObject.SetActive(false);
    }

    IEnumerator HighlightGameObject(GameObject selected, float time)
    {
        while (selected_Gameobject)
        {
            selected.GetComponentInChildren<Light>().intensity = 1f;

            yield return new WaitForSeconds(time);

            selected.GetComponentInChildren<Light>().intensity = .5f;

            yield return new WaitForSeconds(time);
        }
    }
}
