namespace StudentManagement.Application.Interfaces;

public interface IBaseService<TDto, in TCreateDto, in TUpdateDto>
{
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto?> GetByIdAsync(Guid id);
    Task<TDto> CreateAsync(TCreateDto createDto);
    Task<TDto> UpdateAsync(Guid id, TUpdateDto updateDto);
    Task DeleteAsync(Guid id);
} 