# :racing_car: F1Simulator — Simulador de Temporada Fórmula 1 2025 (Backend)

O **F1Simulator** é um sistema backend desenvolvido como **projeto final de treinamento**, com foco em **arquitetura de microsserviços**, **integração entre serviços via HTTP** e **simulação de regras complexas de negócio**, inspirado na temporada de **Fórmula 1 de 2025**.

O projeto não possui interface gráfica. Todo o foco está na **robustez arquitetural**, **desacoplamento entre serviços** e **organização de regras de negócio distribuídas**.

---

## :dart: Objetivo do Projeto

Simular uma temporada completa de Fórmula 1 com:

- Calendário oficial de **24 corridas**
- **11 equipes**, cada uma com:
  - 2 pilotos
  - 2 carros
  - Staff técnico completo
- Evolução técnica contínua dos carros
- Sistema de handicap dos pilotos
- Algoritmo determinístico com fator de sorte
- Comunicação entre serviços majoritariamente via **HTTP**
- Uso pontual de **mensageria assíncrona** no contexto de equipes

---

## :jigsaw: Arquitetura de Microsserviços

O sistema é composto por **4 microsserviços independentes**, cada um com **responsabilidade clara**, banco próprio e regras de negócio isoladas.

### :small_blue_diamond: Serviços do Sistema

| Serviço | Responsabilidade Principal |
|--------|----------------------------|
| Competition Manager API | Gerencia circuitos, calendário e status das corridas |
| Team Management Service | Cadastro de equipes, pilotos, staff e carros |
| Engineering Service | Evolução técnica, handicap e cálculo de PD |
| Race Service | Orquestra a simulação da corrida e consolida resultados |

---

## :brain: Visão Geral dos Serviços

### :checkered_flag: Competition Manager API
Responsável por estruturar a temporada:
- Cadastro de circuitos
- Montagem do calendário (24 etapas obrigatórias)
- Controle de status das corridas
- Garantia da ordem cronológica da temporada

:page_facing_up: `/CompetitionService/README.md`

---

### :busts_in_silhouette: Team Management Service
Backoffice das equipes:
- Cadastro completo de times
- Gestão de pilotos, engenheiros e chefes
- Validação estrutural do time antes de ativação
- Publicação de eventos de **equipe validada** via RabbitMQ

:page_facing_up: `/TeamManagementService/README.md`

---

### :wrench: Engineering Service
Núcleo técnico da simulação:
- Evolução de coeficientes aerodinâmicos e de potência
- Influência da experiência dos engenheiros
- Atualização do handicap dos pilotos
- Cálculo do Performance Delta (PD)

:page_facing_up: `/EngineeringService/README.md`

---

### :racing_car: Race Service
Motor da corrida:
- Valida se a corrida pode iniciar
- Solicita PD à Engineering API
- Processa e ordena os resultados
- Persiste classificação final
- Finaliza o round no Competition Service

:page_facing_up: `/RaceService/README.md`

---

## :arrows_counterclockwise: Fluxo Completo da Temporada

1. Cadastro dos circuitos
2. Montagem do calendário da temporada
3. Criação e ativação das equipes
4. Início oficial da temporada
5. Para cada round:
   - Treino livre (Engineering)
   - Qualificação (Engineering)
   - Corrida (Race + Engineering)
6. Atualização de handicap e avanço do campeonato

---

## :package: Mensageria com RabbitMQ

A **mensageria é utilizada exclusivamente no Team Management Service**, com o objetivo de:

- Validar a estrutura completa das equipes
- Publicar eventos indicando que um time está pronto para competir
- Evitar acoplamento direto entre o processo de validação de times e outros serviços

Os demais serviços do sistema **não consomem eventos** e se comunicam **apenas via HTTP**, mantendo a arquitetura simples e objetiva para o escopo do projeto.

---

## :test_tube: Endpoints Principais

| Método | Endpoint | Serviço |
|------|----------|--------|
| POST | `/api/competition/season/start` | Competition |
| POST | `/api/engineering/race/{teamId}` | Engineering |
| POST | `/api/race/execute/{round}` | Race |
| GET | `/api/standings` | Race |
| GET | `/api/teams/{id}/telemetry` | Team |

Coleções completas disponíveis via **Insomnia**.

---

## :hammer_and_wrench: Stack Tecnológica

- C# (.NET 9+)
- ASP.NET Core Web API
- RabbitMQ (uso pontual)
- SQL Server
- MongoDB
- Dapper
- HTTP Client

---

## :arrow_forward: Como Executar o Projeto (Local)

> :warning: Este projeto **não utiliza Docker Compose**

### Ordem Recomendada

1. Subir o RabbitMQ
2. Iniciar Team Management Service
3. Iniciar Competition Manager API
4. Iniciar Engineering Service
5. Iniciar Race Service

Cada serviço possui banco próprio, porta independente e configuração isolada.

---

## :triangular_ruler: Considerações Arquiteturais

- Separação clara de Bounded Contexts
- Persistência isolada por serviço
- Comunicação síncrona via HTTP como padrão
- Mensageria utilizada de forma específica e controlada
- Organização em camadas:
  - Controllers
  - Services
  - Repositories
  - DTOs
  - Domain

---

## :checkered_flag: Conclusão

O **F1Simulator** representa a aplicação prática dos principais conceitos estudados ao longo do treinamento, unindo **arquitetura de microsserviços**, **integração entre APIs**, **separação de responsabilidades** e **regras de negócio complexas** em um sistema coeso e funcional.

O uso de mensageria foi aplicado de forma **pontual e consciente**, demonstrando entendimento sobre quando a complexidade adicional é realmente necessária. O projeto foi estruturado para simular cenários reais de sistemas distribuídos, priorizando **clareza arquitetural**, **evolução independente dos serviços** e **boas práticas de modelagem de domínio**.

Assim como na Fórmula 1, onde estratégia, confiabilidade e trabalho em equipe definem o resultado final, este sistema evidencia que **decisão técnica bem fundamentada** é tão importante quanto performance.
