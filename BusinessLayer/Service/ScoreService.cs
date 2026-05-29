using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.IService;
using DataAccessLayer.Entity;
using DataAccessLayer.Enums;
using DataAccessLayer.IRepository;

namespace BusinessLayer.Service;

public class ScoreService : IScoreService
{
    private readonly IScoreRecordRepository _repository;

    public ScoreService(IScoreRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<ScoreRecordDto>> GetPagedAsync(int pageNumber, int pageSize, int? subjectClassId, string? search, string? status)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;
        var (items, totalCount) = await _repository.GetPagedAsync(pageNumber, pageSize, subjectClassId, search, status);
        var dtoItems = items.Select(MapToDto).ToList();
        return new PagedResultDto<ScoreRecordDto>
        {
            Items = dtoItems,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<ScoreDetailDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return null;
        return MapToDetailDto(entity);
    }

    public async Task UpdateAsync(int id, UpdateScoreDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) throw new KeyNotFoundException();
        if (dto.FinalGrade < 0) throw new ArgumentException("FinalGrade must be non-negative.");
        if (!Enum.TryParse<ScoreStatus>(dto.Status, true, out var parsedStatus)) throw new ArgumentException("Status must be 'Pass' or 'Fail'.");
        entity.FinalGrade = dto.FinalGrade;
        entity.Status = parsedStatus;
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) throw new KeyNotFoundException();
        _repository.Delete(entity);
        await _repository.SaveChangesAsync();
    }

    private static ScoreRecordDto MapToDto(ScoreRecord e) =>
        new ScoreRecordDto
        {
            Id = e.Id,
            RollNumber = e.RollNumber,
            FullName = e.FullName,
            FinalGrade = e.FinalGrade,
            Status = e.Status.ToString()
        };

    private static ScoreDetailDto MapToDetailDto(ScoreRecord e) =>
        new ScoreDetailDto
        {
            Id = e.Id,
            RollNumber = e.RollNumber,
            FullName = e.FullName,
            FinalGrade = e.FinalGrade,
            Status = e.Status.ToString(),
            Components = e.ScoreComponents?.Select(c => new ScoreComponentDto
            {
                ComponentName = c.ComponentName,
                ScoreValue = c.ScoreValue
            }).ToList() ?? new List<ScoreComponentDto>()
        };
}