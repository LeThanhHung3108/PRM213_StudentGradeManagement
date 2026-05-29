using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository;

public class SubjectClassRepository : ISubjectClassRepository
{
    private readonly AppDbContext _context;

    public SubjectClassRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<SubjectClass> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? search)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 10;

        var query = _context.SubjectClasses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(sc =>
                EF.Functions.ILike(sc.SubjectCode, $"%{search}%")
                || EF.Functions.ILike(sc.SubjectName, $"%{search}%")
                || EF.Functions.ILike(sc.ClassName, $"%{search}%"));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(sc => sc.SubjectCode)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<SubjectClass?> GetByIdAsync(int id)
    {
        return await _context.SubjectClasses
            .FirstOrDefaultAsync(sc => sc.Id == id);
    }

    public async Task AddAsync(SubjectClass entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await _context.SubjectClasses.AddAsync(entity);
    }

    public void Update(SubjectClass entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _context.SubjectClasses.Update(entity);
    }

    public void Delete(SubjectClass entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _context.SubjectClasses.Remove(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
