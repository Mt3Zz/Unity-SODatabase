using System.Runtime.CompilerServices;

// テストからInternal Classへアクセスすることを許可
[assembly: InternalsVisibleTo("SODatabase.Tests.PlayMode")]

// エディターからInternal Classへアクセスすることを許可
[assembly: InternalsVisibleTo("SODatabase.Editor")]

// エディターテストからInternal Classへアクセスすることを許可
[assembly: InternalsVisibleTo("SODatabase.Editor.Tests")]
