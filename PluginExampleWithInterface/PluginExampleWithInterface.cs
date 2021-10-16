﻿using Newtonsoft.Json;
using PluginExampleWithInterface.Views;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualMeetingPluginInterface;

namespace PluginExampleWithInterface
{
    internal class PluginExampleWithInterface : IPlugin
    {
        public Dictionary<string, Func<string>> PlaceHolders = new Dictionary<string, Func<string>>();

        public Dictionary<string, Func<object, dynamic>> ControlList = new Dictionary<string, Func<object, dynamic>>();



        public Dictionary<string, Func<string>> GetPlaceHolder()
        {
            PlaceHolders.Add("PLUGININTERFACE", Main);
            return PlaceHolders;
        }
        public Dictionary<string, Func<object,dynamic>> Interfaces()
        {
            ControlList.Add("Example config", MethodControlConfig);
            return ControlList;
        }
        public UserControl MethodControlConfig(dynamic data)
        {
            if (data != null)
            {
                return new ControlConfig(new Config(Convert.ToString(data["TextBoxValue"])));
            }
            else
            {
                return new ControlConfig(null);
            }
        }
        private string Main()
        {
            return $"Eita preula {this.GetName()} rodou ! {Globals._Config.TextBoxValue}";
        }

        public void loadConfigData(dynamic data)
        {  
            Globals.saveConfig(new Config(Convert.ToString(data["TextBoxValue"])));
        }

      

        public string getConfigData()
        {
            return JsonConvert.SerializeObject(Globals._Config, Formatting.Indented);
        }

        public string Authors()
        {
            return "brutalzinn";
        }

        public string Contact()
        {
            return "robertinho.net";
        }

        public string Description()
        {
            return "This plugin is a example of a plugin with interface";
        }

        public string GetName()
        {
            return "PluginExampleWithInterface";
        }

      
    }
}
