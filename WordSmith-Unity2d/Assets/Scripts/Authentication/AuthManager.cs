using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [Header("Sign in")]
    public GameObject SignInField;
    public TMP_InputField usernameSignInInput;
    public TMP_InputField passwordSignInInput;
    public TextMeshProUGUI warningTextSignIn;
    public TextMeshProUGUI SignInBtnText;

    [Header("Sign up")]
    public GameObject SignUpField;
    public TMP_InputField usernameSignUpInput;
    public TMP_InputField passwordSignUpInput;
    public TextMeshProUGUI warningTextSignUp;

    private string baseUrl = "http://localhost:5000/api/auth"; // Change this if deployed

    // 🔹 Sign Up Function
    public void SignUp()
    {
        string username = usernameSignUpInput.text.Trim();
        string password = passwordSignUpInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            warningTextSignUp.text = "Username and password required!";
            return;
        }

        StartCoroutine(SignUpRequest(username, password));
    }

    IEnumerator SignUpRequest(string username, string password)
    {
        string jsonData = "{\"username\":\"" + username + "\", \"password\":\"" + password + "\"}";

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/register", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //What happens after account in created : 
            Debug.Log("Sign Up Success: " + request.downloadHandler.text);
            warningTextSignUp.color = Color.green;
            warningTextSignUp.text = "Account created successfully! Sign in to continue";
        }
        else
        {
            string errorMessage = request.downloadHandler.text;

            if (errorMessage.Contains("E11000 duplicate key error"))
            {
                warningTextSignUp.text = "Username already taken!";
            }
            else
            {
                warningTextSignUp.text = "Sign Up Failed!";
            }

            Debug.LogError("Sign Up Error: " + errorMessage);
        }
    }

    // 🔹 Sign In Function
    public void SignIn()
    {
        string username = usernameSignInInput.text.Trim();
        string password = passwordSignInInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            warningTextSignIn.text = "Username and password required!";
            return;
        }

        StartCoroutine(SignInRequest(username, password));
    }

    IEnumerator SignInRequest(string username, string password)
    {
        string jsonData = "{\"username\":\"" + username + "\", \"password\":\"" + password + "\"}";

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/login", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Load next scene 
            Debug.Log("Sign In Success: " + request.downloadHandler.text);
            SignInBtnText.text = "Loading...";
            StartCoroutine(LoadScene("Home"));
        }
        else
        {
            warningTextSignIn.text = "Sign In Failed!";
            Debug.LogError("Sign In Error: " + request.error);
        }
    }

    public IEnumerator LoadScene(string SceneName)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(SceneName);
        scene.allowSceneActivation = false;
        while (!scene.isDone)
        {
            float progress = Mathf.Clamp01(scene.progress / 0.9f);
            if(progress >= 1f)
            {
                scene.allowSceneActivation = true;
            }
            yield return null;
        }

    }


    public void SwitchToSignUp()
    {
        SignInField.SetActive(false);
        SignUpField.SetActive(true);
        warningTextSignIn.text = "";
        usernameSignInInput.text = "";
        passwordSignInInput.text = "";
    }

    public void SwitchToSignIn()
    {
        SignUpField.SetActive(false);
        SignInField.SetActive(true);
        warningTextSignUp.text = "";
        usernameSignUpInput.text = "";
        passwordSignUpInput.text = "";
    }
}
