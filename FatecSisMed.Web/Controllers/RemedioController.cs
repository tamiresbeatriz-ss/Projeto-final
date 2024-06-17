using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FatecSisMed.Web.Models;
using FatecSisMed.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FatecSisMed.Web.Controllers
{
    public class RemedioController : Controller
    {

        private readonly IRemedioService _remedioService;
        private readonly IMarcaService _marcaService;

        public RemedioController(IRemedioService remedioService, IMarcaService marcaService)
        {
            _remedioService = remedioService;
            _marcaService = marcaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RemedioViewModel>>> Index()
        {
            var result = await _remedioService.GetAllRemedios(await GetAccessToken());
            if (result == null) return View("Error");
            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> CreateRemedio()
        {
            var marcasResponse = await _marcaService.GetAllMarcas(await GetAccessToken());
            ViewBag.MarcaId = new SelectList(marcasResponse, "Id", "nome");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRemedio(RemedioViewModel marcaViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _remedioService.CreateRemedio(marcaViewModel, await GetAccessToken());
                if (result != null) return RedirectToAction(nameof(Index));
            }
            return View(marcaViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRemedio(int id)
        {
            var result = await _remedioService.FindRemedioById(id, await GetAccessToken());
            var marcasResponse = await _marcaService.GetAllMarcas(await GetAccessToken());
            ViewBag.MarcaId = new SelectList(marcasResponse, "Id", "nome");
            if (result == null) return View("Error");
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRemedio(RemedioViewModel marcaViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _remedioService.UpdateRemedio(marcaViewModel, await GetAccessToken());
                if (result != null) return RedirectToAction(nameof(Index));
            }
            return View(marcaViewModel);
        }

        [HttpGet]
        public async Task<ActionResult<RemedioViewModel>> DeleteRemedio(int id)
        {
            var result = await _remedioService.FindRemedioById(id, await GetAccessToken());
            if (result == null) return View("Error");
            return View(result);
        }

        [HttpPost]
        [ActionName("DeleteRemedio")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _remedioService.DeleteRemedioById(id, await GetAccessToken());
            if (!result) return View("Error");
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GetAccessToken()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }
    }

}