using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EvaluacionGonzagaJavier.Models;
using EvaluacionGonzagaJavier.Data;

namespace EvaluacionGonzagaJavier.Controllers
{
    public class OrderController : Controller
    {
        private OrderDataAccessLayer orderDataAccessLayer;

        public OrderController()
        {
            orderDataAccessLayer = new OrderDataAccessLayer();
        }

        // GET: OrderController
        public ActionResult Index()
        {
            List<Order> orders = orderDataAccessLayer.GetAllOrders().ToList();
            return View(orders);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            Order order = orderDataAccessLayer.GetOrderData(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    order.CalculateTotals();
                    orderDataAccessLayer.AddOrder(order);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(order);
                }
            }
            return View(order);
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            Order order = orderDataAccessLayer.GetOrderData(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    order.CalculateTotals();
                    orderDataAccessLayer.UpdateOrder(order);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(order);
                }
            }
            return View(order);
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            Order order = orderDataAccessLayer.GetOrderData(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: OrderController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                orderDataAccessLayer.DeleteOrder(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/CalculateTotal/5
        public ActionResult CalculateTotal(int id)
        {
            Order order = orderDataAccessLayer.GetOrderData(id);
            if (order == null)
            {
                return NotFound();
            }
            order.CalculateTotals();
            return View(order);
        }
    }
}