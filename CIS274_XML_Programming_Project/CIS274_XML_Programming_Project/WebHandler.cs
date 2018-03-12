﻿/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System;
using System.IO;
using System.Net;

namespace CIS274_XML_Programming_Project
{
    class WebHandler
    {
        public string SavedResponse { get;set;}

        public HttpWebResponse SendRequest(string target, string method)
        {
            var request = WebRequest.Create(target);
            request.Method = method;
            return (HttpWebResponse)request.GetResponse();
        }

        public void GetResponse(HttpWebResponse response)
        {
            Console.WriteLine($"STATUS: {response.StatusDescription}");
            Stream stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            SavedResponse = reader.ReadToEnd();
            reader.Close();
            response.Close();
        }
    }
}
