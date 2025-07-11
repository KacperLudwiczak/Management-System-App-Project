using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using LeaveManagementSystem.Models.LeaveTypes;

namespace LeaveManagementSystem.Controllers;

public class LeaveTypesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private const string NameExistsValidationMessage = "This leave type already exists in the database";

    public LeaveTypesController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: LeaveTypes
    public async Task<IActionResult> Index()
    {
        // var data = SELECT * FROM LeaveTypes
        var data = await _context.LeaveTypes.ToListAsync();
        // convert the datamodel into a view model - Use AutoMapper
        var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
        // return the view model to the view
        return View(viewData);
    }

    // GET: LeaveTypes/Details/id
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        // Parameterization - key for preventing SQL Injection attacks
        // Select * from LeaveTypes WHERE Id = @id
        var leaveType = await _context.LeaveTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (leaveType == null)
        {
            return NotFound();
        }

        var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);

        return View(viewData);
    }

    // GET: LeaveTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LeaveTypes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
    {
        // Adding custom validation and model state error
        if (await CheckIfLeaveTypeNameExists(leaveTypeCreate.Name))
        {
            ModelState.AddModelError(nameof(leaveTypeCreate.Name), "This leave type already exists in the database");
        }

        if (ModelState.IsValid)
        {
            var leaveType = _mapper.Map<LeaveType>(leaveTypeCreate);
            _context.Add(leaveType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(leaveTypeCreate);
    }

    // GET: LeaveTypes/Edit/id
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Select * from LeaveTypes WHERE Id = @id
        var leaveType = await _context.LeaveTypes.FindAsync(id);
        if (leaveType == null)
        {
            return NotFound();
        }

        var viewData = _mapper.Map<LeaveTypeEditVM>(leaveType);

        return View(viewData);
    }

    // POST: LeaveTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
    {
        if (id != leaveTypeEdit.Id)
        {
            return NotFound();
        }

        // Adding custom validation and model state error
        if (await CheckIfLeaveTypeNameExistsForEdit(leaveTypeEdit))
        {
            ModelState.AddModelError(nameof(leaveTypeEdit.Name), NameExistsValidationMessage);
        }

        if (ModelState.IsValid)
        {
            try
            {
                var leaveType = _mapper.Map<LeaveType>(leaveTypeEdit);
                _context.Update(leaveType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveTypeExists(leaveTypeEdit.Id))
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
        return View(leaveTypeEdit);
    }

    // GET: LeaveTypes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var leaveType = await _context.LeaveTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (leaveType == null)
        {
            return NotFound();
        }

        var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);

        return View(viewData);
    }

    // POST: LeaveTypes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var leaveType = await _context.LeaveTypes.FindAsync(id);
        if (leaveType != null)
        {
            _context.LeaveTypes.Remove(leaveType);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LeaveTypeExists(int id)
    {
        return _context.LeaveTypes.Any(e => e.Id == id);
    }

    private async Task<bool> CheckIfLeaveTypeNameExists(string name)
    {
        var lowercaseName = name.ToLower();
        return await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(lowercaseName));
    }
    
    private async Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit)
    {
        var lowercaseName = leaveTypeEdit.Name.ToLower();
        return await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(lowercaseName)
            && q.Id != leaveTypeEdit.Id);
    }
}