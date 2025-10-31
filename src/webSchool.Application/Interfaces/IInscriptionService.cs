namespace application.Interfaces;

using domain.Entities;
using application.DTOs;

public interface IInscriptionService : IGenericService<Inscription, InscriptionDto>
{
    // Reglas adicionales específicas
}
