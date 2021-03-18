using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using System.IO;


namespace MonitorTask
{
    class Program
    {
       

        static void Main(string[] args)

        {
            Console.Title = "Server Monitor";
            string path = @"C:\Logs\Server_Monitor.csv";
            Console.WriteLine("Generando Server_Monitor.csv\nEspere 10 segundos hasta que se complete.\nSi tiene el documento Server_Monitor.csv porfavor cerrarlo\nEl archivo se generara en "+path);
            Console.Beep();
            

            #region Definitions 
            //Ram pc
            PerformanceCounter pcRAM = new PerformanceCounter();
            pcRAM.CategoryName = "Memory";
            //pcRAM.CounterName = "% Committed Bytes in Use";
            pcRAM.CounterName = "Available Mbytes";
            pcRAM.NextValue();

            //cores pcs
            //multi core
            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }

            PerformanceCounter[] pc = new PerformanceCounter[coreCount];

            for (int i = 0; i < coreCount; i++)
            {
                pc[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());

                pc[i].NextValue();


            }
            //temp
            //PerformanceCounter pcTemp = new PerformanceCounter("Thermal Zone Information", "Temperature", @"\_TZ.TZ00");
            //pcTemp.NextValue();

            //general cpu
            PerformanceCounter pcCPU = new PerformanceCounter();
            pcCPU.CategoryName = "Processor";
            pcCPU.CounterName = "% Processor Time";
            pcCPU.InstanceName = "_Total";
            pcCPU.NextValue();
            //Drives
            DriveInfo[] drives = DriveInfo.GetDrives();

            string[] driveInfo = new string[drives.Length];
            int index = 0;
            #endregion
            System.Threading.Thread.Sleep(5000); //Delay de 5 segundos
            #region Strings
            DateTime date = DateTime.Now;

            string datestring = $"Fecha y hora; { date.ToString() } ";
             
            
            string cpuUsageText = $"Uso de CPU total; {(int)pcCPU.NextValue()} % ";

            string ramUsageText = $"RAM disponible; {(int)pcRAM.NextValue()} MB ";

            //string cpuTemperature = $"Temperatura; {(int)(pcTemp.NextValue() - 273.15f)} \u00B0C";

            string[] cpuCoresUsagesText = new string[coreCount];

            for (int i = 0; i < coreCount; i++)
            {                
                cpuCoresUsagesText[i] = $"Uso de CPU Core({i}); {(int)pc[i].NextValue()} %";
            }
                   
            foreach (DriveInfo drive in drives)
            {
                
                driveInfo[index] = ($"Dispositivo {drive.Name}; Espacio Disponible " +
                    $"{((drive.AvailableFreeSpace/1024)/1024)/1024} GB de {((drive.TotalSize/1024)/1024)/1024} GB");
               
                index++;

            }

            #endregion
            
            System.Threading.Thread.Sleep(5000); //Delay de 5 segundos
            
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write($"{datestring}\n{cpuUsageText}\n{string.Join("\n", cpuCoresUsagesText)}\n{ramUsageText}\nAlmacenamiento\n{string.Join("\n", driveInfo)}");
                //sw.WriteLine("");
                //sw.WriteLine("");
            }

        }
    }
}
