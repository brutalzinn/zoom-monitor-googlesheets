﻿using System;
using VisualMeetingPluginInterface;
using RestSharp;
using System.Collections.Generic;

namespace PluginExample
{
    internal class PluginExample : IPlugin
    {
        public string Description()
        {
            return "Esse é um plugin de exemplo.";
        }

        public string Authors()
        {
            return "brutalzinn";
        }

        public string Contact()
        {
            return "robertinho.net";
        }

        public Dictionary<string, Func<string>> GetMultipleHolder()
        {
            throw new NotImplementedException();
        }

        public string GetName() => "My plugin v1";


        public string GetPlaceHolder() => "PLUGINEXAMPLE";


        public string Main()
        {
            return ApiCall();
        }
        private string ApiCall()
        {
            RestClient client = new RestClient("https://baconipsum.com/api/?callback=?");

      
            var request = new RestRequest(Method.GET);

           
            return client.Execute(request).Content;
        }

        
    }
}
