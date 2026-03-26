using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Profile;

[InitializeOnLoad]
public class EnsureDefaultPlatform : MonoBehaviour
{
    // Path to build profile
    private const string ProfilePath = "Assets/Settings/Build Profiles/StartDemo.asset";

    static EnsureDefaultPlatform()
    {
        // Get the currently active profile
        BuildProfile activeProfile = BuildProfile.GetActiveBuildProfile();

        // Check if it is already set to StartDemo
        if (activeProfile == null || activeProfile.name != "StartDemo" && activeProfile.name == "Windows")
        {
            // Load the StartDemo asset
            BuildProfile demoProfile = AssetDatabase.LoadAssetAtPath<BuildProfile>(ProfilePath);

            if (demoProfile != null)
            {
                // Switch to it
                BuildProfile.SetActiveBuildProfile(demoProfile);
                Debug.Log("Startup override: Active Build Profile forced to StartDemo.");
            }
            else
            {
                Debug.LogWarning($"ForceStartDemoProfile script couldn't find the Build Profile at: {ProfilePath}. Check the file path!");
            }
        }
    }
}
