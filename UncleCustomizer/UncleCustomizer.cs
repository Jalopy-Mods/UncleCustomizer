using JaLoader;
using UnityEngine;

namespace UncleCustomizer
{
    public class UncleCustomizer : Mod
    {
        public override string ModID => "UncleCustomizer"; // The mod's ID. Try making it as unique as possible, to avoid conflitcting IDs.
        public override string ModName => "Uncle Customizer"; // The mod's name. This is shown in the mods list. Does not need to be unique.
        public override string ModAuthor => "Leaxx"; // The mod's author (you). Also shown in the mods list.
        public override string ModDescription => "Allows you to modify the uncle's appearence!"; // The mod's description. This is also shown in the mods list, upon clicking on "More Info".
        public override string ModVersion => "1.0.0"; // The mod's version. Also shown in the mods list. If your mod is open-source on GitHub, make sure that you're using the same format as your release tags (for example, 1.0.0)
        public override string GitHubLink => "https://github.com/theLeaxx/UncleCustomizer/"; // If your mod is open-source on GitHub, you can link it here to allow for automatic update-checking in-game. It compares the current ModVersion with the tag of the latest release (ex. 1.0.0 compared with 1.0.1)
        public override WhenToInit WhenToInit => WhenToInit.InGame; // When should the mod's OnEnable/Awake/Start/Update functions be called?

        public override bool UseAssets => true; // Does your mod use custom assetbundles or textures?

        private readonly UncleHelper uncleHelper = UncleHelper.Instance;

        public override void SettingsDeclaration() // Declare all of your per-user settings here
        {
            base.SettingsDeclaration();

            InstantiateSettings();

            AddToggle("UncleHat", "Toggle Uncle's hat", true);
            AddToggle("UncleTexture", "Toggle custom color for Uncle's jacket", false);
            AddToggle("UncleMaterial", "Toggle funky jacket look", false);
        }

        public override void Start() // Default Unity Start() function
        {
            base.Start();

            if (!uncleHelper.UncleEnabled) return;

            var uncle = uncleHelper.Uncle.gameObject;

            if (GetToggleValue("UncleHat") == false)
            {
                uncle.transform.Find("Dantes_Hair_001").gameObject.SetActive(false);
            }

            if (GetToggleValue("UncleTexture") == true)
            {
                var texture = PNGToTexture("UncleJacket");

                if (GetToggleValue("UncleMaterial") == true)
                {
                    uncle.transform.Find("Dantes_Body_001").GetComponent<SkinnedMeshRenderer>().materials[0].mainTexture = texture;
                    return;
                }

                var originalMat = uncle.transform.Find("Dantes_Body_001").GetComponent<SkinnedMeshRenderer>().materials[1];

                var normalMat = new Material(Shader.Find("Legacy Shaders/Diffuse"))
                {
                    color = originalMat.color,
                    mainTexture = texture
                };

                Material[] matArray = new Material[2];
                matArray[0] = normalMat;
                matArray[1] = originalMat;

                uncle.transform.Find("Dantes_Body_001").GetComponent<SkinnedMeshRenderer>().materials = matArray;
            }
        }  
    }
}
