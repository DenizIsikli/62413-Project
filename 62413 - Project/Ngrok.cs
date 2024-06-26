﻿using System;
using System.Diagnostics;
using System.IO;

namespace _62413___Project
{
    class Ngrok
    {
        private string ngrokPath;
        private string projectRoot;

        /// <summary>
        /// Ngrok constructor
        /// </summary>
        public Ngrok()
        {
            string projectRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\");
            this.projectRoot = Path.GetFullPath(projectRoot);
            this.ngrokPath = this.projectRoot + "ngrok.exe";
            StopAllTunnels();
            // Delay to make sure all ngrok processes are killed
            System.Threading.Thread.Sleep(2000);
        }

        public void StopAllTunnels()
        {
            foreach (var proc in Process.GetProcessesByName("ngrok"))
            {
                proc.Kill();
            }
        }

        /// <summary>
        /// Starts an ngrok tunnel and returns the URL
        /// </summary>
        /// <returns></returns>
        public string StartNgrokTunnel()
        {

            var startInfo = new ProcessStartInfo()
            {
                FileName = this.ngrokPath,
                Arguments = "start --config \"ngrok.yml\" my_tunnel --log=stdout",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WorkingDirectory = this.projectRoot
            };

            Process process = new Process { StartInfo = startInfo };
            process.Start();

            string tunnelUrl = null;

            // Read the ngrok output
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                Console.WriteLine(line);

                // Check if the line contains the URL
                if (line.Contains("url=tcp://"))
                {
                    int startIndex = line.IndexOf("url=") + 4;
                    int endIndex = line.IndexOf(' ', startIndex);
                    endIndex = endIndex == -1 ? line.Length : endIndex;
                    tunnelUrl = line.Substring(startIndex, endIndex - startIndex);
                    break;
                }
            }

            //process.WaitForExit();
            return tunnelUrl;
        }
    }
}
