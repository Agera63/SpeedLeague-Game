using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

public class APIMananger
{
    public static APIMananger instance = new();

    private static readonly HttpClient client = new();
    private const string BaseUrl = "https://sl-api-mu.vercel.app";

    public async Task<bool> Login(string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/Auth/Login");

        var payload = new LoginRequest
        {
            userName = username,
            password = password
        };

        string json = JsonUtility.ToJson(payload);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        using var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            var loginResult = JsonUtility.FromJson<LoginResponse>(responseBody);

            PlayerPrefs.SetString("token", loginResult.accessToken);
            PlayerPrefs.Save();

            Debug.Log("Login successful.");

            // No userId in the login response — resolve it separately via GetAllUsers
            string userId = await GetCurrentUserId(username);
            if (userId != null)
            {
                PlayerPrefs.SetString("userId", userId);
                PlayerPrefs.Save();
                Debug.Log($"Resolved userId: {userId}");
            }
            else
            {
                Debug.LogError("Login succeeded but could not resolve a matching userId.");
            }

            return true;
        }
        else
        {
            Debug.LogError($"Login failed ({response.StatusCode})");
            return false;
        }
    }

    public async Task<bool> AddTime(float timeMs)
    {
        string trackId = await FindTrackIdByName(SettingsManager.instance.selectedTrack);
        string carId = await FindCarIdByName(SettingsManager.instance.selectedCarString);

        if (trackId == null || carId == null)
        {
            Debug.LogError("Could not resolve track or car — check the selected names match what's in the database.");
            return false;
        }

        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/api/Time");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", PlayerPrefs.GetString("token"));

        var payload = new AddTimeRequest
        {
            timeMs = timeMs,
            userId = PlayerPrefs.GetString("userId"),
            trackId = trackId,
            carId = carId
        };

        string json = JsonUtility.ToJson(payload);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        using var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            Debug.Log("Time added successfully.");
            return true;
        }
        else
        {
            string error = await response.Content.ReadAsStringAsync();
            Debug.LogError($"Failed to add time ({response.StatusCode}): {error}");
            return false;
        }
    }

    public async Task<bool> Validate()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/Auth/Validate");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", PlayerPrefs.GetString("token"));

        using var response = await client.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Debug.LogWarning("Token is invalid or expired — user needs to log in again.");
            PlayerPrefs.DeleteKey("token"); // clear the stale token
            return false;
        }

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            var result = JsonUtility.FromJson<ValidateResponse>(json);
            Debug.Log($"Token valid. Logged in as: {result.userName}");
            return true;
        }

        // Any other unexpected status (500, network issue, etc.)
        Debug.LogError($"Unexpected error validating token ({response.StatusCode})");
        return false;
    }

    //Helper methods for the Http calls

    public async Task<List<Car>> GetAllCars()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/Car");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", PlayerPrefs.GetString("token"));

        using var response = await client.SendAsync(request);

        string json = await response.Content.ReadAsStringAsync();
        Debug.Log($"GetAllCars status: {response.StatusCode}");
        Debug.Log($"GetAllCars raw response: {json}");

        if (!response.IsSuccessStatusCode)
        {
            Debug.LogError($"Failed to get cars ({response.StatusCode})");
            return new List<Car>();
        }

        string wrapped = "{\"items\":" + json + "}";
        var result = JsonUtility.FromJson<CarListWrapper>(wrapped);
        Debug.Log($"Parsed car count: {result.items?.Count ?? 0}");
        return result.items;
    }

    public async Task<List<Track>> GetAllTracks()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/Track");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", PlayerPrefs.GetString("token"));

        using var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            Debug.LogError($"Failed to get tracks ({response.StatusCode})");
            return new List<Track>();
        }

        string json = await response.Content.ReadAsStringAsync();
        string wrapped = "{\"items\":" + json + "}";
        return JsonUtility.FromJson<TrackListWrapper>(wrapped).items;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/api/User");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", PlayerPrefs.GetString("token"));

        using var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            Debug.LogError($"Failed to get users ({response.StatusCode})");
            return new List<User>();
        }

        string json = await response.Content.ReadAsStringAsync();
        string wrapped = "{\"items\":" + json + "}";
        return JsonUtility.FromJson<UserListWrapper>(wrapped).items;
    }

    public async Task<string> FindCarIdByName(string carName)
    {
        var cars = await GetAllCars();
        var match = cars.FirstOrDefault(c =>
            $"{c.brand}".Equals(carName, StringComparison.OrdinalIgnoreCase));
        return match?.carId;
    }

    public async Task<string> FindTrackIdByName(string trackName)
    {
        var tracks = await GetAllTracks();
        var match = tracks.FirstOrDefault(t =>
            t.name.Equals(trackName, StringComparison.OrdinalIgnoreCase));
        return match?.trackId;
    }

    public async Task<string> GetCurrentUserId(string username)
    {
        var users = await GetAllUsers();
        var match = users.FirstOrDefault(u =>
            u.username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return match?.userId;
    }
}

//Allows an easier conversion of information for specific JSON with [Serializable]
[Serializable]
public class AddTimeRequest
{
    public float timeMs;
    public string userId;
    public string trackId;
    public string carId;
}

[Serializable]
public class LoginRequest
{
    public string userName;
    public string password;
}

[Serializable]
public class LoginResponse
{
    public string accessToken;
    public string userName;
    public int expiresIn;
}

[Serializable]
public class Car
{
    public string carId;
    public string brand;
    public string model;
}

[Serializable]
public class Track
{
    public string trackId;
    public string name;
    public string country;
}

[Serializable]
public class User
{
    public string userId;
    public string username;
}

[Serializable]
public class CarListWrapper
{
    public List<Car> items;
}

[Serializable]
public class TrackListWrapper
{
    public List<Track> items;
}

[Serializable]
public class UserListWrapper
{
    public List<User> items;
}

[Serializable]
public class ValidateResponse
{
    public bool valid;
    public string userName;
}