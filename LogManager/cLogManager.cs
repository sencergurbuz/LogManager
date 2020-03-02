using NetworkManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogManager
{
    public class cLogManager
    {
        private long autoId = 0;

        private List<cLogItem> _LogStack;

        private cNetworkManager _networkManager;

        private StreamWriter sw;

        private bool _isGetFromNetwork = false;

        private bool _isSaveToFile = false;

        private string logFileName = "";

        private int MaximumLogCount = 5;

        public cNetworkManager networkManager
        {
            get 
            {
                return _networkManager; 
            }
            set {                if (value == null)
                {
                    value = new cNetworkManager();
                }

                _networkManager = value;
                _networkManager.onRecieved += _networkManager_onRecieved;
            } 
        }

        public bool isGetFromNetwork
        {
            get
            {
                return _isGetFromNetwork;
            }
            set
            {
                _isGetFromNetwork = value;
                _networkManager.Open();
            }
        }

        public bool isSaveToFile
        {
            get
            {
                sw.Close();
                return _isSaveToFile;
            }
            set
            {
                _isSaveToFile = value;
                
                if (Directory.Exists(FolderPath) == false)
                {
                    Directory.CreateDirectory(FolderPath);
                }

                ManageWorkspace();
            }
        }

        private void ManageWorkspace()
        {



            logFileName = "Logs_" + DateTime.Now.ToString("YYMMDDHHmmss");
            sw = new StreamWriter(FolderPath + logFileName);
        }

        public List<cLogItem> LogStack {
            get
            {
                return _LogStack;
            }
            set
            {
                if (value == null)
                {
                    value = new List<cLogItem> { };
                }

                _LogStack = value;
            }
        }

        public string FolderPath { get; set; }

        public cLogManager() 
        {
            _LogStack = new List<cLogItem> { };
        }

        public void Add(LogLevel level, string summary)
        {
            Add(level, summary, "");
        }

        public void Add(LogLevel level, string summary, string description)
        {
            cLogItem item = new cLogItem();
            item.enLogLevel = level;
            item.Summary = summary;
            item.Description = description;
            Add(item);
        }
        
        private void Add(cLogItem log)
        {            
            LogStack.Add(log);
           
            if (_isSaveToFile)
            {
                sw.WriteLine(ToXML(log));
            }
        }
        public string ToXML(cLogItem item)
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stringwriter, item);
                return stringwriter.ToString();
            }
        }

        public static cLogItem LoadFromXMLString(string xmlText)
        {
            using (var stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(cLogItem));
                return serializer.Deserialize(stringReader) as cLogItem;
            }
        }



        private void _networkManager_onRecieved(byte[] bytes)
        {
            Add(cLogItem.Desserialize(bytes));
        }
    }
}
