﻿using Preservados.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Preservados.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetQuestions()
        {
            var db = new DBPreservadosDataContext();

            var query = from q in db.Questions
                            select q;
            
            var questions = new List<QuestionModel>();

            foreach (var item in query)
            {
                questions.Add(new QuestionModel()
                {
                    Correct = item.Answer,
                    QuestionID = item.questionId,
                    Title = item.QuestionTitle,
                    Questions = new List<string>()
                    {
                        item.Question1, item.Question2,
                        item.Question3, item.Question4
                    },
                    Tips = item.tip
                });
            }
            
            return new JsonResult()
            {
                Data = questions,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public JsonResult SetData(UserInfoModel modelo)
        {
            var db = new DBPreservadosDataContext();

            db.Resultados.InsertOnSubmit(new Resultado()
            {
                Edad = modelo.edad,
                Nombre = modelo.nombre,
                Preferencia = modelo.preferencia,
                Sexo = modelo.sexo
            });

            db.SubmitChanges();

            return new JsonResult()
            {
                Data = new { status = "ok" }
            };
        }
    }
}
