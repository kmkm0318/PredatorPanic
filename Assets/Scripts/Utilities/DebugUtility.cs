using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 디버그 유틸리티
/// </summary>
public static class DebugUtility
{
    /// <summary>
    /// 디버그 로그 출력 확장 함수
    /// 파일 이름, 라인 번호, 멤버 이름 포함
    /// </summary>
    public static void Log(this string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
    {
        string fileName = System.IO.Path.GetFileName(filePath);
        Debug.Log($"[{fileName}:{lineNumber}:{memberName}] {message}");
    }

    /// <summary>
    /// 디버그 경고 로그 출력 확장 함수
    /// /// 파일 이름, 라인 번호, 멤버 이름 포함
    /// </summary>
    public static void LogWarning(this string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
    {
        string fileName = System.IO.Path.GetFileName(filePath);
        Debug.LogWarning($"[{fileName}:{lineNumber}:{memberName}] {message}");
    }

    /// <summary>
    /// 디버그 에러 로그 출력 확장 함수
    /// /// 파일 이름, 라인 번호, 멤버 이름 포함
    /// </summary>
    public static void LogError(this string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
    {
        string fileName = System.IO.Path.GetFileName(filePath);
        Debug.LogError($"[{fileName}:{lineNumber}:{memberName}] {message}");
    }
}