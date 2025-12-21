# Team Management Service API

O **Team Management Service** integra a suíte **F1Simulator** e é responsável por **gerenciar equipes, profissionais e carros**, além de **publicar eventos de times válidos via RabbitMQ** para consumo por outros serviços, como o **Engineering Service**.

Este serviço atua como a **fonte oficial de dados estruturais** da temporada.

---

## 🚀 Propósito do Módulo

O Team Management representa o **backoffice das equipes**, com as seguintes responsabilidades:

- Cadastro e gestão de **Times (Teams)**.
- Gerenciamento de **Staff** (pilotos, engenheiros e chefes).
- Associação correta de **2 carros por time**.
- Garantia de integridade de vínculos (pilotos, engenheiros e carros do mesmo time).
- Validação completa de um time antes de torná-lo **Ativo**.
- Publicação de eventos de times via **RabbitMQ**.

---

## 📋 Diretrizes de Negócio

### Regras de Time
- Cada time ativo deve possuir **exatamente**:
  - 2 chefes
  - 2 pilotos ativos
  - 2 engenheiros de potência
  - 2 engenheiros aerodinâmicos
  - 2 carros ativos
- Todos os profissionais associados a um carro **devem pertencer ao mesmo time**.
- Um time só pode mudar o status para **Ativo** se estiver completo.

---

### Regras de Staff
- Nomes não podem conter números.
- Idade permitida: **18 a 120 anos**.
- Experiência permitida: **1.0 a 5.0**.

---

### Regras de Carro
- Modelo no padrão `ABC12`.
- Coeficientes técnicos entre **0 e 10**.
- Peso entre **700 e 999.99**.
- Máximo de **2 carros do mesmo modelo por time**.

---

## 📨 Integração Assíncrona com RabbitMQ

O Team Management é responsável por **publicar times válidos** para outros serviços por meio de filas.

### 📤 ProducerTeam

Endpoint responsável por **enviar dados de um time para a fila**.

Fluxo:
1. Recebe um `TeamResponseDTO` via HTTP.
2. Valida:
   - Nome não nulo
   - Nome dentro dos padrões
   - Time não duplicado
3. Converte o DTO em entidade `Team`.
4. Publica o objeto serializado na fila **TeamQueue**.

**Fila:** `TeamQueue`

---

### 📥 ConsumerTeam

Processo responsável por **consumir mensagens da fila**.

Fluxo:
1. Lê mensagens da `TeamQueue`.
2. Desserializa o JSON em objeto `Team`.
3. Envia o objeto ao **Repository**.
4. Persiste o time no banco de dados.

---

## 🔗 Integração com Outros Serviços

### Engineering Service
- Consome dados estruturais de times ativos.
- Utiliza:
  - Carros
  - Pilotos
  - Engenheiros e experiências

---

## 🛣️ Rotas da Aplicação

### 🏁 Times

#### Criar Time
POST /api/team

#### Ativar Time
PUT /api/team/{teamId}/activate

---

### 📤 Publicar Time na Fila

POST /api/team/producer

---

## 🗄️ Persistência de Dados

**Banco:** SQL Server (F1SEASON2025)

### Principais Tabelas
- Teams
- Staffs
- Drivers
- Engineers
- AerodynamicEngineer
- PowerEngineer
- Cars

Todas as validações críticas são garantidas por **constraints** e **triggers** no banco.

---

## ⚠️ Possíveis Erros

| Status Code | Motivo |
|------------|-------|
| 400 Bad Request | Dados inválidos ou time incompleto |
| 404 Not Found | Time ou recurso não encontrado |
| 409 Conflict | Time já existente |
| 500 Internal Server Error | Falha no banco ou no RabbitMQ |

---

## 📌 Observações Finais

- Arquitetura baseada em **Microsserviços**.
- Comunicação **HTTP + Mensageria (RabbitMQ)**.
- Persistência isolada por contexto.
- Forte uso de validações de domínio.
- Serviço essencial para inicialização da temporada.

