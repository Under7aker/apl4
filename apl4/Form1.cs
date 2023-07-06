using apl4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using Microsoft.Win32;



namespace apl4
{

    public partial class Form1 : Form
    {
        //private Timer timer;
        private Timer alarmTimer;

        private int docFilesCount;
   
        private int txtFilesCount;
  

        

        public Form1()
        {
            InitializeComponent();
 

            alarmTimer = new Timer();
            alarmTimer.Interval = 10000; // Setăm intervalul de 30 de secunde
            alarmTimer.Tick += AlarmTimer;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.Width = 0;
            this.Height = 0;
            this.Visible = false;
            this.Hide();

            string taskName = "2Monitorizare";
            TaskService ts = new TaskService();
            // TaskDefinition td = ts.NewTask();
            TaskDefinition td = TaskService.Instance.NewTask();
            td.RegistrationInfo.Author = "Task Scheduler Library";
            td.RegistrationInfo.Description = "hai ca se poate";
            td.Principal.RunLevel = TaskRunLevel.Highest;
           
            LogonTrigger logonTrigger = new LogonTrigger();
            logonTrigger.Delay = TimeSpan.FromMinutes(1); // Optional delay after logon
            td.Triggers.Add(logonTrigger);


            string executablePath = Path.Combine(Environment.CurrentDirectory, "apl4.exe");
           

            ExecAction execAction = new ExecAction(executablePath, null);


            td.Actions.Add(execAction);



            TaskService.Instance.RootFolder.RegisterTaskDefinition(taskName, td);
            alarmTimer.Start();

        }



        public static bool Alert1Exists()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\1.1"))
            {
                return key?.GetValue("1") != null;
            }
        }

        public static bool Alert2Exists()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\1.1"))
            {
                return key?.GetValue("2") != null;
            }


        }
        bool IsFileOfType(string filePath, params string[] extensions)
            {
                string fileExtension = Path.GetExtension(filePath);

                foreach (string extension in extensions)
                {
                    if (fileExtension.Equals(extension, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                return false;
            }

          
            void AlarmTimer(object sender, EventArgs e)
            {
        
            string folderPath = "D:\\Interes\\Fac. III\\Practica\\ceva";// Obținem calea folderului introdusă de utilizator
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly); // Obținem toate fișierele din folder
                txtFilesCount = files.Count(file => IsFileOfType(file, ".txt")); // Numărăm fișierele .txt
                docFilesCount = files.Count(file => IsFileOfType(file, ".doc", ".docx")); // Numărăm fișierele .doc și .docx
                        

                if (txtFilesCount > 3)
                {
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey("Software\\Wow6432Node\\1.1", true))
                    {
                        key.SetValue("1", "Alerta1");
                        MessageBox.Show("Alert 1:mai mult de 3 .txt files detectate.");
                        

                        
                    }
                }
                else if (Alert1Exists())
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\1.1", true))
                    {
                        key.DeleteValue("1");
                        MessageBox.Show("Alert 1 a fost stearsa");
                    }
                }
         


            }
            else
            {
                MessageBox.Show("Folderul specificat nu există!");

            }

            if (txtFilesCount < 3 & docFilesCount >= 2)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\1.1", true))
                {
                    key.SetValue("2", "Alerta2");
                    MessageBox.Show("Alert 2: 2 .doc files  sau mai multe sunt in folder.");

                    
                }

            }
            else if (Alert2Exists())
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\1.1", true))
                {
                    key.DeleteValue("2");
                    MessageBox.Show("Alert 2 nu mai este");
                }
            }

        }
           
        


        }
    }


