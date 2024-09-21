using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SODatabase.Editor
{
    public class PackagePreferencesProvider : SettingsProvider
    {
        private const string _settingFolder = "Project/";
        private const string _settingName = "SO Database";

        // 設定のパス
        // 先頭をProjectにすることでProject Settingsに追加されるようになる
        // Preferencesに追加する場合は、先頭をPreferencesにする
        private const string _settingPath = _settingFolder + _settingName;


        /// <summary>
        /// カスタムパッケージ "Observable Turn-Based Combat" のカスタム設定画面を
        /// Project Settingsに追加する
        /// </summary>
        // このメソッドによってProject Settingsに項目が増える
        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            // SettingsProviderを返すことで設定項目を追加する
            return new PackagePreferencesProvider(_settingPath, SettingsScope.Project, null);
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">設定のパス</param>
        /// <param name="scopes">項目をPreferencesとProject Settingsのどちらに追加するか</param>
        /// <param name="keywords">検索時にこの設定を取得するためののキーワード</param>
        public PackagePreferencesProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
        {
        }


        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            EditorGUI.BeginChangeCheck();


            var preferences = PackagePreferences.instance;

            // instanceを編集可能にする
            // NotEditableが含まれていると、デフォルトインスペクターや
            // PropertyFieldを使ったときに入力ができない状態になる（なぜ？）
            preferences.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;


            // WindowPreferenceのデフォルトインスペクターを作成
            var inspector = new InspectorElement(preferences);
            rootElement.Add(inspector);
        }
        public override void OnDeactivate()
        {
            // 変更があったら保存
            if (EditorGUI.EndChangeCheck())
            {
                PackagePreferences.instance.Save();
            }
        }
    }
}
