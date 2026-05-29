using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs;
using BusinessLayer.IService;
using DataAccessLayer.IRepository;

namespace BusinessLayer.Service;

public class StatisticsService : IStatisticsService
{
    private readonly IStatisticsRepository _repository;

    public StatisticsService(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<StatisticsDto> GetStatisticsAsync(int? subjectClassId)
    {
        var (total, pass, fail, average, highest, lowest) = await _repository.GetStatisticsAsync(subjectClassId);
        return new StatisticsDto
        {
            TotalStudents = total,
            PassCount = pass,
            FailCount = fail,
            AverageScore = average,
            HighestScore = highest,
            LowestScore = lowest
        };
    }

    public async Task<PagedResultDto<TopStudentDto>> GetTopStudentsAsync(int pageNumber, int pageSize, int? subjectClassId)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;
        var (items, total) = await _repository.GetTopStudentsAsync(pageNumber, pageSize, subjectClassId);
        var dtoItems = items.Select(x => new TopStudentDto
        {
            StudentId = x.Id,
            RollNumber = x.RollNumber,
            FullName = x.FullName,
            FinalGrade = x.FinalGrade
        }).ToList();
        return new PagedResultDto<TopStudentDto>
        {
            Items = dtoItems,
            TotalCount = total,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<PagedResultDto<TopStudentDto>> GetFailedStudentsAsync(int pageNumber, int pageSize, int? subjectClassId)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;
        var (items, total) = await _repository.GetFailedStudentsAsync(pageNumber, pageSize, subjectClassId);
        var dtoItems = items.Select(x => new TopStudentDto
        {
            StudentId = x.Id,
            RollNumber = x.RollNumber,
            FullName = x.FullName,
            FinalGrade = x.FinalGrade
        }).ToList();
        return new PagedResultDto<TopStudentDto>
        {
            Items = dtoItems,
            TotalCount = total,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}