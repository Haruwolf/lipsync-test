using UnityEngine;
using System;
using System.IO;

public static class Logger
{
    private static string LogFilePath;

    static Logger()
    {
        string logFolder = Path.Combine(Application.persistentDataPath, "Logs");
        if (!Directory.Exists(logFolder))
            Directory.CreateDirectory(logFolder);

        LogFilePath = Path.Combine(logFolder, "log.txt");
    }

    public static void Log(string message)
    {
        WriteLog("LOG", message);
    }

    public static void LogError(string message)
    {
        WriteLog("ERROR", message);
    }

    private static void WriteLog(string logType, string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now} - [{logType}] - {message}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao escrever no arquivo de log: " + e.Message);
        }
    }
}
