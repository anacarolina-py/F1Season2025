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

Agendar etapa na temporadaPOST /api/Competition/calendarPayloadJSON{
  "circuitId": "658f1d2e...", 
  "seasonId": "..." 
}
A numeração do Round é gerada sequencialmente pelo backend.🏆 Controle do CampeonatoOficializar Início de TemporadaPOST /api/Competition/season/startExecuta a validação das 24 etapas e do grid de equipes. Após o sucesso, o calendário é travado.🚦 Execução de ProvasChecagem de Pré-LargadaGET /api/Competition/validate-start/{round}Parâmetros de Rota| Parâmetro | Tipo | Descrição || :--- | :--- | :--- || round | int | Número da etapa para validação |Autorizar Largada (Simulação)PATCH /api/Competition/start/{round}Transiciona o status da corrida para InProgress.Retorno (200 OK)JSON{
  "message": "Simulation for round 1 started successfully."
}
Bandeirada Final (Concluir)PATCH /api/Competition/complete/{round}Define o status como Finished e provê dados da etapa subsequente.Retorno (200 OK)JSON{
  "message": "Simulation for round 1 completed successfully.",
  "nextRace": {
      "round": 2,
      "circuitName": "Monaco GP",
      "status": "Scheduled"
  }
}
⚙️ ManutençãoAlterar Status ManualmentePUT /api/Competition/{id}/statusPermite a intervenção manual no status de ativação da corrida (somente com temporada ativa).PayloadJSONtrue
🛑 Códigos de RetornoStatus CodeCenário400 Bad RequestViolação de regra (calendário cheio, duplicidade, pular etapas).404 Not FoundRecurso (Pista/Etapa) inexistente no banco.500 Internal ErrorFalha na persistência ou indisponibilidade do TeamService.💾 Detalhes TécnicosDatabase: Implementado com MongoDB, segregando coleções para circuits e competitions.Comunicação: Utiliza HttpClient para validação síncrona com a API de Equipes.Segurança: Validação estrita de tipos ObjectId e consistência transacional lógica.
