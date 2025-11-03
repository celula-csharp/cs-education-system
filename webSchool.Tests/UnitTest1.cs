using Xunit;
using application.Services;
using domain.Entities;
using domain.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using application.DTOs;

namespace webSchool.Tests;

public class InscriptionServiceTests
{
    private readonly Mock<IRepository<Inscription>> _repoIns = new();
    private readonly Mock<IRepository<Secction>> _repoSec = new();
    private readonly Mock<IRepository<Student>> _repoStu = new();

    private InscriptionService Service =>
        new(_repoIns.Object, _repoSec.Object, _repoStu.Object);

    [Fact]
    public async Task NoPermite_Inscripcion_Duplicada()
    {
        var inscripciones = new List<Inscription>
        {
            new() { Id = 1, StudentId = 1, SecctionId = 1 }
        };
        _repoIns.Setup(r => r.All()).ReturnsAsync(inscripciones);
        _repoSec.Setup(r => r.ById(1)).ReturnsAsync(new Secction { Id = 1, Capacity = 2, Day = "Lunes", StartTime = new(8,0,0), EndTime = new(10,0,0) });
        _repoStu.Setup(r => r.ById(1)).ReturnsAsync(new Student { Id = 1 });

        var dto = new InscriptionDto { StudentId = 1, SecctionId = 1 };
        await Assert.ThrowsAsync<ArgumentException>(() => Service.CreateAsync(dto));
    }

    [Fact]
    public async Task NoPermite_Superar_Cupo()
    {
        var inscripciones = new List<Inscription>
        {
            new() { Id = 1, StudentId = 1, SecctionId = 1 },
            new() { Id = 2, StudentId = 2, SecctionId = 1 }
        };
        _repoIns.Setup(r => r.All()).ReturnsAsync(inscripciones);
        _repoSec.Setup(r => r.ById(1)).ReturnsAsync(new Secction { Id = 1, Capacity = 2, Day = "Lunes", StartTime = new(8,0,0), EndTime = new(10,0,0) });
        _repoStu.Setup(r => r.ById(3)).ReturnsAsync(new Student { Id = 3 });

        var dto = new InscriptionDto { StudentId = 3, SecctionId = 1 };
        await Assert.ThrowsAsync<ArgumentException>(() => Service.CreateAsync(dto));
    }

    [Fact]
    public async Task NoPermite_Choque_Horario()
    {
        var sec1 = new Secction { Id = 1, Day = "Lunes", StartTime = new(8, 0, 0), EndTime = new(10, 0, 0), Capacity = 5 };
        var sec2 = new Secction { Id = 2, Day = "Lunes", StartTime = new(9, 0, 0), EndTime = new(11, 0, 0), Capacity = 5 };

        var inscripciones = new List<Inscription> { new() { StudentId = 1, SecctionId = 1 } };
        _repoIns.Setup(r => r.All()).ReturnsAsync(inscripciones);
        _repoSec.Setup(r => r.ById(2)).ReturnsAsync(sec2);
        _repoStu.Setup(r => r.ById(1)).ReturnsAsync(new Student { Id = 1 });
        _repoSec.Setup(r => r.All()).ReturnsAsync(new List<Secction> { sec1, sec2 });

        var dto = new InscriptionDto { StudentId = 1, SecctionId = 2 };
        await Assert.ThrowsAsync<ArgumentException>(() => Service.CreateAsync(dto));
    }

    [Fact]
    public async Task NoPermite_Nota_FueraDeRango()
    {
        var repoG = new Mock<IRepository<Grades>>();
        var repoI = new Mock<IRepository<Inscription>>();
        var service = new GradeService(repoG.Object, repoI.Object);

        var dto = new GradeDto { Grade = 6, InscriptionId = 1 };
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(dto));
    }
}
