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

#### Registrar novo traçado
`POST` `/api/Competition/register-circuit`

**Payload**
```json
{
  "name": "Silverstone",
  "country": "UK",
  "laps": 52
}
#### Agendar etapa na temporada
`POST` `/api/Competition/calendar`

**Payload**
```json
{
  "circuitId": "658f1d2e...", 
  "seasonId": "..." 
}
## 🏆 Controle do Campeonato

Este módulo é responsável por gerenciar o ciclo de vida de uma temporada de campeonato, desde a validação inicial até a execução e conclusão de cada corrida.
Ele garante a integridade do calendário, o fluxo correto das etapas e a comunicação com o serviço externo de equipes (TeamService).

🗓️ Gestão de Temporada
▶️ Oficializar Início de Temporada

Executa a validação completa do campeonato, garantindo que:

Existem exatamente 24 etapas cadastradas

O grid de equipes está completo e válido

Não há duplicidade ou inconsistência no calendário

Após a validação bem-sucedida:

O calendário é travado

A temporada é marcada como ativa

Endpoint

POST /api/Competition/season/start

🚦 Execução de Provas

Fluxo responsável por validar, iniciar e concluir cada etapa da temporada.

1️⃣ Checagem de Pré-Largada

Verifica se a etapa informada está apta para ser iniciada, respeitando:

Ordem das etapas

Status atual da corrida

Estado da temporada

Endpoint

GET /api/Competition/validate-start/{round}


Parâmetros

Nome	Tipo	Descrição
round	int	Número da etapa a validar
2️⃣ Autorizar Largada (Simulação)

Transiciona o status da corrida para InProgress, iniciando a simulação da etapa.

Endpoint

PATCH /api/Competition/start/{round}


Exemplo de Resposta – 200 OK

{
  "message": "Simulation for round 1 started successfully."
}

3️⃣ Bandeirada Final (Conclusão da Etapa)

Finaliza a corrida, consolida os resultados e retorna informações da próxima etapa, se existir.

Endpoint

PATCH /api/Competition/complete/{round}


Exemplo de Resposta – 200 OK

{
  "message": "Simulation for round 1 completed successfully.",
  "nextRace": {
    "round": 2,
    "circuitName": "Monaco GP",
    "status": "Scheduled"
  }
}

⚙️ Manutenção
🔧 Alterar Status Manualmente

Permite intervenção manual no status de ativação de uma corrida.
⚠️ Disponível apenas quando a temporada está ativa.

Endpoint

PUT /api/Competition/{id}/status


Payload

true

🛑 Códigos de Retorno
Status Code	Descrição
400 Bad Request	Violação de regras de negócio (calendário incompleto, duplicidade de eventos, tentativa de pular etapas)
404 Not Found	Recurso inexistente (Pista ou Etapa não encontrada)
500 Internal Server Error	Falha na persistência ou indisponibilidade do TeamService
💾 Detalhes Técnicos

Database: MongoDB

Coleções segregadas:

circuits

competitions

Comunicação Externa:

Uso de HttpClient para validação síncrona com a API de Equipes (TeamService)

Segurança e Consistência:

Validação rigorosa de tipos ObjectId

Garantia de consistência transacional lógica durante as mudanças de estado da corrida
