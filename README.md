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

## 🛣️ Registrar Novo Traçado

Cadastra um novo circuito que poderá ser utilizado no calendário do campeonato.

### Endpoint
POST /api/Competition/register-circuit

### Payload
```json
{
  "name": "Silverstone",
  "country": "UK",
  "laps": 52
}
