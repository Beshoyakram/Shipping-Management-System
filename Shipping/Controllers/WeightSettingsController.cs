using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.Models;

namespace Shipping.Controllers
{
    public class WeightSettingsController : Controller
    {
        MyContext _context;
        public WeightSettingsController(MyContext context)
        {
            _context = context;
        }

        // GET: WeightSettings/Edit
        public async Task<IActionResult> Edit()
        {
            var shippingCost = await _context.weightSettings.FirstOrDefaultAsync();
            if (shippingCost == null)
            {
                return NotFound();
            }

            return View(shippingCost);
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cost,Addition_Cost")] WeightSetting shippingCost)
        {
            if (id != shippingCost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingCost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingCostExists(shippingCost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }

            return View(shippingCost);
        }
        private bool ShippingCostExists(int id)
        {
            return _context.weightSettings.Any(e => e.Id == 1);
        }
    }
}

