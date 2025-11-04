namespace application.Interfaces.Services;

public interface IGeneryServices<T>
{
    // contratos para los DTOS

    // para mostrar all
    Task<IEnumerable<T>> GetAllGeneryAsync();

    // para mostar por id
    Task<T> GetIdGeneryAsync(int id);

    // para crear
    Task<T> CreateGeneryAsync(T entity);

    // para Actualizar
    Task<T> UpdateGeneryAsync(int id, T entity);

    // Eliminar
    Task<bool> DeleteGeneryAsync(int id);
}
