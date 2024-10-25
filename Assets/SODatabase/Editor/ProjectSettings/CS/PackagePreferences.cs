using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SODatabase.Editor
{
    using ObjectStorage = DataObject.ObjectStorage;


    [FilePath("ProjectSettings/SODatabase.asset",
        FilePathAttribute.Location.ProjectFolder)]
    internal class PackagePreferences : ScriptableSingleton<PackagePreferences>
    {
        public FolderPreferences FolderPreferences => _folderPreferences;
        [SerializeField]
        private FolderPreferences _folderPreferences = new();


        public IList<ObjectStorage> DistinctStorages => _storages.Distinct().ToList();
        [SerializeField]
        private List<ObjectStorage> _storages = new();


        internal void Save()
        {
            Save(true);
        }
    }
    [Serializable]
    internal class FolderPreferences
    {
        internal string RootFolder => _rootFolder;
        internal string InitialRootFolder => _initialRootFolder;
        internal string ItemFolder     => RootFolder + "/Records";
        internal string TemplateFolder => RootFolder + "/Templates";
        internal string StorageFolder  => RootFolder + "/Storages";
        internal string TypeFolder     => RootFolder + "/Types";

        [SerializeField]
        private string _rootFolder = _initialRootFolder;
        [SerializeField]
        private const string _initialRootFolder = "Assets/SODatabase";



        // そのまま呼び出すと、途中にディレクトリが存在しない場合エラーになる。
        /// <summary>
        /// AssetDatabase.CreateFolderで再帰的にディレクトリを作成する。
        /// </summary>
        /// <param name="dirPath">"Assets/"からはじまるディレクトリパス</param>
        internal void CreateDirectoryRecursively(string dirPath)
        {
            // Assetsから始まらない場合は処理をスキップ
            if (!dirPath.StartsWith("Assets/"))
            {
                var msg = ""
                    + "ディレクトリパスが\"Assets/\"からはじまりません。\n"
                    + "ディレクトリ作成処理をスキップしました。\n"
                    + $"dirPath : {dirPath}";
                Debug.Log(msg);
                return;
            }

            // 区切り文字"/"で分解
            var dirs = dirPath.Split("/");
            var path = dirs[0];

            // Assetsの部分をスキップ
            foreach (var dir in dirs.Skip(1))
            {
                // ディレクトリが存在するか確認
                if (!AssetDatabase.IsValidFolder(path + "/" + dir))
                {
                    AssetDatabase.CreateFolder(path, dir);
                }
                path += "/" + dir;
            }
        }
    }
}
