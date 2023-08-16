# Prova Técnica - Desenvolvedor de Software Backend .Net

Este repositório contém a implementação de uma API em .Net 6. A API permite realizar operações CRUD (Create, Read, Update, Delete) para o recurso "Produto". Além disso, a implementação inclui regras de validação, campos calculados e utiliza três tipos de repositórios para armazenar os dados dos produtos: Arquivo Texto, Banco de Dados SQL e Banco NoSQL.

---

## Instruções

Para testar a API e executar os testes unitários, siga as instruções abaixo:

### Requisitos

- Plataforma: .NET Core 6
- Banco de Dados SQL: SQL Server
- Banco NoSQL: MongoDB

---

### Passos

1. Clone o repositório:

```bash
git clone https://github.com/Jvramiro/tech-test-backend-csharp.git
```

2. Atribua as conexões com os bancos de dados:

- Abra o arquivo appsettings.json.
- Substitua as strings de conexão para o SQL Server e MongoDB:

```json
"ConnectionStrings": {
  "SQLServer": "SUA_CONNECTION_STRING_SQL_SERVER",
  "MongoDB": "SUA_CONNECTION_STRING_MONGODB"
}
```

3. Execute a API

- Ao executar a API irá abrir um Controle Swagger em seu navegador
- Realize o CRUD nos endpoints de acordo com as informações requisitas  
  **Post** para Criar um Produto  
  **Get** para Ler todos os produtos  
  **Get** com Id para Ler um produto  
  **Put** para Atualizar um produto  
  **Delete** para Remover um produto

## Testes Unitários

### Através do Projeto Tester é possível testar os endpoints

- Abra o terminal na raiz do projeto.
- Execute o seguinte comando:

```Bash
dotnet test
```
---

## Individualidades

### Existem observações importantes do projeto para análise avaliatória

- **Selecionar o Banco de Dados no Swagger:**
Através do Swagger, você pode realizar as operações CRUD e selecionar o Banco de Dados que deseja utilizar.

- **Connection Strings:** Certifique-se de substituir as strings de conexão no arquivo appsettings.json para o SQL Server e MongoDB, permitindo a comunicação adequada com os bancos de dados.

- **Caminho para CRUD em Arquivo de Texto:** Se desejar alterar o caminho para realizar o CRUD em arquivo de texto, altere o arquivo appsettings conforme demonstrado abaixo:
```json
"FilePath": "PASTA_OU_DIRETORIO_DE_PREFERENCIA/NOME_DO_ARQUIVO.txt"
```

- **Nome do Banco de Dandos NoSQL:** Se desejar alterar o nome para o Banco de Dados NoSQL, altere o arquivo appsettings conforme demonstrado abaixo:
```json
  "Database": {
    "Name": "NOME_DO_BANCO_DE_DADOS"
  }
```

- **Seleção do Servidor para Testes:** No construtor do ProdutoControllerTest, você pode escolher o servidor para realizar os testes. Basta ajustar o parâmetro databaseSelection entre DatabaseSelection.SqlServer, DatabaseSelection.MongoDB ou DatabaseSelection.FileText, de acordo com suas necessidades.

- **Execução de Testes:** Todos os testes foram criados para validar as principais funcionalidades da API, garantindo seu funcionamento correto.

---

## Considerações Finais

Este projeto implementa uma API em C# que atende aos requisitos da prova técnica da companhia Mobilus Tecnologia. Ele inclui uma estrutura flexível para manipular diferentes bancos de dados, regras de validação, campos calculados e testes unitários para garantir a qualidade do código. Os devidos testes foram criados, utilizados em mudanças na estruta e validados. Sinta-se à vontade para explorar o código.
