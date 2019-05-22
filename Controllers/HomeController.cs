using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebFIA.Models;

namespace WebFIA.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Index()
        {
        IEnumerable<place> students = null;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://52.165.230.106/api/v1.0/");
            //HTTP GET
            var responseTask = client.GetAsync("Places");
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<place>>();
                readTask.Wait();

                students = readTask.Result;
            }
            else 
            {
                students = Enumerable.Empty<place>();

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
        }
        return View(students);
    }



     public ActionResult create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult create(place place)
    {
        using (var coordinates = new HttpClient())
        {
            coordinates.BaseAddress = new Uri("http://52.165.230.106/api/v1.0/");

            //HTTP POST
            var postTask = coordinates.PostAsJsonAsync<place>("Places", place);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
        }

        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        return View(place);
    }

    public ActionResult Edit(int id)
        {
            place place = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://52.165.230.106/api/v1.0/");
                //HTTP GET
                var responseTask = client.GetAsync("Places/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<place>();
                    readTask.Wait();

                    place = readTask.Result;
                }
            }

            return View(place);
        }

    [HttpPost]
    public ActionResult Edit(place place)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://52.165.230.106/api/v1.0/");

            //HTTP POST
            var putTask = client.PutAsJsonAsync<place>("Places", place);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
        }
        return View(place);
    }


        public ActionResult Delete(int id)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://52.165.230.106/api/v1.0/");

            //HTTP DELETE
            var deleteTask = client.DeleteAsync("Places/" + id.ToString());
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
        }

        return RedirectToAction("Index");
    }




    }
}
