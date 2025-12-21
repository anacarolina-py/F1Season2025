# Team Management Service API

O **Team Management Service** integra a suíte **F1Simulator** e é responsável por **gerenciar equipes, profissionais e carros**, garantindo que todas as regras estruturais da Fórmula 1 sejam respeitadas antes do início da temporada.

Este serviço atua como a **fonte oficial de dados organizacionais**, sendo consumido por outros módulos como **Engineering**, **Competition** e **Race Simulation**.

---

## 🚀 Propósito do Módulo

O Team Management representa a **administração esportiva e técnica das equipes**, com responsabilidades como:

- Cadastro e ativação de times.
- Gerenciamento de profissionais (pilotos, engenheiros e chefes).
- Associação correta de profissionais aos carros.
- Garantia de integridade estrutural de cada time.
- Fornecimento de dados confiáveis para outros serviços.

---

## 📋 Diretrizes de Negócio

### Estrutura Obrigatória de um Time Ativo
Para que um time possa ser ativado (`Status = Ativa`), ele deve possuir:

- 2 Pilotos ativos  
- 2 Chefes de equipe ativos  
- 2 Engenheiros Aerodinâmicos ativos  
- 2 Engenheiros de Potência ativos  
- 2 Carros ativos  

Caso qualquer requisito não seja atendido, a ativação é bloqueada via **trigger no banco de dados**.

---

### Regras de Profissionais
- Nomes não podem conter números.
- Idade permitida: **18 a 120 anos**.
- Experiência deve estar entre **1.0 e 5.0**.
- Todo profissional pertence a apenas **um time**.

---

### Regras de Carros
- Modelo deve seguir o padrão `ABC12`.
- Coeficientes aerodinâmico e de potência variam entre `0` e `10`.
- Peso permitido: `700` a `999.99`.
- Todos os profissionais associados ao carro devem pertencer ao **mesmo time**.
- Máximo de **2 carros ativos por time**.

---

## 🔗 Integração com Outros Serviços

### Engineering Service
- Fornece:
  - Coeficientes técnicos dos carros.
  - Experiência dos engenheiros.
  - Handicap e experiência dos pilotos.
- A Engineering API **não altera dados do Team Management**.

---

### Competition Service
- Utiliza o Team Management para:
  - Validar se todos os times estão completos antes do início da temporada.
  - Garantir integridade do grid.

---

## 🛣️ Rotas da Aplicação

### 📌 Times

#### Criar Time
POST /api/teams

```json
{
  "name": "Ferrari"
}
```

---

#### Ativar Time
PUT /api/teams/{teamId}/activate

---

### 👤 Profissionais

#### Criar Staff
POST /api/staffs

```json
{
  "firstName": "Carlos",
  "lastName": "Sainz",
  "age": 29,
  "experience": 4.2,
  "teamId": 1
}
```

---

#### Vincular Profissional
- Piloto
- Engenheiro
- Chefe

POST /api/drivers  
POST /api/engineers  
POST /api/bosses  

---

### 🚗 Carros

#### Criar Carro
POST /api/cars

```json
{
  "model": "FER25",
  "aerodynamicCoefficient": 7.2,
  "powerCoefficient": 7.0,
  "weight": 795,
  "teamId": 1,
  "driverId": 10,
  "powerEngineerId": 5,
  "aerodynamicEngineerId": 6
}
```

---

### 🔄 Integração Técnica (Engineering)

#### Obter Dados para Engenharia
Retorna todos os dados necessários para evolução técnica.

GET /api/teams/{teamId}/engineering-info

**Response**
```json
[
  {
    "teamId": 1,
    "carId": 1,
    "driverId": 10,
    "aerodynamicCoefficient": 7.2,
    "powerCoefficient": 7.0,
    "engineerExperienceCa": 4.5,
    "engineerExperienceCp": 4.2,
    "driverHandicap": 95,
    "driverExperience": 4.8
  }
]
```

---

## 🗄️ Persistência de Dados

### Banco: SQL Server (F1SEASON2025)

Principais tabelas:
- Teams
- Staffs
- Drivers
- Engineers
- AerodynamicEngineer
- PowerEngineer
- Cars

Validações críticas são garantidas por **triggers** no banco.

---

## ⚠️ Possíveis Erros

| Status Code | Motivo |
|------------|-------|
| 400 | Dados inválidos ou estrutura incompleta |
| 404 | Time ou recurso não encontrado |
| 409 | Violação de regra de negócio |
| 500 | Erro interno ou falha de banco |

---

## 📌 Observações Finais

- Arquitetura baseada em microsserviços.
- Banco central de autoridade organizacional.
- Forte uso de constraints e triggers.
- Serviço fundamental para o funcionamento da Engineering e Competition APIs.
