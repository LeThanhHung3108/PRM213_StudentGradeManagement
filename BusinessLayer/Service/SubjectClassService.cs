using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.IService;
using DataAccessLayer.Entity;
using DataAccessLayer.IRepository;

namespace BusinessLayer.Service;

public class SubjectClassService : ISubjectClassService
{
    private readonly ISubjectClassRepository _repository;

    public SubjectClassService(ISubjectClassRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<SubjectClassDto>> GetPagedAsync(int pageNumber, int pageSize, string? search)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var (items, totalCount) = await _repository.GetPagedAsync(pageNumber, pageSize, search);

        var dtoItems = items.Select(MapToDto).ToList();

        return new PagedResultDto<SubjectClassDto>
        {
            Items = dtoItems,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<SubjectClassDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return null;
        return MapToDto(entity);
    }

    public async Task<SubjectClassDto> CreateAsync(CreateSubjectClassDto dto)
    {
        ValidateCreateDto(dto);

        var entity = new SubjectClass
        {
            SubjectCode = dto.SubjectCode.Trim(),
            SubjectName = dto.SubjectName.Trim(),
            ClassName = dto.ClassName.Trim(),
            Semester = dto.Semester.Trim()
        };

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return MapToDto(entity);
    }

    public async Task UpdateAsync(int id, UpdateSubjectClassDto dto)
    {
        ValidateUpdateDto(dto);

        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) throw new KeyNotFoundException($"SubjectClass {id} not found.");

        entity.SubjectCode = dto.SubjectCode.Trim();
        entity.SubjectName = dto.SubjectName.Trim();
        entity.ClassName = dto.ClassName.Trim();
        entity.Semester = dto.Semester.Trim();

        _repository.Update(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) throw new KeyNotFoundException($"SubjectClass {id} not found.");

        _repository.Delete(entity);
        await _repository.SaveChangesAsync();
    }

    private static SubjectClassDto MapToDto(SubjectClass e) =>
        new SubjectClassDto
        {
            Id = e.Id,
            SubjectCode = e.SubjectCode,
            SubjectName = e.SubjectName,
            ClassName = e.ClassName,
            Semester = e.Semester,
        };

    private static void ValidateCreateDto(CreateSubjectClassDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.SubjectCode)) throw new ArgumentException("SubjectCode is required.");
        if (string.IsNullOrWhiteSpace(dto.SubjectName)) throw new ArgumentException("SubjectName is required.");
        if (string.IsNullOrWhiteSpace(dto.ClassName)) throw new ArgumentException("ClassName is required.");
        if (string.IsNullOrWhiteSpace(dto.Semester)) throw new ArgumentException("Semester is required.");

        if (dto.SubjectCode.Length > 20) throw new ArgumentException("SubjectCode max length is 20.");
    }

    private static void ValidateUpdateDto(UpdateSubjectClassDto dto)
    {
        // same validation rules for update
        ValidateCreateDto(new CreateSubjectClassDto
        {
            SubjectCode = dto.SubjectCode,
            SubjectName = dto.SubjectName,
            ClassName = dto.ClassName,
            Semester = dto.Semester
        });
    }
}
