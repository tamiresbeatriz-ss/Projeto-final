using FatecSisMed.Web.Models;
using FatecSisMed.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FatecSisMed.Web.Controllers
{
    public class MarcaController : Controller
    {
        private readonly IMarcaService _marcaService;

        public MarcaController(IMarcaService marcaService)
        {
            _marcaService = marcaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcaViewModel>>> Index()
        {
            var result = await _marcaService.GetAllMarcas(await GetAccessToken());
            if (result == null) return View("Error");
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateMarca()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMarca(MarcaViewModel marcaViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _marcaService.CreateMarca(marcaViewModel, await GetAccessToken());
                if (result != null) return RedirectToAction(nameof(Index));
            }
            return View(marcaViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMarca(int id)
        {
            var result = await _marcaService.FindMarcaById(id, await GetAccessToken());
            if (result == null) return View("Error");
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMarca(MarcaViewModel marcaViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _marcaService.UpdateMarca(marcaViewModel, await GetAccessToken());
                if (result != null) return RedirectToAction(nameof(Index));
            }
            return View(marcaViewModel);
        }

        [HttpGet]
        public async Task<ActionResult<MarcaViewModel>> DeleteMarca(int id)
        {
            var result = await _marcaService.FindMarcaById(id, await GetAccessToken());
            if (result == null) return View("Error");
            return View(result);
        }

        [HttpPost]
        [ActionName("DeleteMarca")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _marcaService.DeleteMarcaById(id, await GetAccessToken());
            if (!result) return View("Error");
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GetAccessToken()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }
    }
}
