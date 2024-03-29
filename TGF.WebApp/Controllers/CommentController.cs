﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TGF.WebApp.Models;

namespace TGF.WebApp.Controllers
{
    public class CommentController : Controller
    {
        public IConfiguration Configuration;
        private JWToken JWToken;

        public CommentController(IConfiguration configuration, JWToken jWToken)
        {
            Configuration = configuration;
            JWToken = jWToken;
    }

        public ContentResult GetHostUrl()
        {
            var result = Configuration["RestApiUrl:HostUrl"];
            return Content(result);
        }

        private string CN()
        {
            return ControllerContext.RouteData.Values["controller"].ToString();
        }

        public async Task<IActionResult> Index()
        {
            //var tokenString = GenerateJSONWebToken();

            //string _restpath = "https://localhost:5001/profile";
            string _restpath = GetHostUrl().Content + CN();

            List<CommentVM> commentsList = new List<CommentVM>();

            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync(_restpath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    commentsList = JsonConvert.DeserializeObject<List<CommentVM>>(apiResponse);
                }
            }

            return View(commentsList);
        }

        //[Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CommentVM c)
        {
            // var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(c);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using var response = await httpClient.PostAsync(_restpath, content);
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return RedirectToAction(nameof(Index));
        }

        //[Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CommentVM c = new CommentVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        c = JsonConvert.DeserializeObject<CommentVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(c);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CommentVM c)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CommentVM cResult = new CommentVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(c);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    using (var response = await httpClient.PutAsync($"{ _restpath}/{c.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        cResult = JsonConvert.DeserializeObject<CommentVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(cResult);
        }

        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            CommentVM c = new CommentVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{ _restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        c = JsonConvert.DeserializeObject<CommentVM>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View(c);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(CommentVM c)
        {
            //var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using var response = await httpClient.DeleteAsync($"{ _restpath}/{c.Id}");
                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
