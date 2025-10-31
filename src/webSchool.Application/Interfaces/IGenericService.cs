namespace application.Interfaces;

public interface IGenericService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    Task<List<TDto>> AllAsync();
    Task<TDto?> ByIdAsync(int id);
    Task<TDto> CreateAsync(TDto dto);
    Task<bool> UpdateAsync(TDto dto);
    Task<bool> DeleteAsync(int id);
}
