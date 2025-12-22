# 🏁 Race Service API

O **Race Service** integra a suíte **F1Simulator** e é responsável por **executar a simulação das corridas**, consolidar resultados esportivos e coordenar a comunicação entre os serviços de **Engineering**, **Competition** e **Team Management**.

Este serviço representa o evento da corrida em si, sendo acionado quando uma etapa está pronta para ser disputada.

---

## 🚀 Propósito do Módulo

O **Race Service** atua como o orquestrador da corrida, com as seguintes responsabilidades:

- Validar se a corrida pode ser iniciada.
- Solicitar o PD de corrida à Engineering API.
- Processar e ordenar os resultados finais.
- Persistir o resultado da etapa.
- Encerrar oficialmente a corrida no Competition Service.

---

## 📋 Diretrizes de Negócio

### Pré-requisitos para Corrida

- A temporada deve estar iniciada.
- O round atual deve estar com status `InProgress`.
- Apenas uma corrida ativa por vez é permitida.
- Corridas finalizadas não podem ser reexecutadas.

### Processamento da Corrida

1.  Solicita à **Engineering API** o PD de corrida de todos os pilotos ativos e os dados de engenharia dos carros.
2.  Ordena os pilotos por:
    - Maior PD
    - Critério de desempate interno (`DriverId`).
3.  Gera a classificação final.
4.  Persiste os dados no banco do Race Service.
5.  Notifica o **Competition Service** para finalizar o round.

---

## 🔗 Integração com Outros Serviços

### 🧠 Engineering Service

Fornece:

- PD de corrida dos pilotos.

Endpoint consumido:

```http
GET /api/engineering/pd
```

### 🏁 Competition Service

- Valida o status da corrida.
- Atualiza o estado do round para `Finished`.

### 👥 Team Management Service

Fornece:

- Pilotos ativos.
- Associação piloto ↔ time.

Utilizado para enriquecer os resultados finais.

---

## 🛣️ Rotas da Aplicação

### 🏁 Iniciar Simulação da Corrida

Inicia oficialmente a corrida do round informado.

```http
POST /api/race/start/{round}
```

| Parâmetro | Tipo | Descrição |
| :--- | :--- | :--- |
| `round` | `int` | Número da rodada |

### 🏎️ Executar Corrida

Processa a corrida completa, calcula resultados e encerra a etapa.

```http
POST /api/race/execute/{round}
```

**Response (200 OK)**

```json
{
  "round": 5,
  "status": "Finished",
  "winner": {
    "driverId": 12,
    "team": "Ferrari",
    "pd": 8.432
  }
}
```

### 📊 Consultar Resultado da Corrida

```http
GET /api/race/results/{round}
```

**Response**

```json
[
  {
    "position": 1,
    "driverId": 12,
    "team": "Ferrari",
    "pd": 8.432
  },
  {
    "position": 2,
    "driverId": 7,
    "team": "Mercedes",
    "pd": 8.210
  }
]
```

---

## 🗄️ Persistência de Dados

**Banco:** SQL Server (`RaceDB`)

```sql
CREATE TABLE RaceResults (
    Id INT IDENTITY PRIMARY KEY,
    Round INT NOT NULL,
    DriverId INT NOT NULL,
    TeamId INT NOT NULL,
    Position INT NOT NULL,
    Pd DECIMAL(6,3) NOT NULL
);
```

---

## ⚠️ Possíveis Erros

| Status Code | Motivo |
| :--- | :--- |
| `400 Bad Request` | Corrida inválida ou round incorreto |
| `404 Not Found` | Round não encontrado |
| `409 Conflict` | Corrida já finalizada |
| `500 Internal Server Error` | Falha de integração entre serviços |

---

## 🧪 Guia de Testes (Insomnia / Postman)

### Fluxo recomendado

1.  **Competition Service**
    - Criar calendário
    - Iniciar temporada
2.  **Engineering Service**
    - `POST /practice/{teamId}`
    - `POST /qualifying/{teamId}`
    - `POST /race/{teamId}`
3.  **Race Service**
    - `POST /race/start/{round}`
    - `POST /race/execute/{round}`
    - `GET /race/results/{round}`

---

## 📌 Observações Finais

- Arquitetura baseada em **microsserviços**.
- Comunicação síncrona via **HTTP Client**.
- Separação clara de responsabilidades.
- O **Race Service não calcula desempenho técnico**, apenas consome o PD.

**🏁 Race Service — Onde a temporada ganha vida.**
