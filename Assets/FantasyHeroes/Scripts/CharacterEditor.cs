using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FantasyHeroes.Scripts
{
    /// <summary>
    /// Defines editor's behaviour
    /// </summary>
    public class CharacterEditor : MonoBehaviour
    {
        public SpriteCollection SpriteCollection;
        public AnimationManager AnimationManager;
        public Character Dummy;

        [Header("UI")]
        public GameObject Editor;
        public GameObject CommonPalette;
        public GameObject SkinPalette;
        public Dropdown HeadDropdown;
        public Dropdown EarsDropdown;
        public Dropdown HairDropdown;
        public Dropdown EyebrowsDropdown;
        public Dropdown EyesDropdown;
        public Dropdown MouthDropdown;
        public Dropdown BeardDropdown;
        public Dropdown BodyDropdown;
        public Dropdown HelmetDropdown;
        public Dropdown ArmorDropdown;
        public Dropdown CloakDropdown;
        public Dropdown MeleeWeapon1HDropdown;
        public Dropdown MeleeWeapon1HSecondaryDropdown;
        public Dropdown MeleeWeapon2HDropdown;
        public Dropdown ShieldDropdown;
        public Dropdown BowDropdown;

        /// <summary>
        /// Called automatically on app start
        /// </summary>
        public void Start()
        {
            Refresh();
            InitializeDropdown(HeadDropdown, SpriteCollection.Head, Dummy.Head, texture => Dummy.Head = texture);
            InitializeDropdown(EarsDropdown, SpriteCollection.Ears, Dummy.Ears, texture => Dummy.Ears = texture);
            InitializeDropdown(HairDropdown, SpriteCollection.Hair, Dummy.Hair, texture => Dummy.Hair = texture);
            InitializeDropdown(EyebrowsDropdown, SpriteCollection.Eyebrows, Dummy.Eyebrows, texture => Dummy.Eyebrows = texture);
            InitializeDropdown(EyesDropdown, SpriteCollection.Eyes, Dummy.Eyes, texture => Dummy.Eyes = texture);
            InitializeDropdown(MouthDropdown, SpriteCollection.Mouth, Dummy.Mouth, texture => Dummy.Mouth = texture);
            InitializeDropdown(BeardDropdown, SpriteCollection.Beard, Dummy.Beard, texture => Dummy.Beard = texture);
            InitializeDropdown(BodyDropdown, SpriteCollection.Body, Dummy.Body, texture => Dummy.Body = texture);
            InitializeDropdown(HelmetDropdown, SpriteCollection.Helmet, Dummy.Helmet, texture => Dummy.Helmet = texture);
            InitializeDropdown(ArmorDropdown, SpriteCollection.Armor, Dummy.Armor, texture => Dummy.Armor = texture);
            InitializeDropdown(CloakDropdown, SpriteCollection.Cloak, Dummy.Cloak, texture => Dummy.Cloak = texture);
            InitializeDropdown(MeleeWeapon1HDropdown, SpriteCollection.MeleeWeapon1H, Dummy.PrimaryMeleeWeapon, texture => { Dummy.PrimaryMeleeWeapon = texture; if (Dummy.WeaponType != WeaponType.MeleeTween) Dummy.WeaponType = WeaponType.Melee1H; AnimationManager.Reset(); });
            InitializeDropdown(MeleeWeapon1HSecondaryDropdown, SpriteCollection.MeleeWeapon1H, Dummy.SecondaryMeleeWeapon, texture => { Dummy.SecondaryMeleeWeapon = texture; ReturnMeleeWeapon1H(); Dummy.WeaponType = WeaponType.MeleeTween; AnimationManager.Reset(); });
            InitializeDropdown(MeleeWeapon2HDropdown, SpriteCollection.MeleeWeapon2H, Dummy.PrimaryMeleeWeapon, texture => { Dummy.PrimaryMeleeWeapon = texture; Dummy.WeaponType = WeaponType.Melee2H; AnimationManager.Reset(); });
            InitializeDropdown(ShieldDropdown, SpriteCollection.Shield, Dummy.Shield, texture => { Dummy.Shield = texture; ReturnMeleeWeapon1H(); Dummy.WeaponType = WeaponType.Melee1H; AnimationManager.Reset(); });
            InitializeDropdown(BowDropdown, SpriteCollection.Bow, Dummy.Bow, texture => { Dummy.Bow = texture; Dummy.WeaponType = WeaponType.Bow; AnimationManager.Reset(); });
        }

        private string _target;

        /// <summary>
        /// Open palette to change sprite color
        /// </summary>
        /// <param name="target">Pass one of the following values: Head, Ears, Body, Hair, Eyes, Mouth</param>
        public void OpenPalette(string target)
        {
            _target = target;
            Editor.SetActive(false);

            switch (_target)
            {
                case "Head":
                case "Ears":
                case "Body": SkinPalette.SetActive(true); break;
                case "Hair":
                case "Eyebrows":
                case "Eyes":
                case "Mouth":
                case "Beard": CommonPalette.SetActive(true); break;
            }
        }

        /// <summary>
        /// Close palette
        /// </summary>
        public void ClosePalette()
        {
            CommonPalette.SetActive(false);
            SkinPalette.SetActive(false);
            Editor.SetActive(true);
        }

        /// <summary>
        /// Save character to prefab
        /// </summary>
        public void Save()
        {
            #if UNITY_EDITOR

            for (var i = 0; i < 100; i++)
            {
                var path = string.Format("Assets/SavedHero#{0}.prefab", i);

                if (!System.IO.File.Exists(path))
                {
                    UnityEditor.PrefabUtility.CreatePrefab(path, Dummy.gameObject);
                    Debug.LogFormat("<color=green>Saved as {0}</color>", path);
                    return;
                }
            }

            #endif
        }

        /// <summary>
        /// Flip character by X-axis
        /// </summary>
        public void Flip()
        {
            var scale = Dummy.transform.localScale;

            scale.x *= -1;
            Dummy.transform.localScale = scale;
        }

        /// <summary>
        /// Visit publisher on the Asset Store
        /// </summary>
        public void More()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:11086");
        }

        /// <summary>
        /// Pick color and apply to sprite
        /// </summary>
        /// <param name="color"></param>
        public void PickColor(Color color)
        {
            switch (_target)
            {
                case "Head": Dummy.HeadRenderer.color = color; break;
                case "Ears": Dummy.EarsRenderer.color = color; break;
                case "Hair": Dummy.HairRenderer.color = color; break;
                case "Eyebrows": Dummy.EyebrowsRenderer.color = color; break;
                case "Eyes": Dummy.EyesRenderer.color = color; break;
                case "Mouth": Dummy.MouthRenderer.color = color; break;
                case "Beard": Dummy.BeardRenderer.color = color; break;
                case "Body": foreach (var part in Dummy.BodyRenderers) part.color = color; break;
            }
        }

        private void InitializeDropdown(Dropdown dropdown, List<Texture2D> sprites, Texture2D texture, Action<Texture2D> callback)
        {
            dropdown.options = sprites.Select(i => new Dropdown.OptionData(GetSpriteName(i))).ToList();
            dropdown.options.Add(new Dropdown.OptionData("Empty"));
            dropdown.value = sprites.Contains(texture) ? sprites.IndexOf(texture) : sprites.Count;
            dropdown.onValueChanged.AddListener(index => { callback(index == sprites.Count ? null : sprites[index]); Refresh(); });
        }

        private void ReturnMeleeWeapon1H()
        {
            var index = MeleeWeapon1HDropdown.value;

            Dummy.PrimaryMeleeWeapon = index == SpriteCollection.MeleeWeapon1H.Count ? null : SpriteCollection.MeleeWeapon1H[index];
        }

        private void Refresh()
        {
            if (SpriteCollection.Hair.Contains(Dummy.Hair) && Dummy.Helmet != null)
            {
                Dummy.Hair = SpriteCollection.HairShort.SingleOrDefault(i => i.name == Dummy.Hair.name);
            }
            else if(SpriteCollection.HairShort.Contains(Dummy.Hair) && Dummy.Helmet == null)
            {
                Dummy.Hair = SpriteCollection.Hair.SingleOrDefault(i => i.name == Dummy.Hair.name);
            }

            Dummy.Initialize();
        }

        private static string GetSpriteName(Texture2D texture)
        {
            if (texture == null) return "Empty";
            if (texture.name.All(c => char.IsUpper(c))) return texture.name;

            return Regex.Replace(Regex.Replace(texture.name, "[A-Z]", " $0"), "([a-z])([1-9])", "$1 $2").Trim();
        }
    }
}