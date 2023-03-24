﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            IQueryable<Category> categories = DataController.GetCategories();
            IQueryable<Product> products = DataController.RandomProduct(DataController.GetProducts("New", ""),5);
            IQueryable<Product> all_product = DataController.GetProducts("", "");
            return View(products, all_product, categories);
        }

        private ActionResult View(IQueryable<Product> products, IQueryable<Product> all_product, IQueryable<Category> categories)
        {
            ViewBag.Categories = categories;
            ViewBag.Products = products;
            ViewBag.AllProducts = all_product;
            return View();
        }

        // GET: Shop/Details/5
        public ActionResult Details(int id)
        {

            IQueryable<Category> categories = DataController.GetCategories();
            var product = DataController.GetProduct(id);
            IQueryable<Product> products = DataController.RandomProduct(DataController.GetProducts("", ""), 6);
            IQueryable<ImageProduct> img = DataController.Image(id);
            return View(product, products, categories, img);
        }

        private ActionResult View(Product product, IQueryable<Product> products, IQueryable<Category> categories, IQueryable<ImageProduct> img)
        {
            ViewBag.Categories = categories;
            ViewBag.product = product;
            ViewBag.Products = products;
            ViewBag.Image = img;
            return View();
        }

        // GET: Shop/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shop/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Shop/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Shop/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Shop/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Shop/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}