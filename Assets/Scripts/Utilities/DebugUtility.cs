using System.Runtime.CompilerServices;

/// <summary>
/// 디버그 유틸리티
/// </summary>
public static class DebugUtility
{
    public static void Log(this string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
    {
        string fileName = System.IO.Path.GetFileName(filePath);
        UnityEngine.Debug.Log($"[{fileName}:{lineNumber}:{memberName}] {message}");
    }
}