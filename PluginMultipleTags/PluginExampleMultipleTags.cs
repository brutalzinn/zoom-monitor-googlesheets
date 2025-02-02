﻿using System;
using System.Collections.Generic;
using VisualMeetingPluginInterface;

namespace PluginMultipleTags
{
    internal class PluginExampleMultipleTags : IPlugin
    {

        Dictionary<string, Func<string>> PlaceHolders = new Dictionary<string, Func<string>>();
        public Dictionary<string, Func<string>> GetMultipleHolder()
        {
            PlaceHolders.Add("TAG1", TAGONE);
            PlaceHolders.Add("TAG2", TAGTWO);
            PlaceHolders.Add("TAG3", TAGTHREE);
            return PlaceHolders;
        }

        private string TAGONE()
        {
            return "This is a tag one with same plugin";
        }

        private string TAGTWO()
        {
            return "This is a tag two with same plugin";
        }
        private string TAGTHREE()
        {
            return $"This is a tag thread with same plugin {new Random().Next(0, 42).ToString()}";
        }
        public string GetName() => "Multiple TAGS Plugin";


        public string GetPlaceHolder() => throw new NotImplementedException();



        public string Main() => throw new NotImplementedException();

        public string Description()
        {
            return "Esse é um plugin com múltiplas tags";
        }

        public string Authors()
        {
            return "brutalzinn";
        }

        public string Contact()
        {
            return "robertinho.net";
        }
    }
}
