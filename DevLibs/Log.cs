using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Log
{
    //private static string logDirPath = Environment.CurrentDirectory+"\\log";
    //private static string fileName = "log_{0}.txt";

    private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    public static void d(string msg, Exception e = null)
    {
        if (e == null)
            logger.Debug(msg);
        else
            logger.Debug($"{msg}->{e}");
    }

    public static void i(string msg, Exception e = null)
    {
        if (e == null)
            logger.Info(msg);
        else
            logger.Info($"{msg}->{e}");
    }

    public static void e(string msg, Exception e = null)
    {
        if (e == null)
        {
            logger.Error(msg);
        }
        else
        {
            logger.Error($"{msg}->{e}");
        }
    }
}

