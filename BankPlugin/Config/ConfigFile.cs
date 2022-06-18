using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankPlugin.Config
{
    public class ConfigFile
    {
        public string StoragePath = "default";
        public Storage StorageType = Storage.Json;
        public HistoryType HistoryType = HistoryType.CSV;
    }
}
