using System;

namespace SeriesCRUD
{
  class Program
  {
    static SerieRepositorio repositorio = new SerieRepositorio();
    static void Main(string[] args)
    {
      string opcaoUsuario = ObterOpcaoUsuario();

      while (opcaoUsuario.ToUpper() != "X")
      {
        switch (opcaoUsuario)
        {
            case "1":
              ListarSeries();
              break;
            case "2":
              InserirSerie();
              break;
            case "3":
              AtualizarSerie();
              break;
            case "4":
              ExcluirSerie();
              break;
            case "5":
              VisualizarSerie();
              break;
            case "C":
              Console.Clear();
              break;
            
            default:
              throw new ArgumentOutOfRangeException();
        }

        opcaoUsuario = ObterOpcaoUsuario();
      }

      Console.WriteLine("Obrigado por utilizar nossos serviços.");
      Console.ReadLine();
    }

    private static void ListarSeries()
    {
      Console.WriteLine("Listar séries");

      var lista = repositorio.Lista();

      if (lista.Count == 0)
      {
        Console.WriteLine("Nenhuma séria cadastrada.");
        return;
      }

      foreach (var serie in lista)
      {
        Console.WriteLine($"#ID {serie.retornaId()} - {serie.retornaTitulo()} {(serie.retornaExcluido() ? "*Excluído*" : "")}");
      }
    }

    private static void InserirSerie()
    {
      Console.WriteLine("Inserir série");

      // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getvalues?view=netcore-3.1
      // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getname?view=netcore-3.1
      try
      {
        var novaSerie = ObterNovaSerie();

        repositorio.Insere(novaSerie);
      }
      catch (System.Exception e)
      {
          Console.Write($"{e.Message} Inserção falha!");
          Console.ReadLine();
      }
    }

    private static void ExcluirSerie()
    {
      Console.Write("Digite o id da série: ");
      int indiceSerie = int.Parse(Console.ReadLine());

      if (indiceSerie >= 0 && indiceSerie < repositorio.Lista().Count)
      {
        var serie = repositorio.RetornaPorId(indiceSerie);

        Console.WriteLine(serie);
        Console.WriteLine();
        Console.WriteLine("Tem certeza que deseja excluir esta série da sua lista?(Y/N) ");
        string confirmacao = Console.ReadLine();

        if (confirmacao.ToUpper() == "Y")
        {
          repositorio.Exclui(indiceSerie);
        }
        else
        {
          Console.WriteLine("Operação cancelada!");
        }
      }
      else
      {
        Console.Write("Id inválido! Exclusão falha!");
        Console.ReadLine();
      }
    }

    private static void VisualizarSerie()
    {
      Console.Write("Digite o id da série: ");
      int indiceSerie = int.Parse(Console.ReadLine());

      if (indiceSerie >= 0 && indiceSerie < repositorio.Lista().Count)
      {
        var serie = repositorio.RetornaPorId(indiceSerie);

        Console.WriteLine(serie.ToString());
      }
      else
      {
        Console.Write("Id inválido! Visualização falha!");
        Console.ReadLine();
      }
    }

    private static void AtualizarSerie()
    {
      Console.Write("Digite o id da série: ");
      int indiceSerie = int.Parse(Console.ReadLine());

      // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getvalues?view=netcore-3.1
      // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getname?view=netcore-3.1
      if (indiceSerie >= 0 && indiceSerie < repositorio.Lista().Count)
      {
        try
        {
          var novaSerie = ObterNovaSerie();

          repositorio.Atualiza(indiceSerie, novaSerie);
        }
        catch (System.Exception e)
        {    
          Console.Write($"{e.Message} Atualização falha!");
          Console.ReadLine();
        }
      }
      else
      {
        Console.Write("Id inválido! Atualização falha!");
        Console.ReadLine();
      }
    }

    private static string ObterOpcaoUsuario()
    {
      Console.WriteLine();
      Console.WriteLine("DIO Serie a seu dispor!!!");
      Console.WriteLine("Informe a opção desejada:");

      Console.WriteLine("1- Listar séries");
      Console.WriteLine("2- Inserir nova série");
      Console.WriteLine("3- Atualizar série");
      Console.WriteLine("4- Excluir série");
      Console.WriteLine("5- Visualizar série");
      Console.WriteLine("C- Limpar tela");
      Console.WriteLine("X- Sair");
      Console.WriteLine();

      string opcaoUsuario = Console.ReadLine().ToUpper();
      Console.WriteLine();
      return opcaoUsuario;
    }

    
    private static Serie ObterNovaSerie()
    {
      foreach (int i in Enum.GetValues(typeof(Genero)))
      {
        Console.WriteLine($"{i}-{Enum.GetName(typeof(Genero), i)}");
      }
      Console.WriteLine("Digite o gênero entre as opções acima: ");
      int entradaGenero = int.Parse(Console.ReadLine());

      if (Enum.IsDefined(typeof(Genero), entradaGenero))
      {
        Console.WriteLine("Digite o Título da Série: ");
        string entradaTitulo = Console.ReadLine();

        Console.WriteLine("Digite o Ano de Início da Série: ");
        int entradaAno = int.Parse(Console.ReadLine());

        Console.WriteLine("Digite a Descrição da Série: ");
        string entradaDescricao = Console.ReadLine();

        Serie novaSerie = new Serie(id: repositorio.ProximoId(),
                                    genero: (Genero)entradaGenero,
                                    titulo: entradaTitulo,
                                    ano: entradaAno,
                                    descricao: entradaDescricao);

        return novaSerie;
      }
      else
      {
        throw new ArgumentOutOfRangeException("Gênero inválido!");
      }
    }
  }
}
