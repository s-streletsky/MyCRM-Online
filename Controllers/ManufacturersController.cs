﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCRM_Online.Db;
using MyCRM_Online.Models.Entities;
using MyCRM_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyCRM_Online.ViewModels.Manufacturers;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class ManufacturersController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public ManufacturersController(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            IQueryable<ManufacturerEntity> source = dataContext.Manufacturers;
            var totalCount = await source.CountAsync();
            var entities = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var manufacturers = mapper.Map<List<ManufacturerEntity>>(entities);

            var pageInfo = new PageInfo<ManufacturerEntity>(totalCount, page, pageSize, manufacturers);

            return View(pageInfo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] ManufacturerCreateViewModel manufacturer)
        {
            if (!ModelState.IsValid)
            {
                return View(manufacturer);                
            }

            var newManufacturer = mapper.Map<ManufacturerEntity>(manufacturer);
            dataContext.Manufacturers.Add(newManufacturer);
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var entity = dataContext.Manufacturers.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            var manufacturer = mapper.Map<ManufacturerEditViewModel>(entity);

            return View(manufacturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, ManufacturerEditViewModel manufacturer)
        {
            if (manufacturer.Id != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(manufacturer);
            }

            var entity = dataContext.Manufacturers.Find(id);
            mapper.Map(manufacturer, entity);
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete([FromForm] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            dataContext.Manufacturers.Remove(new ManufacturerEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
