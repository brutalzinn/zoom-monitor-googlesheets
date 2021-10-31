﻿using PluginInterface;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Windows.Forms;
using PluginServiceExample.Views;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace PluginServiceExample
{
    internal class PluginService : IPlugin, IService, IConfig, IVisual
    {
        public Dictionary<string, Func<object, dynamic>> ControlList = new Dictionary<string, Func<object, dynamic>>();

        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Virtual Meeting";
        static readonly string GoogleSecret = "client_secret.json";
        private static string SpreadsheetId = "";
        private static string sheet = "";
        static SheetsService service;


        public string Authors()
        {
            return "brutalzinn";
        }

        public string Contact()
        {
            return "brutalzinn";
        }

        public string Description()
        {
            return "Google Sheets connetor - Oficial";
        }

        public void Executor(List<object> values)
        {
            throw new NotImplementedException();
        }
        public Dictionary<string, Func<object, dynamic>> Interfaces()
        {
            ControlList.Add(Name(), MethodControlConfig);
            return ControlList;
        }
        public UserControl MethodControlConfig(dynamic data)
        {
            if (data != null)
            {
                Config configModel = JsonConvert.DeserializeObject<Config>(Convert.ToString(data));
                return new ServiceView(configModel);
            }
            else
            {
                return new ServiceView(null);
            }
        }
        public void loadConfigData(dynamic data)
        {
            Config configModel = JsonConvert.DeserializeObject<Config>(Convert.ToString(data));
            Globals.saveConfig(configModel);
        }

        public string getConfigData()
        {
            return JsonConvert.SerializeObject(Globals._Config, Formatting.Indented);
        }

        void CreateEntry(List<object> oblist)
        {

            var range = $"{sheet}!A:C";
            var valueRange = new ValueRange();   
            valueRange.Values = new List<IList<object>> { oblist };

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
        }

        public string Name()
        {
            return "GoogleAPI service example";
       }

   
    }
}
