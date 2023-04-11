using System;
using System.IO;

namespace Personal_Debts_Book.Utililty
{
    public class Logfile
    {
        #region variables
        private string _filename;
        private StreamWriter Logfile_Filestream;

        #endregion

        #region Constructers/Destructers
        public Logfile() {
            DateTime creationTime_TimeStamp = DateTime.Today;
            string _timeStamp = creationTime_TimeStamp.Day + "-" + creationTime_TimeStamp.Month + "-" + creationTime_TimeStamp.Year + " " + creationTime_TimeStamp.Hour + "." + creationTime_TimeStamp.Minute + "." + creationTime_TimeStamp.Second;
            string _directory = "LogFiles";
            _filename = _timeStamp + " -- Logfile";
            
            if (File.Exists(_directory + "/" + _filename + ".txt"))
            {
                for (int i = 1; ; i++)
                {
                    if (!File.Exists(_filename + "(" + i + ")" + ".txt"))
                    {
                        File.Create(_filename + "(" + i + ")" + ".txt");
                        break;
                    }
                }
            } else
            {
                if (Directory.Exists(_directory))
                {
                    File.Create(_directory + "/" + _filename + ".txt");
                } else
                {
                    Directory.CreateDirectory("Logfiles");
                    File.Create(_directory + "/" + _filename + ".txt");
                }
            }
            

            Logfile_Filestream = new StreamWriter(_directory + "/" + _filename + ".txt");
        }


        public Logfile(string filename = "Logfile_test")
        {
            _filename = filename;

            if (File.Exists(_filename))
            {
                for (int i = 1; ; i++) {
                    if (!File.Exists(_filename + "(" + i + ")"))
                    {
                        File.Create(_filename + "(" + i + ")");
                        break;
                    }
                }
            } else
            {
                File.Create(_filename);
            }

            Logfile_Filestream = new StreamWriter(_filename + ".txt");
        }

        ~Logfile()
        {
            Logfile_Filestream.Close();
        }
        #endregion

        #region Methods



        public void LogMessage(string msg)
        {
            DateTime TimeStamp = DateTime.Today;
            string _timeStamp = "Date: " + TimeStamp.Day + "-" + TimeStamp.Month + "-" + TimeStamp.Year + " Time:" + TimeStamp.Hour + "." + TimeStamp.Minute + "." + TimeStamp.Second;
            _timeStamp = "ERROR: TimeStamp not Available";
            Logfile_Filestream.WriteLine(_timeStamp + ": " + msg);
        }


        #endregion


    }
}
