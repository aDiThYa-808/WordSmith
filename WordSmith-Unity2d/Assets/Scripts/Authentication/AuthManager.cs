using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;

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

    private string baseUrl = "https://word-smith-c8ny.onrender.com/api/auth"; 
    public string username;

    [System.Serializable]
    public class ResponseData
    {
        public string token;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Sign-up logic
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
        string jsonData = JsonConvert.SerializeObject(new { username, password });

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/register", "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Sign Up Success: " + request.downloadHandler.text);
            warningTextSignUp.color = Color.green;
            warningTextSignUp.text = "Account created! Sign in to continue";
        }
        else
        {
            // Handle duplicate key error (username already taken)
            warningTextSignUp.text = request.downloadHandler.text.Contains("E11000 duplicate key error")
                ? "Username already taken!"
                : "Sign Up Failed!";
            Debug.LogError("❌ Sign Up Error: " + request.downloadHandler.text);
        }
    }

    // Sign-in logic
    public void SignIn()
    {
        username = usernameSignInInput.text.Trim();
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
        string jsonData = JsonConvert.SerializeObject(new { username, password });

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/login", "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("✅ API Response: " + jsonResponse);

            try
            {
                // Try parsing the response
                ResponseData responseData = JsonConvert.DeserializeObject<ResponseData>(jsonResponse);
                string token = responseData?.token;

                if (string.IsNullOrEmpty(token))
                {
                    Debug.LogError("❌ API response does not contain a valid token!");
                    warningTextSignIn.text = "Invalid response from server!";
                    yield break;
                }

                Debug.Log("🔹 Retrieved token: " + token);

                // Save username to the instance variable
                username = usernameSignInInput.text.Trim();

                SignInBtnText.text = "Loading...";
                StartCoroutine(LoadScene("Home"));  // Load Level-1 after sign-in
            }
            catch (Exception e)
            {
                Debug.LogError("❌ JSON Parsing Error: " + e.Message);
                warningTextSignIn.text = "Error processing server response!";
            }
        }
        else
        {
            warningTextSignIn.text = "Sign In Failed!";
            Debug.LogError("❌ Sign In Error: " + request.error);
        }
    }

    // Get the username
    public string GetUsername()
    {
        return username;
    }

    // Load a new scene with a delay
    public IEnumerator LoadScene(string SceneName)
    {
        yield return new WaitForSeconds(2f); // Give time for PlayerPrefs to save
        AsyncOperation scene = SceneManager.LoadSceneAsync(SceneName);
        scene.allowSceneActivation = false;

        while (!scene.isDone)
        {
            if (scene.progress >= 0.9f)
            {
                yield return new WaitForSeconds(2f); // Short delay before activation
                scene.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    // Switch to sign-up form
    public void SwitchToSignUp()
    {
        SignInField.SetActive(false);
        SignUpField.SetActive(true);
        usernameSignInInput.text = passwordSignInInput.text = "";
        warningTextSignIn.text = "";
    }

    // Switch to sign-in form
    public void SwitchToSignIn()
    {
        SignUpField.SetActive(false);
        SignInField.SetActive(true);
        usernameSignUpInput.text = passwordSignUpInput.text = "";
        warningTextSignUp.text = "";
    }
}
