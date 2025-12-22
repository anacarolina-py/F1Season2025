# Competition Manager API

O módulo **Competition Service** integra a suíte **F1Simulator**, sendo encarregado de estruturar os **circuitos**, definir o **calendário oficial** e orquestrar o **progresso das etapas** (Grand Prix) durante a temporada.

## 🚀 Propósito do Módulo

Este serviço atua como o "Diretor de Prova" do sistema, com as seguintes atribuições:
* Catalogar e persistir dados dos circuitos (traçados físicos).
* Compilar e validar a grade de corridas da temporada (limite regulamentar).
* Sincronizar o início do campeonato validando o grid junto ao *Team Service*.
* Gerenciar os estados de cada etapa (`Scheduled` → `InProgress` → `Finished`).
* Assegurar a linearidade cronológica do campeonato (Round 1 precede Round 2).

## 📋 Diretrizes de Negócio

* **Restrição de Cadastro:** O sistema bloqueia a criação de pistas com mesmo nome e país para evitar redundância.
* **Integridade do Calendário:**
  * Uma temporada válida exige **estritamente 24 etapas**.
  * É vetada a inserção duplicada de um mesmo circuito na temporada vigente.
* **Validação de Grid:** O comando `StartSeason` requisita aprovação externa (API de Equipes) para garantir que carros e pilotos estão aptos.
* **Fluxo Cronológico:**
  * O início de uma prova é condicionado à conclusão da anterior.
  * Etapas finalizadas (`Finished`) tornam-se imutáveis.

## 🔗 Rotas da Aplicação

Este módulo é responsável pelo **cadastro de circuitos** e pelo **agendamento das etapas** no calendário da temporada.

---

## 🛣️ Registrar Novo Circuito

Cadastra um novo circuito que poderá ser utilizado no calendário do campeonato.

### Endpoint
`POST` **/api/Competition/register-circuit**

**Body**
```json
{
  "name": "Silverstone",
  "country": "UK",
  "laps": 52
}
```

### Agendar Etapa na Temporada

Define qual circuito será utilizado em uma determinada etapa da temporada.

### Endpoint
`POST` **/api/Competition/calendar**

**Body**
```json
{
  "circuitId": "658f1d2e..."
}
```

---

## 🚦 Controle de Temporada

### Iniciar Temporada Oficial

Valida as 24 corridas e a prontidão das equipes. Bloqueia edições no calendário após sucesso.

### Endpoint
`POST` **/api/Competition/season/start**

---

## 🏎️ Simulação de Corrida

### Validar Prontidão de Largada

Verifica se a rodada atual cumpre os pré-requisitos para ser iniciada (ex: rodada anterior finalizada).

### Endpoint
`GET` **/api/Competition/validate-start/{round}**

| Parâmetro | Tipo | Descrição |
| :--- | :--- | :--- |
| `round` | `int` | O número da rodada a ser verificada |

### Iniciar Simulação (Largada)

Altera o status da corrida para `InProgress`.

### Endpoint
`PATCH` **/api/Competition/start/{round}**

**Response (200 OK)**
```json
{
  "message": "Simulation for round 1 started successfully."
}
```

### Completar Simulação (Bandeirada Final)

Altera o status para `Finished`, calcula resultados e libera a próxima etapa.

### Endpoint
`PATCH` **/api/Competition/complete/{round}**

**Response (200 OK)**
```json
{
  "message": "Simulation for round 1 completed successfully.",
  "nextRace": {
      "round": 2,
      "circuitName": "Monaco GP",
      "status": "Scheduled"
  }
}
```

---

## 🛠️ Administrativo

### Forçar Status da Corrida

Permite ativar ou desativar uma corrida manualmente (apenas se a temporada já tiver iniciado).

### Endpoint
`PUT` **/api/Competition/{id}/status**

> **Nota:** Envia apenas o booleano ou objeto conforme configuração do serializador.

---

## ⚠️ Possíveis Erros

| Status Code | Motivo |
| :--- | :--- |
| **400 Bad Request** | Calendário incompleto/cheio, circuito duplicado ou tentativa de pular rounds. |
| **404 Not Found** | Circuito ou Corrida (Round) não encontrados no banco. |
| **500 Internal Error** | Erro de conexão com o banco de dados ou falha no TeamService. |

---

## 📌 Observações Finais

* **Persistência:** Utiliza MongoDB com coleções separadas para `circuits` e `competitions`.
* **Integração:** Comunica-se via HTTP Client com a API de Equipes para validar o grid.
* **Segurança:** Validações rigorosas de IDs (`ObjectId`) e consistência de dados.
