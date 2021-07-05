using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using MVCMongodbCrudOpration.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace MVCMongodbCrudOpration.Controllers
{
    public class HomeController : Controller
    {
        MongoClient mongo = new MongoClient("mongodb://localhost");
        MongoServer server;
        MongoDatabase database;
        MongoCollection<info> _infos;
        info _info;
        public ActionResult Index()
        {
            dbcon();
            
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }
        public void dbcon()
        {
            server = mongo.GetServer();
            server.Connect();
            database = server.GetDatabase("mydb");
            _infos = database.GetCollection<info>("info");
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult MongoDBHome()
        {
            dbcon();
            
            
            return View(_infos.FindAll());
        }
        public void addinfo(info _info)
        {

            _info.Id = ObjectId.GenerateNewId();

            _infos.Insert(_info);
            //return item;
        }
        public ActionResult Create()
        { return View(); }
        [HttpPost]
        public ActionResult Create(MVCMongodbCrudOpration.Models.info model)
        {
            dbcon();
            _info = new info { info_id = (int)database.GetCollection("info").Count() + 1, firstname = model.firstname, lastname = model.lastname, age = Convert.ToInt16(model.age) };
            addinfo(_info);
            return View("MongoDBHome",_infos.FindAll());
        }
        public void updateInfo(info _info)
        {
            IMongoQuery query = Query.EQ("info_id", _info.info_id);
            IMongoUpdate update = Update

                .Set("firstname", _info.firstname)

               .Set("lastname", _info.lastname)
               .Set("age", _info.age);
            SafeModeResult result = _infos.Update(query, update);
            // return result.UpdatedExisting;

        }
        public ActionResult Edit(int id)
        {
            dbcon();
          
            IMongoQuery query = Query.EQ("info_id", id);
            _info = _infos.Find(query).FirstOrDefault();
           
           return View(_info);
        }

        [HttpPost]
        public ActionResult Edit(Models.info model)
        {
            dbcon();
             IMongoQuery query = Query.EQ("info_id", model.info_id);
            _info = _infos.Find(query).FirstOrDefault();
            
            _info.firstname = model.firstname;
            _info.lastname = model.lastname;
            _info.age = Convert.ToInt16(model.age);
            updateInfo(_info);
           
            return View("MongoDBHome", _infos.FindAll());
        }

      

        [HttpPost]
        public ActionResult Delete(MVCMongodbCrudOpration.Models.info model)
        {
            dbcon();

            IMongoQuery query = Query.EQ("info_id", model.info_id);
            SafeModeResult result = _infos.Remove(query);
            return View("MongoDBHome", _infos.FindAll());
        }
        public ActionResult Delete(int id)
        {
            dbcon();
            info _info = new info();
            IMongoQuery query = Query.EQ("info_id", id);
            _info = _infos.Find(query).FirstOrDefault();

            return View(_info);

        }

        public ActionResult Details(int id)
        {
            dbcon();

            IMongoQuery query = Query.EQ("info_id", id);
            _info = _infos.Find(query).FirstOrDefault();

            return View(_info);
        }

    }
}
