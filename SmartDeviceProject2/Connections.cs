﻿using System;
using System.IO;

namespace SmartDeviceProject2
{
    class Connections
    {
        private static string[] confix;
        private static string path;
        public static string[] Confix()
        {
            string result = "";
            try
            {
                path = Directory.GetCurrentDirectory().ToString() + @"\config.txt";
            }
            catch
            {
                path = @"\Storage Card\Debug\config.txt";
            }
            using (FileStream filestream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(filestream))
                {
                    while (sr.Peek() >= 0)
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            confix = result.Split(new Char[] { ';' });
            return confix;
        }
    }
}
