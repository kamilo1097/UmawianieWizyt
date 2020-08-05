using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VisitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Visits
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Visits.Include(v => v.Doctor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Visits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visits = await _context.Visits
                .Include(v => v.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visits == null)
            {
                return NotFound();
            }

            return View(visits);
            
        }
        //Pobrać listę Wszystkich terminów w których znajduje się ID DOCTORA oraz termin (DATE) sama data (bez godziny) jest równa terminowi wybranemu
        //Następnie porównać pobraną listę za każdym razem z przyciętę godziną i ta która istnieje ma być zablokowana
        //AJAX Sprawdzający przy kazdej zmianie daty funkcje powyżej
        public IActionResult RefSelectList()
        {
            var date = Request.Form["Date"].ToString();
            int idDoc = Int32.Parse(Request.Form["DoctorId"].ToString());
            var visits = _context.Visits.Where(a => a.Date.Date.ToString() == date).Where(a=>a.Doctor.Id == idDoc).Select(a=>a.Date.ToString("HH:mm"));
            List<SelectListItem> itemList = new List<SelectListItem>();
            string iString;
            string mString;
            for (int i = 8; i < 17; i++)
            {
                if (i < 10)
                {
                    iString = "0" + i;
                }
                else
                {
                    iString = i.ToString();
                }
                int m = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (m < 10)
                    {
                        mString = "0" + m;
                    }
                    else
                    {
                        mString = m.ToString();
                    }
                    string hoursString = iString + ":" + mString;
                    if(visits.AsEnumerable().Where(s => s == hoursString).Any())
                    {
                        itemList.Add(new SelectListItem { Text = hoursString, Value = hoursString, Disabled = true });
                    }
                    else
                    {
                        itemList.Add(new SelectListItem { Text = hoursString, Value = hoursString, Disabled = false });
                    }
                    
                    
                    m += 15;
                }
            }
            //Odświeżenie Selectlisty
            return new JsonResult(itemList);
        }
        public IActionResult RedirectToCreate(int? id)
        {
            TempData["docId"] = id;
            return RedirectToAction("Create");
        }
        // GET: Visits/Create
        public List<SelectListItem> HoursSelectList()
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            string iString;
            string mString;
            for (int i = 8; i < 17; i++)
            {
                if (i < 10)
                {
                    iString = "0" + i;
                }
                else
                {
                    iString = i.ToString();
                }

                int m = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (m < 10)
                    {
                        mString = "0" + m;
                    }
                    else
                    {
                        mString = m.ToString();
                    }
                    string hoursString = iString + ":" + mString;
                    itemList.Add(new SelectListItem { Text = hoursString, Value = hoursString });
                    m += 15;
                }
            }
            return itemList;
        }
        public IActionResult Create()
        {
            
            if (TempData["docId"] == null)
            {
                return NotFound();
            }
            else
            {
                int idDoc = Int32.Parse(TempData["docId"].ToString());
                ViewData["DocId"] = idDoc;
                ViewData["DocName"] = _context.Doctors.Where(n => n.Id == idDoc).Select(n => n.PersonalData).FirstOrDefault();
                ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "PersonalData");
                ViewData["HoursList"] = HoursSelectList();
                return View();
                
            }
        }

        // POST: Visits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,Date,FirstName,LastName,PhoneNumber")] Visits visits)
        {
            
            if (ModelState.IsValid)
            {
                ///////////////DROBNA EDYCJA ZMIENNYCH//////////////////
                var date = Request.Form["Date"].ToString();
                var time = Request.Form["Time"].ToString();
                string fullDate = date + " " + time;
                TempData["message"] = "Pomyślnie dodano wizytę w terminie: " + fullDate;
                visits.Date = Convert.ToDateTime(fullDate);
               ////////DODANIE DO CONTEXTU I ZAPISANIE/////////////////
                _context.Add(visits);
                await _context.SaveChangesAsync();
                return Redirect("/");
            }
            
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "PersonalData", visits.DoctorId);
            return View();
        }

        // GET: Visits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visits = await _context.Visits.FindAsync(id);
            if (visits == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "PersonalData", visits.DoctorId);
            return View(visits);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,Date")] Visits visits)
        {
            if (id != visits.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visits);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitsExists(visits.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "PersonalData", visits.DoctorId);
            return View(visits);
        }

        // GET: Visits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visits = await _context.Visits
                .Include(v => v.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visits == null)
            {
                return NotFound();
            }

            return View(visits);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visits = await _context.Visits.FindAsync(id);
            _context.Visits.Remove(visits);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitsExists(int id)
        {
            return _context.Visits.Any(e => e.Id == id);
        }
    }
}
