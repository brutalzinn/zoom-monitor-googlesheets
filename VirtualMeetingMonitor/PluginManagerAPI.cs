﻿
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using VirtualMeetingMonitor.ApiPluginManager.models;

namespace VirtualMeetingMonitor
{
    public class PluginManagerAPI
    {
        private readonly string url = "http://localhost:8000"; //esp 8266 fixed ip


        public bool addUser(UserModel body) => callAuthUser(body) == HttpStatusCode.OK;

        public bool addPackage(FileModel body) => callAddPackage(body) == HttpStatusCode.OK;
        public GenericFiles getPackages(int page = 0, int size = 3) =>  CallPackageList(page,size);
        private HttpStatusCode callAuthUser(UserModel body)
        {
            RestClient client = new RestClient(url);
            const string api = "/login";
            var request = new RestRequest(api, Method.POST);
            dynamic json = new ExpandoObject();
            json.email = body.Email;
            json.password = body.Password;
            request.AddJsonBody(json);
            var query = client.Execute(request);
            string tokenOriginal = JsonConvert.DeserializeObject<UserModel>(query.Content).Token;
            string token = JsonConvert.DeserializeObject<UserModel>(query.Content).Token?.Split('.')[1];
            if (token != null) {
            var user_info = JsonConvert.DeserializeObject<UserModel>(Core.DecodeBase64(token));
            Core.UserAccount.Id = user_info.Id;
            Core.UserAccount.Token = tokenOriginal;
            Core.UserAccount.Rank = user_info.Rank;
            Core.UserAccount.Name = user_info.Name;
            Core.UserAccount.Email = user_info.Email;
            Core.UserAccount.onLogin();
           return query.StatusCode;
            }
            else
            {
                Core.UserAccount.Error();
                return HttpStatusCode.BadRequest;
            }
        }
      
        private HttpStatusCode callAddPackage(FileModel body)
        {
            RestClient client = new RestClient(url);
            var request = new RestRequest("/files/upload", Method.POST)
            {
                AlwaysMultipartFormData = true
            };
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MSwibmFtZSI6IlJvb3QgdXNlciIsImVtYWlsIjoicm9vdEByb290LmNvbSIsInJhbmsiOjEsImlhdCI6MTYzNDYwNDU2NCwiZXhwIjoxNjM0NjkwOTY0fQ.5ifac6yOSrhfmlr1BPc0awML7hwZhvBrZSnjyyvI7U4";
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddFile("plugin",body.Filename);
            request.AddParameter("name", body.Name);
            request.AddParameter("description", body.Description);
            request.AddParameter("repo", body.Repo);
            request.AddParameter("version", Convert.ToString(body.Version.version));
            request.AddParameter("crc", Convert.ToString(body.Version.crc));
            request.AddParameter("sha", Convert.ToString(body.Version.sha));
            return client.Execute(request).StatusCode;
        }
        private GenericFiles CallPackageList(int page, int size)
        {
            RestClient client = new RestClient(url);      
            var request = new RestRequest($"/files?page={page}&size={size}", Method.GET);
            request.AddParameter("page", page, ParameterType.UrlSegment);
            request.AddParameter("size", size, ParameterType.UrlSegment);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);
            GenericFiles configModel = JsonConvert.DeserializeObject<GenericFiles>(queryResult.Content);
            return configModel;
        }
    }
}