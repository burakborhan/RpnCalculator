using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using CalculatorWeb.Data;
using CalculatorWeb.Data.Interfaces;

namespace CalculatorWeb.Controllers
{
    public class CalculateController : Controller
    {
        private readonly IRpnWithCache _rpnWithCache;
        private readonly IRpnCalculate _rpnCalculate;
        private readonly IMemoryCache _memoryCache;


        public CalculateController(IRpnCalculate rpnCalculate, IRpnWithCache rpnWithCache, IMemoryCache memoryCache)
        {
            _rpnCalculate = rpnCalculate;
            _rpnWithCache = rpnWithCache;
            _memoryCache = memoryCache;
        }

        public IActionResult Calculator()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculator(string s)
        {
            _rpnCalculate.CalculateExpression(s);
            _rpnWithCache.CalculateRpmWithCache(s);
            ViewBag.CachedResults = _rpnWithCache.GetAllCachedItems(); 
            return View("Calculator");
        }
    }
}
