using HVK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HVK.Controllers
{
    public class HomePageController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public HomePageController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int id = (HttpContext.Session.GetInt32("HvkUserID") ?? -1);
            var customer = _context.Hvkusers
                .Where(x => x.HvkuserId == id)
                .Include(x => x.Pets)
                .FirstOrDefault();


            if (customer == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentList = (
                from r in _context.Reservations
                join pr in _context.PetReservations on r.ReservationId equals pr.ReservationId
                join p in _context.Pets on pr.PetId equals p.PetId
                join c in _context.Hvkusers on p.HvkuserId equals c.HvkuserId
                where c.HvkuserId == id && (r.StartDate <= DateTime.Today && r.EndDate > DateTime.Today) && pr.RunId != null
                orderby r.EndDate ascending
                group new { Pet = p, Reservation = r, HvkUser = c } by r.ReservationId into grouped
                select new JoinPetReservationHvkuser
                {
                    Reservation = grouped.FirstOrDefault().Reservation,
                    HvkUser = grouped.FirstOrDefault().HvkUser,
                    Pets = grouped.Select(g => g.Pet).ToList()
                }
            ).ToList();

            var futureList = (
                from r in _context.Reservations
                join pr in _context.PetReservations on r.ReservationId equals pr.ReservationId
                join p in _context.Pets on pr.PetId equals p.PetId
                join c in _context.Hvkusers on p.HvkuserId equals c.HvkuserId
                where c.HvkuserId == id && (r.StartDate > DateTime.Today && r.EndDate > DateTime.Today) && pr.RunId == null
                orderby r.StartDate ascending
                group new { Pet = p, Reservation = r, HvkUser = c } by r.ReservationId into grouped
                select new JoinPetReservationHvkuser
                {
                    Reservation = grouped.FirstOrDefault().Reservation,
                    HvkUser = grouped.FirstOrDefault().HvkUser,
                    Pets = grouped.Select(g => g.Pet).ToList()
                }
            ).ToList();

            ViewData["Current"] = currentList;
            ViewData["Future"] = futureList;
            return View(customer);
        }

        public IActionResult Staff()
        {
            var leaveList = (
                from r in _context.Reservations
                join pr in _context.PetReservations on r.ReservationId equals pr.ReservationId
                join p in _context.Pets on pr.PetId equals p.PetId
                join c in _context.Hvkusers on p.HvkuserId equals c.HvkuserId
                where r.EndDate == DateTime.Today
                orderby c.FirstName, c.LastName ascending
                group new { Pet = p, Reservation = r, HvkUser = c } by r.ReservationId into grouped
                select new JoinPetReservationHvkuser
                {
                    Reservation = grouped.FirstOrDefault().Reservation,
                    HvkUser = grouped.FirstOrDefault().HvkUser,
                    Pets = grouped.Select(g => g.Pet).ToList()
                }
            ).ToList();

            var nowList = (
                from r in _context.Reservations
                join pr in _context.PetReservations on r.ReservationId equals pr.ReservationId
                join p in _context.Pets on pr.PetId equals p.PetId
                join c in _context.Hvkusers on p.HvkuserId equals c.HvkuserId
                where r.StartDate <= DateTime.Today && r.EndDate > DateTime.Today && pr.RunId != null
                orderby r.EndDate ascending
                group new { Pet = p, Reservation = r, HvkUser = c } by r.ReservationId into grouped
                select new JoinPetReservationHvkuser
                {
                    Reservation = grouped.FirstOrDefault().Reservation,
                    HvkUser = grouped.FirstOrDefault().HvkUser,
                    Pets = grouped.Select(g => g.Pet).ToList()
                }
            ).ToList();

            var startingToday = (
                from r in _context.Reservations
                join pr in _context.PetReservations on r.ReservationId equals pr.ReservationId
                join p in _context.Pets on pr.PetId equals p.PetId
                join c in _context.Hvkusers on p.HvkuserId equals c.HvkuserId
                where r.StartDate.DayOfYear == DateTime.Today.DayOfYear && r.StartDate.Year == DateTime.Today.Year && pr.RunId == null
                orderby r.StartDate ascending
                group new { Pet = p, Reservation = r, HvkUser = c } by r.ReservationId into grouped
                select new JoinPetReservationHvkuser
                {
                    Reservation = grouped.FirstOrDefault().Reservation,
                    HvkUser = grouped.FirstOrDefault().HvkUser,
                    Pets = grouped.Select(g => g.Pet).ToList()
                }
            ).ToList();

            ViewData["LeavingToday"] = leaveList;
            ViewData["Ongoing"] = nowList;
            ViewData["StartingToday"] = startingToday;
            return View();
        }
    }
}
