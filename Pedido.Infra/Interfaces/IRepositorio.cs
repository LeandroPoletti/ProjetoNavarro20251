using ProjetoEntidades.Models;

namespace Pedido.Infra.Interfaces;

public interface IRepositorio
{
    public int Adicionar<T>(T obj) where T : EntidadeBase;
    public int Deletar<T>(int id) where T : EntidadeBase;
    public int Merge<T>(T obj) where T : EntidadeBase;
    public T? Retornar<T>(int id) where T : EntidadeBase;
    public List<T> RetornarList<T>() where T : EntidadeBase;
}