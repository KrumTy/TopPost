namespace TopPost.Web.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using TopPost.Data;
    using TopPost.Models;
    using TopPost.Web.Areas.Administration.Controllers;
    using TopPost.Web.Infrastructure.Caching;

    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;

    using Model = TopPost.Models.Category;
    using ViewModel = TopPost.Web.Areas.Administration.Models.CategoryViewModel;
    using System.Collections.Generic;

    public class CategoriesController : KendoGridAdministrationController
    {
        public CategoriesController(ITopPostData data, ICacheService service)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        protected override IEnumerable GetData()
        {
            return this.db
                .Categories
                .All()
                .Project()
                .To<ViewModel>();
        }

        protected override T GetById<T>(object id)
        {
            return this.db.Categories.Find(id) as T;
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            //if (model != null && ModelState.IsValid)
            //{
            //    var category = new Category()
            //    {
            //        Name = model.Name
            //    };

            //    this.db.Categories.Add(category);
            //    this.db.SaveChanges();

            //    model.Id = category.Id;
            //    return this.GridOperation(model, request);
            //}
            var dbModel = base.Create<Model>(model);
            if (dbModel != null) model.Id = dbModel.Id;

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            var category = this.db.Categories.Find(model.Id);

            if (category == null)
            {
                category = new Category()
                {
                    Name = model.Name
                };
                this.db.Categories.Add(category);
            }
            else
            {
                category.Name = model.Name;
                this.db.Categories.Update(category);
            }

            this.db.SaveChanges();

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            //if (model != null && ModelState.IsValid)
            {
                var category = this.db.Categories.Find(model.Id);

                //foreach (var post in category.Posts.Select(x=>x.Id))
                //{
                    
                //}

                this.db.Categories.Delete(category);
                this.db.SaveChanges();
            }

            return this.GridOperation(model, request);
        }        
    }
}