using Pedido.Infra.Interfaces;
using ProjetoEntidades.Models;

namespace Pedido.Infra;

public class Repositorio : IRepositorio
{
    private readonly SqlContext context;

    public Repositorio(SqlContext context)
    {
        this.context = context;
    }
    public int Adicionar<T>(T obj) where T : EntidadeBase
    {
        context.Set<T>().Add(obj);
        return context.SaveChanges();
    }

    public int Deletar<T>(int id) where T : EntidadeBase
    {
        var entidade = context.Set<T>().FirstOrDefault(c => c.Id == id);
        if (entidade is null)
        {
            return 0;
        }
        context.Set<T>().Remove(entidade);
        return context.SaveChanges();
    }

    public int Merge<T>(T obj) where T : EntidadeBase
    {
        context.Set<T>().Update(obj);
        return context.SaveChanges();
    }

    public T? Retornar<T>(int id) where T : EntidadeBase
    {
        return context.Set<T>().FirstOrDefault(c => c.Id == id);
    }

    public List<T> RetornarList<T>() where T : EntidadeBase
    {
        return context.Set<T>().ToList();
    }
}