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

### 🗺️ Gestão de Pistas e Grade

Este módulo é responsável pelo **cadastro de circuitos** e pelo **agendamento das etapas** no calendário da temporada.

---

## 🛣️ Registrar Novo Circuito

Cadastra um novo circuito que poderá ser utilizado no calendário do campeonato.

### Endpoint
POST /api/Competition/register-circuit

```json
{
  "name": "Silverstone",
  "country": "UK",
  "laps": 52
}
```
### Agendar etapa na temporada

### Endpoint

POST /api/Competition/calendar

```json
{
  "circuitId": "658f1d2e...",
}
```
## 🚦 Controle de Temporada

Iniciar Temporada Oficial

### Endpoint

POST /api/Competition/season/start

Valida as 24 corridas e a prontidão das equipes. Bloqueia edições no calendário após sucesso.

## 🏎️ Simulação de Corrida

Validar se a corrida pode começar!

### Endpoint

GET /api/Competition/validate-start/{round}

Path Params | Nome | Tipo | Descrição | | :--- | :--- | :--- | | round | int | O número da rodada a ser verificada |

Iniciar Simulação (Largar)
PATCH /api/Competition/start/{round}

Altera o status da corrida para InProgress.

Resposta da requisição (200 OK)
