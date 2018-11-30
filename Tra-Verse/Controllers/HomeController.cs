
﻿using System.Web.Mvc;
﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;


//testing this commit
namespace Tra_Verse.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Always Moving Forward";
            return View();
        }
        public ActionResult NASA()
        {
            //1. request and response, setting up to make the call out to the API
            string nasaAPIKey = System.Configuration.ConfigurationManager.AppSettings["NASA API Header"];
            HttpWebRequest request = WebRequest.CreateHttp("https://api.nasa.gov/planetary/apod?api_key=" + nasaAPIKey);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            //2. Reading the reponse into a streamreader
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            rd.Close(); 
            //nothing works from here on down

            //3.JSON object, Json token list
            JObject giphyJson = JObject.Parse(data);// must parse else would throw the JSON reader exception, its not in JObject format at all
            List<JToken> giphys = giphyJson["data"].ToList();//this one doesnt have children, results are formatted differently
            //you know what key to choose if your website has good JSON documentation (its the manual that tells you what youre looking for, where it's nested and how to find it

            //4. foreach to cycle through list
            List<GiphyModel> output = new List<GiphyModel>(); //dont forget your output
            foreach (JToken token in giphys)
            {
                GiphyModel gp = new GiphyModel();
                gp.ImageURL = token["images"]["downsized"]["url"].ToString();
                gp.Title = token["title"].ToString();
                gp.LinkURL = token["url"].ToString();
                //now add these to the output
                output.Add(gp);
            }


            //5. return the view with an object
            return View(output);
        }
    }
}