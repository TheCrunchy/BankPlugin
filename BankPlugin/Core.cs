using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch;
using Torch.API;
using Torch.API.Plugins;
using Torch.Managers;
using Torch.API.Managers;
using Torch.Session;
using Torch.API.Session;
using Torch.Managers.PatchManager;
using System.Reflection;
using Sandbox.Game.Entities.Cube;
using System.IO;
using Sandbox.Game.GameSystems;
using BankPlugin.Config;
using BankPlugin.BankServices;
using NLog;

namespace BankPlugin
{
    public class Core : TorchPluginBase
    {
        public static IBankService BankService { get; set; }

        public static Logger Log = LogManager.GetLogger("BankPlugin");

        public static ConfigFile config;

        public static FileUtils utils = new FileUtils();

        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            var sessionManager = Torch.Managers.GetManager<TorchSessionManager>();

            if (sessionManager != null)
            {
                sessionManager.SessionStateChanged += SessionChanged;
            }

            SetupConfig();
            InitBankService(config.StoragePath, config.StorageType);
            SetupFolders();
        }
        private void SetupConfig()
        {
            FileUtils utils = new FileUtils();

            if (File.Exists(StoragePath + "\\BankConfig.xml"))
            {
                config = utils.ReadFromXmlFile<ConfigFile>(StoragePath + "\\BankConfig.xml");
                utils.WriteToXmlFile<ConfigFile>(StoragePath + "\\BankConfig.xml", config, false);
            }
            else
            {
                config = new ConfigFile();
                utils.WriteToXmlFile<ConfigFile>(StoragePath + "\\BankConfig.xml", config, false);
            }

        }

        private void SetupFolders()
        {
            if (config.StoragePath == "default")
            {
                Directory.CreateDirectory("//BankPlugin");
                Directory.CreateDirectory("//BankPlugin//Data");
                Directory.CreateDirectory("//BankPlugin//Data//Json");
                Directory.CreateDirectory("//BankPlugin//Data//XML");
                Directory.CreateDirectory("//BankPlugin//Data//History");
            }
            else
            {
                config.StoragePath = config.StoragePath.Replace("//BankPlugin", "");
                Directory.CreateDirectory($"{config.StoragePath}//BankPlugin");
                Directory.CreateDirectory($"{config.StoragePath}//BankPlugin//Data");
                Directory.CreateDirectory($"{config.StoragePath}//BankPlugin//Data//Json");
                Directory.CreateDirectory($"{config.StoragePath}//BankPlugin//Data//XML");
                Directory.CreateDirectory($"{config.StoragePath}//BankPlugin//Data//History");
            }
        }

        public void InitBankService(string path, Storage storage)
        {
            switch (storage)
            {
                case Storage.Json:
                    BankService = new JsonBankService(path);
                    break;
                case Storage.XML:
                    BankService = new XMLBankService(path);
                    break;
            }
        }

        private void SessionChanged(ITorchSession session, TorchSessionState newState)
        {

        }
 


    }
}

