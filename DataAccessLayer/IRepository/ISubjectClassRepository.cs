using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Entity;

namespace DataAccessLayer.IRepository;

public interface ISubjectClassRepository
{
    Task<(List<SubjectClass> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? search);
    Task<SubjectClass?> GetByIdAsync(int id);
    Task AddAsync(SubjectClass entity);
    void Update(SubjectClass entity);
    void Delete(SubjectClass entity);
    Task<int> SaveChangesAsync();
}
