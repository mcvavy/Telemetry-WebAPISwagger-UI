using System;
using System.Collections.Generic;
using System.IO;
using MANOR.Core.Entities;
using Newtonsoft.Json;

namespace MANOR.Infrastructure.Utilities
{
    public interface IReadData
    {
        List<Telemetry> ReadAllData();
    }



    public class ReadData : IReadData
    {
        public List<Telemetry> ReadAllData()
        {
            string path = String.Empty;
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\Data\\telemetry.json")))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\Data\\telemetry.json");
            }
            else
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\telemetry.json");
            }

            try
            {
                using (var r = new StreamReader(path))
                {
                    return JsonConvert.DeserializeObject<List<Telemetry>>(r.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                var mess = ex.InnerException.Message;

                return null;
            }
        }
    }
}
