using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mission13.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    {
        private IBowlersRepository _repo { get; set; }

        public HomeController(IBowlersRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index(int teamid)
        {
            if(teamid == 0)
            {
                ViewBag.Team = "";
            }
            else
            {
                ViewBag.Team = _repo.Teams
                .Single(b => b.TeamID == teamid).TeamName;
            }
            

            var blah = _repo.Bowlers
                .Where(b => b.TeamID == teamid || teamid == 0)
                .ToList();

            return View(blah);
        }


        [HttpGet]
        public IActionResult AddBowler()
        {
            ViewBag.Teams = _repo.Teams.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddBowler(Bowler b)
        {
            if (ModelState.IsValid)
            {
                _repo.CreateBowler(b);

                return View("Confirmation", b);
            }
            else //If Invalid
            {
                ViewBag.Teams = _repo.Teams.ToList();

                return View();
            }

        }

        [HttpGet]
        public IActionResult Edit(int bowlerid)
        {
            ViewBag.Teams = _repo.Teams.ToList();

            var bowler = _repo.Bowlers.Single(x => x.BowlerID == bowlerid);

            return View("AddBowler", bowler);
        }

        [HttpPost]
        public IActionResult Edit(Bowler blah)
        {
            _repo.SaveBowler(blah);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int bowlerid)
        {
            var bowler = _repo.Bowlers.Single(x => x.BowlerID == bowlerid);

            return View(bowler);
        }

        [HttpPost]
        public IActionResult Delete (Bowler b)
        {
            _repo.DeleteBowler(b);

            return RedirectToAction("Index");
        }
    }
}
